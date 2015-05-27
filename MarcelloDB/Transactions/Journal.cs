using System;
using MarcelloDB.Collections;
using System.Reflection;
using System.Collections.Generic;
using MarcelloDB.Storage;
using System.Linq;
using MarcelloDB.Helpers;
using MarcelloDB.Transactions.__;
using MarcelloDB.Serialization;
using MarcelloDB.Storage.StreamActors;

namespace MarcelloDB.Transactions
{
    internal class Journal
    {
        const string JOURNAL_COLLECTION_NAME = "journal";

        Session Session { get; set; }

        List<JournalEntry> UncommittedEntries { get;set; }

        Dictionary<string, Writer> Writers {get;set;}

        Writer JournalWriter { get; set; }
        Reader JournalReader { get; set; }

        internal Journal (Session session)
        {
            this.Session = session;
            this.Writers = new Dictionary<string, Writer>();
            this.JournalWriter = new Writer(this.Session, JOURNAL_COLLECTION_NAME);
            this.JournalReader = new Reader(this.Session, JOURNAL_COLLECTION_NAME);
            this.UncommittedEntries = new List<JournalEntry> ();
        }

        internal void Write (string collectionName, long address, byte[] data)
        {
            var entry = new JournalEntry()
            {
                CollectionName = collectionName,
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
                var writer = GetWriterForEntry(entry);
                writer.Write(entry.Address, entry.Data);
            }
            ClearJournal();
        }

        internal void ApplyToData(string collectionName, Int64 address, byte[] data)
        {
            var entries = this.AllEntriesForCollectionName(collectionName);          
            foreach (var entry in entries) 
            {
                DataHelper.CopyData(entry.Address, entry.Data, address, data);
            }
        }

        internal void ClearUncommitted()
        {
            this.UncommittedEntries.Clear();
        }           

        Writer GetWriterForEntry(JournalEntry entry)
        {
            if(!this.Writers.ContainsKey(entry.CollectionName))
            {
                var writer = new Writer(this.Session, entry.CollectionName);
                this.Writers[entry.CollectionName] = writer;
            }
            return this.Writers[entry.CollectionName];
        }
            
        IEnumerable<JournalEntry> AllEntriesForCollectionName(string collectionName)
        {
            return this.UncommittedEntries.Where(e => e.CollectionName == collectionName);         
        }                  
            
        void PersistJournal(TransactionJournal transactionJournal){
            var bson = new BsonSerializer<TransactionJournal>().Serialize(transactionJournal);

            this.JournalWriter.Write(0, 
                new BufferWriter(new byte[sizeof(int)], BitConverter.IsLittleEndian)
                    .WriteInt32(bson.Length).GetTrimmedBuffer()
            );
            this.JournalWriter.Write(sizeof(int), bson); 
        }

        TransactionJournal LoadJournal(){
            var length = new BufferReader(
                this.JournalReader.Read(0, sizeof(int)), BitConverter.IsLittleEndian
            ).ReadInt32();
            if (length > 0)
            {
                var bytes = this.JournalReader.Read(sizeof(int), length);
                return new BsonSerializer<TransactionJournal>().Deserialize(bytes);
            }
            return null;
        }

        void ClearJournal(){
            this.JournalWriter.Write(0, 
                //add 0 as length, ignore all after
                new BufferWriter(new byte[sizeof(int)], BitConverter.IsLittleEndian)
                .WriteInt32(0).GetTrimmedBuffer()
            );
        }
    }
}

