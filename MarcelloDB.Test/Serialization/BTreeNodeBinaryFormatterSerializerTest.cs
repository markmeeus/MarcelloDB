using System;
using NUnit.Framework;
using MarcelloDB.Index;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using MarcelloDB.Records;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class BTreeNodeBinaryFormatterSerializerTest
    {
        class CustomData{}

        [Test]
        public void Cannot_Serialize_Custom_Value()
        {
            Assert.IsFalse(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(CustomData)));
        }

        [Test]
        public void Cannot_Serialize_NodeCustom_Value()
        {
            Assert.IsFalse(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<CustomData>)));
        }

        [Test]
        public void Cannot_Serialize_NodeValueWithAddress()
        {
            Assert.IsFalse(BTreeNodeBinaryFormatterSerializer
                .CanSerialize(typeof(Node<ValueWithAddressIndexKey<CustomData>>)));
        }

        [Test]
        public void Can_Serialize_Boolean()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Boolean>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Boolean>>)));
        }

        [Test]
        public void Serializes_Node_Bool()
        {
            SerializesNode<Boolean>();
        }

        [Test]
        public void Serializes_Boolean_Entries()
        {
            SerializesEntries<Boolean>(true, true, false);
        }

        [Test]
        public void Can_Serialize_NullableBoolean()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Boolean?>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Boolean?>>)));
        }

        [Test]
        public void Serializes_Node_NullableBoolean()
        {
            SerializesNode<Boolean?>();
        }

        [Test]
        public void Serializes_NullableBoolean_Entries()
        {
            SerializesEntries<Boolean?>(true, true, false);
        }

        [Test]
        public void Serializes_BoolWithAddress()
        {
            SerializesEntriesWithAddress<Boolean>(true, true, false);
        }

        [Test]
        public void Can_Serialize_Byte()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Byte>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Byte>>)));
        }

        [Test]
        public void Serializes_Node_Byte()
        {
            SerializesNode<Byte>();
        }

        [Test]
        public void Serializes_Byte_Entries()
        {
            SerializesEntries<Byte>(1, 2, 3);
        }

        [Test]
        public void Can_Serialize_NullableByte()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Byte?>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Byte?>>)));
        }

        [Test]
        public void Serializes_Node_NullableByte()
        {
            SerializesNode<Byte?>();
        }

        [Test]
        public void Serializes_NullableByte_Entries()
        {
            SerializesEntries<Byte?>(1, 2, 3);
        }

        [Test]
        public void Can_Serialize_Int16()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Int16>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Int16>>)));
        }

        [Test]
        public void Serializes_Int16()
        {
            SerializesEntries<Int16>(1, 2, 3);
        }

        [Test]
        public void Can_Serialize_NullableInt16()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Int16?>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Int16?>>)));
        }

        [Test]
        public void Serializes_NullableInt16()
        {
            SerializesEntries<Int16?>(1, 2, 3);
        }

        [Test]
        public void Can_Serialize_Int32()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Int32>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Int32>>)));
        }

        [Test]
        public void Serializes_Int32()
        {
            SerializesEntries<Int32>(1, 2, 3);
        }

        [Test]
        public void Can_Serialize_NullableInt32()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Int32?>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Int32?>>)));
        }

        [Test]
        public void Serializes_NullableInt32()
        {
            SerializesEntries<Int32?>(1, 2, 3);
        }

        [Test]
        public void Can_Serialize_Int64()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Int64>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Int64>>)));
        }

        [Test]
        public void Serializes_Int64()
        {
            SerializesEntries<Int64>(1, 2, 3);
        }

        [Test]
        public void Can_Serialize_NullableInt64()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Int64?>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Int64?>>)));
        }

        [Test]
        public void Serializes_NullableInt64()
        {
            SerializesEntries<Int64?>(1, 2, 3);
        }

        [Test]
        public void Can_Serialize_Decimal()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Decimal>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Decimal>>)));
        }

        [Test]
        public void Serializes_Decimal()
        {
            SerializesEntries<Decimal>(0.1m, 0.2m, 0.2m);
        }

        [Test]
        public void Can_Serialize_NullableDecimal()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Decimal?>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Decimal?>>)));
        }

        [Test]
        public void Serializes_NullableDecimal()
        {
            SerializesEntries<Decimal?>(0.1m, 0.2m, 0.2m);
        }

        [Test]
        public void Can_Serialize_Single()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Single>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Single>>)));
        }

        [Test]
        public void Serializes_Single()
        {
            SerializesEntries<Single>((Single)0.1, (Single)0.2, (Single)0.3);
        }

        [Test]
        public void Can_Serialize_NullableSingle()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Single?>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Single?>>)));
        }

        [Test]
        public void Serializes_NullableSingle()
        {
            SerializesEntries<Single?>((Single)0.1, (Single)0.2, (Single)0.3);
        }

        [Test]
        public void Can_Serialize_Double()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Double>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Double>>)));
        }

        [Test]
        public void Serializes_Double()
        {
            SerializesEntries<Double>(0.1, 0.2, 0.3);
        }

        [Test]
        public void Can_Serialize_NullableDouble()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<Double?>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<Double?>>)));
        }

        [Test]
        public void Serializes_NullableDouble()
        {
            SerializesEntries<Double?>(0.1, 0.2, 0.3);
        }

        [Test]
        public void Can_Serialize_DateTime()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<DateTime>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<DateTime>>)));
        }

        [Test]
        public void Serializes_DateTime()
        {
            SerializesEntries<DateTime>(
                new DateTime(2016, 5, 9, 8, 5, 10),
                new DateTime(2016, 5, 11, 8, 5, 10),
                new DateTime(2016, 5, 10, 8, 5, 10)
           );
        }

        [Test]
        public void Can_Serialize_NullableDateTime()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<DateTime?>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<DateTime>>)));
        }

        [Test]
        public void Serializes_NullableDateTime()
        {
            SerializesEntries<DateTime?>(
                new DateTime(2016, 5, 9, 8, 5, 10),
                new DateTime(2016, 5, 11, 8, 5, 10),
                new DateTime(2016, 5, 10, 8, 5, 10)
            );
        }

        [Test]
        public void Can_Serialize_String()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<String>)));
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(typeof(Node<ValueWithAddressIndexKey<String>>)));
        }

        [Test]
        public void Serializes_String()
        {
            SerializesEntries<string>(
                "String 1", " String 2", "String 3"
            );
        }

        [Test]
        public void Can_Serialize_CompoundValue_1()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(
                typeof(Node<ValueWithAddressIndexKey<CompoundValue<int>>>)));
        }

        [Test]
        public void Serializes_CompoundValue_1()
        {
            SerializesEntries<CompoundValue<Int32>>(
                CompoundValue.Build(1), CompoundValue.Build(2), CompoundValue.Build(3));
        }

        [Test]
        public void Can_Serialize_CompoundValue_2()
        {
            Assert.IsTrue(BTreeNodeBinaryFormatterSerializer.CanSerialize(
                typeof(Node<ValueWithAddressIndexKey<CompoundValue<int, string>>>)));
        }

        [Test]
        public void Serializes_CompoundValue_2()
        {
            SerializesEntries<CompoundValue<Int32, string>>(
                CompoundValue.Build(1, "a"), CompoundValue.Build(2, "b"), CompoundValue.Build(3, "c"));
        }
        [Test]
        public void Cannot_Serialize_CompoundValue_Custom()
        {
            Assert.IsFalse(BTreeNodeBinaryFormatterSerializer.CanSerialize(
                typeof(Node<ValueWithAddressIndexKey<CompoundValue<int, CustomData>>>)));
        }


        [Test]
        public void SerializesChildren()
        {
            var serializer = new BTreeNodeBinaryFormatterSerializer<Byte>();
            var node = new Node<Byte>(4);

            node.EntryList.Add(new Entry<byte>(){ Key = 1, Pointer = 2 });
            node.EntryList.Add(new Entry<byte>(){ Key = 3, Pointer = 4 });

            node.ChildrenAddresses.Add(123);
            node.ChildrenAddresses.Add(456);
            node.ChildrenAddresses.Add(789);

            node = Reserialize(node, serializer);

            Assert.AreEqual(123, node.ChildrenAddresses[0]);
            Assert.AreEqual(456, node.ChildrenAddresses[1]);
            Assert.AreEqual(789, node.ChildrenAddresses[2]);
        }

        public void SerializesNode<TKey>()
        {
            var node = new Node<TKey>(4);
            var serializer = new BTreeNodeBinaryFormatterSerializer<TKey>();
            Reserialize<TKey>(node, serializer);
        }

        public void SerializesEntries<TKey>(TKey entryKey1, TKey entryKey2, TKey entryKey3)
        {
            var node = new Node<TKey>(4);
            var serializer = new BTreeNodeBinaryFormatterSerializer<TKey>();

            node.EntryList.Add(new Entry<TKey>(){ Key = entryKey1, Pointer = 123 });
            node.EntryList.Add(new Entry<TKey>(){ Key = entryKey2, Pointer = 456 });
            node.EntryList.Add(new Entry<TKey>(){ Key = entryKey3, Pointer = 789 });

            node = Reserialize(node, serializer);

            Assert.AreEqual(3, node.EntryList.Count);
            Assert.AreEqual(entryKey1, node.EntryList[0].Key);
            Assert.AreEqual(123, node.EntryList[0].Pointer);
            Assert.AreEqual(entryKey2, node.EntryList[1].Key);
            Assert.AreEqual(456, node.EntryList[1].Pointer);
            Assert.AreEqual(entryKey3, node.EntryList[2].Key);
            Assert.AreEqual(789, node.EntryList[2].Pointer);
        }

        public void SerializesEntriesWithAddress<TKey>(TKey entryKey1, TKey entryKey2, TKey entryKey3)
        {
            var node = new Node<ValueWithAddressIndexKey<TKey>>(4);
            var serializer = new BTreeNodeBinaryFormatterSerializer<ValueWithAddressIndexKey<TKey>>();

            node.EntryList.Add(new Entry<ValueWithAddressIndexKey<TKey>>(){
                Key = new ValueWithAddressIndexKey<TKey>(){A = 111, V = entryKey1},
                Pointer = 123
            });
            node.EntryList.Add(new Entry<ValueWithAddressIndexKey<TKey>>(){
                Key = new ValueWithAddressIndexKey<TKey>(){A = 222, V = entryKey2},
                Pointer = 456
            });
            node.EntryList.Add(new Entry<ValueWithAddressIndexKey<TKey>>(){
                Key = new ValueWithAddressIndexKey<TKey>(){A = 333, V = entryKey3},
                Pointer = 789
            });

            node = Reserialize(node, serializer);

            Assert.AreEqual(3, node.EntryList.Count);
            Assert.AreEqual(111, node.EntryList[0].Key.A);
            Assert.AreEqual(entryKey1, node.EntryList[0].Key.V);
            Assert.AreEqual(123, node.EntryList[0].Pointer);
            Assert.AreEqual(222, node.EntryList[1].Key.A);
            Assert.AreEqual(entryKey2, node.EntryList[1].Key.V);
            Assert.AreEqual(456, node.EntryList[1].Pointer);
            Assert.AreEqual(333, node.EntryList[2].Key.A);
            Assert.AreEqual(entryKey3, node.EntryList[2].Key.V);
            Assert.AreEqual(789, node.EntryList[2].Pointer);
        }

        Node<TKey> Reserialize<TKey>(Node<TKey> node, BTreeNodeBinaryFormatterSerializer<TKey> serializer)
        {
            return serializer.Deserialize(serializer.Serialize(node));
        }

        Node<TKey> Reserialize<TKey>(Node<TKey> node, BTreeNodeBsonSerializer<TKey> serializer)
        {
            return serializer.Deserialize(serializer.Serialize(node));
        }
    }
}

