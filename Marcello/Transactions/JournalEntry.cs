using System;

namespace Marcello.Transactions
{
    public class JournalEntry
    {
        public Guid ID { get; set;}

        public string ObjectTypeName { get; set; }

        public Int64 Address { get; set; }

        public byte[] Data { get; set; }

        internal JournalEntry()
        {
            this.ID = Guid.NewGuid();
        }
    }
}

