using System;
using NUnit.Framework;
using MarcelloDB.Records;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class IndexMetaRecordSerializerTest
    {
        IndexMetaRecord _record;
        IndexMetaRecord _deserializedRecord;

        [SetUp]
        public void Initialize()
        {
            _record = new IndexMetaRecord
                {
                    RootNodeAddress = 1,
                    NumberOfNodes = 2,
                    NumberOfEntries = 3,
                    TotalAllocatedDataSize = 4,
                    TotalAllocatedSize = 5
                };

            var serializer = new IndexMetaRecordSerializer();
            _deserializedRecord = serializer.Deserialize(
                serializer.Serialize(_record));
        }

        [Test]
        public void Serializes_RootRecordAddress()
        {
            Assert.AreEqual(_record.RootNodeAddress, _deserializedRecord.RootNodeAddress);
        }

        [Test]
        public void Serializes_NumberOfEntries()
        {
            Assert.AreEqual(_record.NumberOfEntries, _deserializedRecord.NumberOfEntries);
        }

        [Test]
        public void Serializes_NumberOfNodes()
        {
            Assert.AreEqual(_record.NumberOfNodes, _deserializedRecord.NumberOfNodes);
        }

        [Test]
        public void Serializes_TotalAllocatedSize()
        {
            Assert.AreEqual(_record.TotalAllocatedSize, _deserializedRecord.TotalAllocatedSize);
        }

        [Test]
        public void Serializes_TotalAllocatedDataSize()
        {
            Assert.AreEqual(_record.TotalAllocatedDataSize, _deserializedRecord.TotalAllocatedDataSize);
        }
    }
}

