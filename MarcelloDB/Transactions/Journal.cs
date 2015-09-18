using System;
using MarcelloDB.Collections;
using System.Reflection;
using System.Collections.Generic;
using MarcelloDB.Storage;
using System.Linq;
using MarcelloDB.Helpers;
using MarcelloDB.Transactions;
using MarcelloDB.Serialization;
using MarcelloDB.Storage.StreamActors;

namespace MarcelloDB.Transactions
{
    internal class Journal : SessionBoundObject
    {
        internal const string JOURNAL_COLLECTION_NAME = "journal";

        List<JournalEntry> UncommittedEntries { get;set; }

        Dictionary<string, Writer> Writers {get;set;}

        Writer JournalWriter { get; set; }
        Reader JournalReader { get; set; }

        internal Journal (Session session) : base(session)
        {
            this.Writers = new Dictionary<string, Writer>();
            this.JournalWriter = new Writer(this.Session, JOURNAL_COLLECTION_NAME);
            this.JournalReader = new Reader(this.Session, JOURNAL_COLLECTION_NAME);
            this.UncommittedEntries = new List<JournalEntry> ();
            }

        internal void Write (string streamName, long address, byte[] data)
        {
            var entry = new JournalEntry()
            {
                StreamName = streamName,
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
            var entries = this.AllEntriesForStreamName(collectionName);
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
            if(!this.Writers.ContainsKey(entry.StreamName))
            {
                var writer = new Writer(this.Session, entry.StreamName);
                this.Writers[entry.StreamName] = writer;
            }
            return this.Writers[entry.StreamName];
        }

        IEnumerable<JournalEntry> AllEntriesForStreamName(string streamName)
        {
            return this.UncommittedEntries.Where(e => e.StreamName == streamName);
        }

        void PersistJournal(TransactionJournal transactionJournal){
            var bson = this.Session.SerializerResolver.SerializerFor<TransactionJournal>()
                .Serialize(transactionJournal);

            this.JournalWriter.Write(0,
                new BufferWriter(new byte[sizeof(int)])
                    .WriteInt32(bson.Length).GetTrimmedBuffer()
            );
            this.JournalWriter.Write(sizeof(int), bson);
        }

        TransactionJournal LoadJournal(){
            var length = new BufferReader(
                this.JournalReader.Read(0, sizeof(int))
            ).ReadInt32();
            if (length > 0)
            {
                var bytes = this.JournalReader.Read(sizeof(int), length);
                return this.Session.SerializerResolver.SerializerFor<TransactionJournal>()
                    .Deserialize(bytes);
            }
            return null;
        }

        void ClearJournal(){
            this.JournalWriter.Write(0,
                //add 0 as length, ignore all after
                new BufferWriter(new byte[sizeof(int)])
                .WriteInt32(0).GetTrimmedBuffer()
            );
        }
    }
}

