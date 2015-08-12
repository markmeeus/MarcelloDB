using System;
using NUnit.Framework;
using MarcelloDB.Records;
using MarcelloDB.Index;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class EmptyRecordIndexNodeSerializerTest
    {
        [SetUp]
        public void Initialize()
        {
        }

        [Test]
        public void Serializes_OK()
        {
            var node = new Node<EmptyRecordIndexKey, Int64>(12);
            var serializer = new EmptyRecordIndexNodeSerializer();
            node.ChildrenAddresses.Add(123);
            node.ChildrenAddresses.Add(456);
            node.ChildrenAddresses.Add(678);
            node.EntryList.Add(new Entry<EmptyRecordIndexKey, Int64>{
                Key = new EmptyRecordIndexKey{S = 123, A = 456 },
                Pointer = 678
            });
            node.EntryList.Add(new Entry<EmptyRecordIndexKey, Int64>{
                Key = new EmptyRecordIndexKey{S = 321, A = 654 },
                Pointer = 876
            });
            var deserialized = serializer.Deserialize(
                                   serializer.Serialize(node)
                               );

            Assert.AreEqual(node.Degree, deserialized.Degree);
            Assert.AreEqual(node.ChildrenAddresses.Addresses, deserialized.ChildrenAddresses.Addresses);
            Assert.AreEqual(node.EntryList.Count, deserialized.EntryList.Count);

            Assert.AreEqual(node.EntryList[0].Pointer, deserialized.EntryList[0].Pointer);
            Assert.AreEqual(node.EntryList[0].Key, deserialized.EntryList[0].Key);
            Assert.AreEqual(node.EntryList[1].Pointer, deserialized.EntryList[1].Pointer);
            Assert.AreEqual(node.EntryList[1].Key, deserialized.EntryList[1].Key);

        }
    }
}

