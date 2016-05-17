using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Transactions;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class TransactionJournalSerializerTest
    {
        TransactionJournalSerializer _serializer;

        [SetUp]
        public void Initialize()
        {
            _serializer = new TransactionJournalSerializer();
        }

        [Test]
        public void SerializesTansactionJournal()
        {
            var journal = new TransactionJournal();
            journal.Entries.Add(new JournalEntry
                {
                    Address = 1,
                    Data = new byte[]{ 2, 3, 4 },
                    Stamp = 5,
                    StreamName = "TestStream 1"
                });

            journal.Entries.Add(new JournalEntry
                {
                    Address = 6,
                    Data = new byte[]{ 7, 8, 9 },
                    Stamp = 10,
                    StreamName = "TestStream 2"
                });

            var deserialized = _serializer.Deserialize(_serializer.Serialize(journal));
            Assert.AreEqual(2, deserialized.Entries.Count);

            Assert.AreEqual(1, deserialized.Entries[0].Address);
            Assert.AreEqual(new byte[]{2, 3, 4}, deserialized.Entries[0].Data);
            Assert.AreEqual(5, deserialized.Entries[0].Stamp);
            Assert.AreEqual("TestStream 1", deserialized.Entries[0].StreamName);

            Assert.AreEqual(6, deserialized.Entries[1].Address);
            Assert.AreEqual(new byte[]{7, 8, 9}, deserialized.Entries[1].Data);
            Assert.AreEqual(10, deserialized.Entries[1].Stamp);
            Assert.AreEqual("TestStream 2", deserialized.Entries[1].StreamName);
        }
    }
}

