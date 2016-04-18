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
            Assert.AreEqual(node.ChildrenAddresses, deserialized.ChildrenAddresses);
            Assert.AreEqual(node.EntryList.Count, deserialized.EntryList.Count);

            Assert.AreEqual(node.EntryList[0].Pointer, deserialized.EntryList[0].Pointer);
            Assert.AreEqual(node.EntryList[0].Key, deserialized.EntryList[0].Key);
            Assert.AreEqual(node.EntryList[1].Pointer, deserialized.EntryList[1].Pointer);
            Assert.AreEqual(node.EntryList[1].Key, deserialized.EntryList[1].Key);

        }

        [Test]
        public void Empty_And_Full_Nodes_Have_Same_Byte_Size()
        {
            var node = new Node<EmptyRecordIndexKey, Int64>(12);
            var serializer = new EmptyRecordIndexNodeSerializer();
            var emptyBytes = serializer.Serialize(node);
            for(int i = 1; i <= Node<int,int>.MaxEntriesForDegree(12); i++) //max nr of entries for degree 12
            {
                node.EntryList.Add(
                    new Entry<EmptyRecordIndexKey, long>{
                        Key = new EmptyRecordIndexKey{S = 1, A = 1},
                        Pointer = i
                    }
                );
            }
            for(int i = 1; i <= Node<int,int>.MaxChildrenForDegree(12); i++) //max nr of childres for degree 12
            {
                node.ChildrenAddresses.Add(i);
            }

            var fullBytes = serializer.Serialize(node);
            Assert.AreEqual(emptyBytes.Length, fullBytes.Length);
        }

    }
}

