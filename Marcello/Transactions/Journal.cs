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

        Dictionary<Type, StorageEngine> StorageEngines { get; set; }

        internal Journal (Marcello session)
        {
            this.Session = session;
            this.StorageEngines = new Dictionary<Type, StorageEngine>();
            JournalCollection = session.Collection<JournalEntry>();
            JournalCollection.DisableJournal(); //no journalling for the journal ofcourse
        }

        internal void Write (Type objectType, long address, byte[] data)
        {
            var entry = new JournalEntry()
            {
                ObjectTypeName = objectType.AssemblyQualifiedName,
                Address = address, 
                Data = data 
            };
            this.JournalCollection.Persist (entry);
        }

        internal void Apply()
        {
            foreach (var entry in this.JournalCollection.All) 
            {
                var engine = GetStorageEngineForEntry(entry);
                engine.Write (entry.Address, entry.Data);
            }

            this.Clear();
        }

        internal void ApplyToData(Type objectType, Int64 address, byte[] data)
        {
            var lastAddress = address + data.Length -1;

            var entries = this.JournalCollection.All.Where(e => e.ObjectTypeName == objectType.AssemblyQualifiedName);

            foreach (var entry in entries) 
            {
                DataHelper.CopyData(entry.Address, entry.Data, address, data);
            }
        }

        internal void Clear()
        {
            this.JournalCollection.DestroyAll();
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
    }
}

