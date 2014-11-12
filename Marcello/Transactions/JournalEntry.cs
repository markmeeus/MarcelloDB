using System;
using System.Collections.Generic;

namespace Marcello.Transactions.__
{
    public class TransactionJournal
    {
        public Guid ID { get; set;}
    
        public List<JournalEntry> Entries { get; set; }

        public TransactionJournal()
        {
            this.Entries = new List<JournalEntry>();
            this.ID = Guid.NewGuid();
        }
    }

    public class JournalEntry
    {
        public string ObjectTypeName { get; set; }

        public Int64 Address { get; set; }

        public byte[] Data { get; set; }
    }
}

