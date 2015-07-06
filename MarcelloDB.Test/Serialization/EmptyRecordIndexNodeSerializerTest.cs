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
            node.Entries.Add(new Entry<EmptyRecordIndexKey, Int64>{
                Key = new EmptyRecordIndexKey{S = 123, A = 456 }, 
                Pointer = 678
            });
            node.Entries.Add(new Entry<EmptyRecordIndexKey, Int64>{
                Key = new EmptyRecordIndexKey{S = 321, A = 654 }, 
                Pointer = 876
            });
            var deserialized = serializer.Deserialize(
                                   serializer.Serialize(node)
                               );

            Assert.AreEqual(node.Degree, deserialized.Degree);
            Assert.AreEqual(node.ChildrenAddresses, deserialized.ChildrenAddresses);
            Assert.AreEqual(node.Entries.Count, deserialized.Entries.Count);

            Assert.AreEqual(node.Entries[0].Pointer, deserialized.Entries[0].Pointer);
            Assert.AreEqual(node.Entries[0].Key, deserialized.Entries[0].Key);
            Assert.AreEqual(node.Entries[1].Pointer, deserialized.Entries[1].Pointer);
            Assert.AreEqual(node.Entries[1].Key, deserialized.Entries[1].Key);

        }
    }
}

