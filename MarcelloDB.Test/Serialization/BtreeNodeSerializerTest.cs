using System;
using NUnit.Framework;
using MarcelloDB.Index;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test
{
    public class CustomEntryKey
    {
        public string SomeValue { get; set; }
    }

    [TestFixture]
    public class BtreeNodeSerializerTest
    {
        [Test]
        public void Serializes_A_Node()
        {
            var node = new Node<string, Int64>(2);

            node.EntryList.Add(new Entry<string, Int64>(){ Key = "Item 1", Pointer = 1 });
            node.EntryList.Add(new Entry<string, Int64>(){ Key = "Item 2", Pointer = 2 });
            node.ChildrenAddresses.Add(1);
            node.ChildrenAddresses.Add(2);

            var serializer = new BTreeNodeSerializer<string, Int64>();

            var bytes = serializer.Serialize(node);
            var deserialized = serializer.Deserialize(bytes);

            Assert.AreEqual(node.EntryList[0].Key, deserialized.EntryList[0].Key);
            Assert.AreEqual(node.EntryList[0].Pointer, deserialized.EntryList[0].Pointer);
            Assert.AreEqual(node.EntryList[1].Key, deserialized.EntryList[1].Key);
            Assert.AreEqual(node.EntryList[1].Pointer, deserialized.EntryList[1].Pointer);
            Assert.AreEqual(node.ChildrenAddresses[0], deserialized.ChildrenAddresses[0]);
            Assert.AreEqual(node.ChildrenAddresses[1], deserialized.ChildrenAddresses[1]);
        }

        [Test]
        public void Serializes_With_Custom_Entry_Keys()
        {
            var node = new Node<CustomEntryKey, Int64>(2);
            node.EntryList.Add(
                new Entry<CustomEntryKey, long>(){
                    Key = new CustomEntryKey(){SomeValue = "Some Test Value"},
                    Pointer = 1
                });

                node.EntryList.Add(
                    new Entry<CustomEntryKey, long>(){
                        Key = new CustomEntryKey(){SomeValue = "Another Test Value"},
                        Pointer = 1
                });

            var serializer = new BTreeNodeSerializer<CustomEntryKey, Int64>();

            var bytes = serializer.Serialize(node);
            var deserialized = serializer.Deserialize(bytes);

            Assert.AreEqual(node.EntryList[0].Key.SomeValue, deserialized.EntryList[0].Key.SomeValue);
            Assert.AreEqual(node.EntryList[1].Key.SomeValue, deserialized.EntryList[1].Key.SomeValue);
        }

        [Test]
        public void Deserialized_Should_Not_Be_Dirty()
        {
            var node = new Node<string, Int64>(2);

            node.EntryList.Add(new Entry<string, Int64>(){ Key = "Item 1", Pointer = 1 });
            node.EntryList.Add(new Entry<string, Int64>(){ Key = "Item 2", Pointer = 2 });
            node.ChildrenAddresses.Add(1);
            node.ChildrenAddresses.Add(2);

            var serializer = new BTreeNodeSerializer<string, Int64>();

            var bytes = serializer.Serialize(node);
            var deserialized = serializer.Deserialize(bytes);

            Assert.IsFalse(deserialized.Dirty);
        }
    }
}

