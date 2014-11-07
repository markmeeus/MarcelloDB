using System;
using Marcello.Collections;
using System.Reflection;
using System.Collections.Generic;
using Marcello.Storage;
using System.Linq;
using Marcello.Helpers;

namespace Marcello.Transactions
{
    internal class Journal
    {
        Marcello Session { get; set; }

        Collection<JournalEntry> JournalCollection { get; set; }               

        List<JournalEntry> UncommittedEntries { get;set; }

        Dictionary<Type, StorageEngine> StorageEngines { get; set; }

        internal Journal (Marcello session)
        {
            this.Session = session;
            this.StorageEngines = new Dictionary<Type, StorageEngine>();
            this.JournalCollection = session.Collection<JournalEntry>();
            this.JournalCollection.DisableJournal(); //no journalling for the journal ofcourse
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
            foreach (var entry in this.UncommittedEntries) 
            {
                this.JournalCollection.Persist(entry);
            }
            this.ClearUncommitted();
        }

        internal void Apply()
        {
            foreach (var entry in this.JournalCollection.All) 
            {
                var engine = GetStorageEngineForEntry(entry);
                engine.Write (entry.Address, entry.Data);
            }

            this.JournalCollection.DestroyAll();
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
            var type = Type.GetType (entry.ObjectTypeName);
            if (!this.StorageEngines.ContainsKey (type)) 
            {
                var genericType = typeof(StorageEngine<>).GetTypeInfo().MakeGenericType(new Type[] { type });
                var engine = (StorageEngine)Activator.CreateInstance (genericType, new object[] {this.Session});
                engine.DisableJournal ();
                this.StorageEngines.Add(type, engine);
            }

            return this.StorageEngines[type];
        }

        IEnumerable<JournalEntry> AllEntriesForObjectType(Type objectType)
        {
            var allEntries = this.JournalCollection.All.Where(e => e.ObjectTypeName == objectType.AssemblyQualifiedName).ToList();
            allEntries.AddRange(this.UncommittedEntries.Where(e => e.ObjectTypeName == objectType.AssemblyQualifiedName));
            return allEntries;
        }
    }
}

