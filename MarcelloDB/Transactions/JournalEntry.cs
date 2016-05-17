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
        public List<JournalEntry> Entries { get; set; }

        public TransactionJournal()
        {
            this.Entries = new List<JournalEntry>();
        }
    }

    internal class JournalEntry
    {
        public long Stamp { get; set; }

        public string StreamName { get; set; }

        public Int64 Address { get; set; }

        public byte[] Data { get; set; }

        public JournalEntry(){
            this.Stamp = TimeStamp.Next();
        }
    }
}

