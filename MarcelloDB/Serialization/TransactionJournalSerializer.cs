using System;
using MarcelloDB.Transactions;
using System.Linq;
using System.Text;

namespace MarcelloDB.Serialization
{
    internal class TransactionJournalSerializer : IObjectSerializer<TransactionJournal>
    {
        const int FORMAT_VERSION = 1;

        public TransactionJournalSerializer()
        {
        }

        #region IObjectSerializer implementation

        public byte[] Serialize(TransactionJournal journal)
        {
            var expectedSize = sizeof(byte) + sizeof(Int32) + journal.Entries.Select(entry =>
                {
                    return sizeof(Int64) +
                        sizeof(Int32) +
                        entry.Data.Length +
                        sizeof(Int64) +
                        sizeof(Int32) + Encoding.UTF8.GetByteCount(entry.StreamName);
                }).Sum();

            var writer = new BufferWriter(new byte[expectedSize]);

            writer.WriteByte(FORMAT_VERSION);
            writer.WriteInt32(journal.Entries.Count);

            int count = 0;
            foreach (var entry in journal.Entries)
            {
                WriteEntry(writer, entry);
            }
            return writer.GetTrimmedBuffer();
        }

        public TransactionJournal Deserialize(byte[] bytes)
        {
            var reader = new BufferReader(bytes);
            reader.ReadByte(); //FORMAT_VERSION

            var journal = new TransactionJournal();
            var entryCount = reader.ReadInt32();

            for (int i = 0; i < entryCount; i++)
            {
                journal.Entries.Add(ReadEntry(reader));
            }
            return journal;
        }

        #endregion

        void WriteEntry(BufferWriter writer, JournalEntry entry)
        {
            writer.WriteInt64(entry.Address);
            writer.WriteInt32(entry.Data.Length);
            writer.WriteBytes(entry.Data);
            writer.WriteInt64(entry.Stamp);
            var streamNameBytes = Encoding.UTF8.GetBytes(entry.StreamName);
            writer.WriteInt32(streamNameBytes.Length);
            writer.WriteBytes(streamNameBytes);
        }

        JournalEntry ReadEntry(BufferReader reader)
        {
            var journalEntry = new JournalEntry();
            journalEntry.Address = reader.ReadInt64();
            var dataLength = reader.ReadInt32();
            journalEntry.Data = reader.ReadBytes(dataLength);
            journalEntry.Stamp = reader.ReadInt64();
            var streamNameLength = reader.ReadInt32();
            var streamNameBytes = reader.ReadBytes(streamNameLength);

            journalEntry.StreamName = Encoding.UTF8.GetString(streamNameBytes, 0, streamNameBytes.Length);
            return journalEntry;
        }
    }
}

