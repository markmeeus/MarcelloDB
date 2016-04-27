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
            var node = new Node<string>(2);

            node.EntryList.Add(new Entry<string>(){ Key = "Item 1", Pointer = 1 });
            node.EntryList.Add(new Entry<string>(){ Key = "Item 2", Pointer = 2 });
            node.ChildrenAddresses.Add(1);
            node.ChildrenAddresses.Add(2);

            var serializer = new BTreeNodeSerializer<string>();

            var bytes = serializer.Serialize(node);
            var deserialized = serializer.Deserialize(bytes);

            Assert.AreEqual(node.EntryList.Entries[0].Key, deserialized.EntryList.Entries[0].Key);
            Assert.AreEqual(node.EntryList.Entries[0].Pointer, deserialized.EntryList.Entries[0].Pointer);
            Assert.AreEqual(node.EntryList.Entries[1].Key, deserialized.EntryList.Entries[1].Key);
            Assert.AreEqual(node.EntryList.Entries[1].Pointer, deserialized.EntryList.Entries[1].Pointer);
            Assert.AreEqual(node.ChildrenAddresses.Addresses[0], deserialized.ChildrenAddresses.Addresses[0]);
            Assert.AreEqual(node.ChildrenAddresses.Addresses[1], deserialized.ChildrenAddresses.Addresses[1]);
        }

        [Test]
        public void Serializes_With_Custom_Entry_Keys()
        {
            var node = new Node<CustomEntryKey>(2);
            node.EntryList.Add(
                new Entry<CustomEntryKey>(){
                    Key = new CustomEntryKey(){SomeValue = "Some Test Value"},
                    Pointer = 1
                });

                node.EntryList.Add(
                    new Entry<CustomEntryKey>(){
                        Key = new CustomEntryKey(){SomeValue = "Another Test Value"},
                        Pointer = 1
                });

            var serializer = new BTreeNodeSerializer<CustomEntryKey>();

            var bytes = serializer.Serialize(node);
            var deserialized = serializer.Deserialize(bytes);

            Assert.AreEqual(node.EntryList.Entries[0].Key.SomeValue, deserialized.EntryList.Entries[0].Key.SomeValue);
            Assert.AreEqual(node.EntryList.Entries[1].Key.SomeValue, deserialized.EntryList.Entries[1].Key.SomeValue);
        }

        [Test]
        public void Deserialized_Should_Not_Be_Dirty()
        {
            var node = new Node<string>(2);

            node.EntryList.Add(new Entry<string>(){ Key = "Item 1", Pointer = 1 });
            node.EntryList.Add(new Entry<string>(){ Key = "Item 2", Pointer = 2 });
            node.ChildrenAddresses.Add(1);
            node.ChildrenAddresses.Add(2);

            var serializer = new BTreeNodeSerializer<string>();

            var bytes = serializer.Serialize(node);
            var deserialized = serializer.Deserialize(bytes);

            Assert.IsFalse(deserialized.Dirty);
        }
    }
}

