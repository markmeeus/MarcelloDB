using System;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using System.Collections.Generic;
using MarcelloDB.Records;
using System.Reflection;
using System.Linq;
using MarcelloDB.Serialization.ValueSerializers;

namespace MarcelloDB.Serialization
{
    internal class BTreeNodeBinaryFormatterSerializer
    {
        internal static bool CanSerialize(Type nodeType){
            if (nodeType.IsConstructedGenericType)
            {
                var genericType = nodeType.GetGenericTypeDefinition();
                if (genericType == typeof(Node<>))
                {
                    var valueType = nodeType.GenericTypeArguments[0];
                    if (valueType.IsConstructedGenericType)
                    {
                        if (valueType.GetGenericTypeDefinition() == typeof(ValueWithAddressIndexKey<>))
                        {
                            return CanSerializeValueOfType(valueType.GenericTypeArguments[0]);
                        }
                    }
                    return CanSerializeValueOfType(valueType);
                }
            }
            return false;
        }

        static bool CanSerializeValueOfType(Type valueType)
        {
            return
                valueType == typeof(Boolean) || valueType == typeof(Boolean?)
                || valueType == typeof(Byte) || valueType == typeof(Byte?)
                || valueType == typeof(Int16) || valueType == typeof(Int16?)
                || valueType == typeof(Int32) || valueType == typeof(Int32?)
                || valueType == typeof(Int64) || valueType == typeof(Int64?)
                || valueType == typeof(Decimal) || valueType == typeof(Decimal?)
                || valueType == typeof(Single) || valueType == typeof(Single?)
                || valueType == typeof(Double) || valueType == typeof(Double?)
                || valueType == typeof(DateTime) || valueType == typeof(DateTime?)
                || valueType == typeof(String);
        }
    }

    internal class BTreeNodeBinaryFormatterSerializer<TKey> : BTreeNodeBinaryFormatterSerializer, IObjectSerializer<Node<TKey>>
    {
        const byte BTREE_NODE_FORMAT_VERSION = 1;

        ValueSerializer ValueSerializer { get; set; }

        public BTreeNodeBinaryFormatterSerializer()
        {
            SetValueSerializer();
        }

        #region IObjectSerializer implementation

        public byte[] Serialize(Node<TKey> node)
        {
            var writer = new BufferWriter(new byte[0]);
            var formatter = new BinaryFormatter(writer);

            //avoiding incompatibility issues if formatting needs to change
            formatter.WriteByte(BTREE_NODE_FORMAT_VERSION);

            WriteEntries(formatter, node.EntryList.Entries);
            WriteChildrenAddresses(formatter, node.ChildrenAddresses);

            return writer.GetTrimmedBuffer();
        }

        public Node<TKey> Deserialize(byte[] bytes)
        {
            var reader = new BufferReader(bytes);
            var formatter = new BinaryFormatter(reader);

            formatter.ReadByte(); //Read format version;

            var node = new Node<TKey>(RecordIndex.BTREE_DEGREE);

            node.EntryList.SetEntries(ReadEntries(formatter));
            node.ChildrenAddresses.SetAddresses(ReadChildrenAddresses(formatter));
            return node;
        }

        #endregion

        List<Entry<TKey>> ReadEntries(BinaryFormatter formatter)
        {
            var result = new List<Entry<TKey>>();
            var count = (int)formatter.ReadByte();
            for(int i = 0; i < count; i++)
            {
                result.Add(ReadEntry(formatter));
            }
            return result;
        }

        Entry<TKey> ReadEntry(BinaryFormatter formatter)
        {
            var entry = new Entry<TKey>();
            entry.Pointer = formatter.ReadInt64();

            entry.Key = (TKey) ((ValueSerializer<TKey>)this.ValueSerializer).ReadValue(formatter);

            return entry;
        }

        void WriteEntries(BinaryFormatter formatter, List<Entry<TKey>> entries)
        {
            formatter.WriteByte((byte)entries.Count);
            foreach (var entry in entries)
            {
                WriteEntry(formatter, entry);
            }
        }

        void WriteEntry(BinaryFormatter formatter, Entry<TKey> entry)
        {
            formatter.WriteInt64(entry.Pointer);

            ((ValueSerializer<TKey>)this.ValueSerializer).WriteValue(formatter, entry.Key);

        }

        void WriteConcreteEntryKey(BinaryFormatter formatter, Entry<TKey> entry)
        {
            formatter.WriteByte((Byte)(object)entry.Key);
        }

        TKey ReadConcreteEntryKey(BinaryFormatter formatter)
        {
            return (TKey)(object)formatter.ReadByte();
        }

        List<Int64> ReadChildrenAddresses(BinaryFormatter formatter)
        {
            var count = formatter.ReadByte();
            var result = new List<Int64>();
            for (int i = 0; i < count; i++)
            {
                result.Add(formatter.ReadInt64());
            }
            return result;
        }

        void WriteChildrenAddresses(BinaryFormatter formatter, AddressList childrenAddresses)
        {
            formatter.WriteByte((byte)childrenAddresses.Count);
            foreach (var childAddress in childrenAddresses.Addresses)
            {
                formatter.WriteInt64(childAddress);
            }
        }

        void SetValueSerializer()
        {
            this.ValueSerializer = ValueSerializer.Instance<TKey>();
        }
    }
}

