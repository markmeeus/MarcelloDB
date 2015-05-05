using System;
using MarcelloDB.Collections;
using System.Reflection;
using System.Collections.Generic;
using MarcelloDB.Storage;
using System.Linq;
using MarcelloDB.Helpers;
using MarcelloDB.Transactions.__;
using MarcelloDB.Serialization;

namespace MarcelloDB.Transactions
{
    internal class Journal
    {
        Marcello Session { get; set; }

        List<JournalEntry> UncommittedEntries { get;set; }

        Dictionary<Type, StorageEngine> StorageEngines { get; set; }

        StorageEngine<TransactionJournal> JournalStorageEngine { get; set; }

        internal Journal (Marcello session)
        {
            this.Session = session;
            this.StorageEngines = new Dictionary<Type, StorageEngine>();
            this.JournalStorageEngine = new StorageEngine<TransactionJournal>(this.Session);
            this.UncommittedEntries = new List<JournalEntry> ();
        }

        internal void Write (Type objectType, long address, byte[] data)
        {
            var entry = new JournalEntry()
            {
                ObjectTypeName = objectType.AssemblyQualifiedName,
                Address = address, 
                Data = data 
            };
            this.UncommittedEntries.Add(entry);
        }

        internal void Commit()
        {
            if (this.UncommittedEntries.Count == 0)
                return;

            var transactionJournal = new TransactionJournal();
            foreach (var entry in this.UncommittedEntries) 
            {
                transactionJournal.Entries.Add(entry);
            }
            PersistJournal(transactionJournal);
            this.ClearUncommitted();
        }

        internal void Apply()
        {           
            var journal = LoadJournal();
            if (journal == null)
            {
                return; //nothing to apply
            }

            foreach (var entry in journal.Entries.OrderBy(e => e.Stamp)) {
                var engine = GetStorageEngineForEntry(entry);
                engine.DisableJournal ();
                engine.Write (entry.Address, entry.Data);
            }
            ClearJournal();
        }

        internal void ApplyToData(Type objectType, Int64 address, byte[] data)
        {
            var entries = this.AllEntriesForObjectType(objectType);          
            foreach (var entry in entries) 
            {
                DataHelper.CopyData(entry.Address, entry.Data, address, data);
            }
        }

        internal void ClearUncommitted()
        {
            this.UncommittedEntries.Clear();
        }

        StorageEngine GetStorageEngineForEntry(JournalEntry entry)
        {
            var type = Type.GetType(entry.ObjectTypeName);
            if (!this.StorageEngines.ContainsKey (type)) 
            {
                var genericType = typeof(StorageEngine<>).GetTypeInfo().MakeGenericType(new Type[] { type });
                var engine = (StorageEngine)Activator.CreateInstance (genericType, new object[] {this.Session});
                this.StorageEngines.Add(type, engine);
            }

            return this.StorageEngines[type];
        }

        IEnumerable<JournalEntry> AllEntriesForObjectType(Type objectType)
        {
            return this.UncommittedEntries.Where(e => e.ObjectTypeName == objectType.AssemblyQualifiedName);         
        }                  


        void PersistJournal(TransactionJournal transactionJournal){
            var bson = new BsonSerializer<TransactionJournal>().Serialize(transactionJournal);

            this.JournalStorageEngine.Write(0, 
                new BufferWriter(new byte[sizeof(int)], BitConverter.IsLittleEndian)
                    .WriteInt32(bson.Length).GetTrimmedBuffer()
            );
            this.JournalStorageEngine.Write(sizeof(int), bson); 
        }

        TransactionJournal LoadJournal(){
            var length = new BufferReader(
                this.JournalStorageEngine.Read(0, sizeof(int)), BitConverter.IsLittleEndian
            ).ReadInt32();
            var bytes = this.JournalStorageEngine.Read(sizeof(int), length);
            return new BsonSerializer<TransactionJournal>().Deserialize(bytes);
        }

        void ClearJournal(){
            this.JournalStorageEngine.Write(0, 
                //add 0 as length, ignore all after
                new BufferWriter(new byte[sizeof(int)], BitConverter.IsLittleEndian)
                .WriteInt32(0).GetTrimmedBuffer()
            );
        }
    }
}

