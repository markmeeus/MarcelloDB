using System;
using System.Collections.Generic;

namespace MarcelloDB.Transactions
{
    internal static class TimeStamp
    {
        static long _lastStamp;

        internal static long Next(){
            return ++_lastStamp;
        }
    }
        
    internal class TransactionJournal
    {
        public String ID { get; set;}

        public long Stamp{ get; set;}

        public List<JournalEntry> Entries { get; set; }

        public TransactionJournal()
        {
            this.Entries = new List<JournalEntry>();
            this.ID = Guid.NewGuid().ToString();
            this.Stamp = TimeStamp.Next();
        }
    }

    public class JournalEntry
    {
        public long Stamp { get; set; }

        public string CollectionName { get; set; }

        public Int64 Address { get; set; }

        public byte[] Data { get; set; }

        public JournalEntry(){
            this.Stamp = TimeStamp.Next();
        }
    }
}

