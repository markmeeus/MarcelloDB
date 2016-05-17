using System;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using System.Collections.Generic;
using MarcelloDB.Records;
using System.Reflection;
using System.Linq;

namespace MarcelloDB.Serialization
{
    abstract class ValueSerializer{}
    abstract class ValueSerializer<TValue> : ValueSerializer
    {
        internal abstract void WriteValue(BinaryFormatter formatter, TValue value);

        internal abstract TValue ReadValue(BinaryFormatter formatter);
    }

    class BooleanSerializer : ValueSerializer<Boolean>
    {
        internal override Boolean ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadBool();
        }
        internal override void WriteValue(BinaryFormatter formatter, Boolean value)
        {
            formatter.WriteBool(value);
        }
    }

    class NullableBooleanSerializer : ValueSerializer<Boolean?>
    {
        internal override Boolean? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableBool();
        }
        internal override void WriteValue(BinaryFormatter formatter, Boolean? value)
        {
            formatter.WriteNullableBool(value);
        }
    }

    class ByteSerializer : ValueSerializer<Byte>
    {
        internal override Byte ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadByte();
        }
        internal override void WriteValue(BinaryFormatter formatter, Byte value)
        {
            formatter.WriteByte(value);
        }
    }

    class NullableByteSerializer : ValueSerializer<Byte?>
    {
        internal override Byte? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableByte();
        }
        internal override void WriteValue(BinaryFormatter formatter, Byte? value)
        {
            formatter.WriteNullableByte(value);
        }
    }

    class Int16Serializer : ValueSerializer<Int16>
    {
        internal override Int16 ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadInt16();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int16 value)
        {
            formatter.WriteInt16(value);
        }
    }

    class NullableInt16Serializer : ValueSerializer<Int16?>
    {
        internal override Int16? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableInt16();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int16? value)
        {
            formatter.WriteNullableInt16(value);
        }
    }

    class Int32Serializer : ValueSerializer<Int32>
    {
        internal override Int32 ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadInt32();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int32 value)
        {
            formatter.WriteInt32(value);
        }
    }

    class NullableInt32Serializer : ValueSerializer<Int32?>
    {
        internal override Int32? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableInt32();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int32? value)
        {
            formatter.WriteNullableInt32(value);
        }
    }

    class Int64Serializer : ValueSerializer<Int64>
    {
        internal override Int64 ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadInt64();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int64 value)
        {
            formatter.WriteInt64(value);
        }
    }

    class NullableInt64Serializer : ValueSerializer<Int64?>
    {
        internal override Int64? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableInt64();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int64? value)
        {
            formatter.WriteNullableInt64(value);
        }
    }

    class DecimalSerializer : ValueSerializer<Decimal>
    {
        internal override Decimal ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadDecimal();
        }
        internal override void WriteValue(BinaryFormatter formatter, Decimal value)
        {
            formatter.WriteDecimal(value);
        }
    }

    class NullableDecimalSerializer : ValueSerializer<Decimal?>
    {
        internal override Decimal? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableDecimal();
        }
        internal override void WriteValue(BinaryFormatter formatter, Decimal? value)
        {
            formatter.WriteNullableDecimal(value);
        }
    }

    class SingleSerializer : ValueSerializer<Single>
    {
        internal override Single ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadSingle();
        }
        internal override void WriteValue(BinaryFormatter formatter, Single value)
        {
            formatter.WriteSingle(value);
        }
    }

    class NullableSingleSerializer : ValueSerializer<Single?>
    {
        internal override Single? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableSingle();
        }
        internal override void WriteValue(BinaryFormatter formatter, Single? value)
        {
            formatter.WriteNullableSingle(value);
        }
    }

    class DoubleSerializer : ValueSerializer<Double>
    {
        internal override Double ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadDouble();
        }
        internal override void WriteValue(BinaryFormatter formatter, Double value)
        {
            formatter.WriteDouble(value);
        }
    }

    class NullableDoubleSerializer : ValueSerializer<Double?>
    {
        internal override Double? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableDouble();
        }
        internal override void WriteValue(BinaryFormatter formatter, Double? value)
        {
            formatter.WriteNullableDouble(value);
        }
    }

    class DateTimeSerializer : ValueSerializer<DateTime>
    {
        internal override DateTime ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadDateTime();
        }
        internal override void WriteValue(BinaryFormatter formatter, DateTime value)
        {
            formatter.WriteDateTime(value);
        }
    }

    class NullableDateTimeSerializer : ValueSerializer<DateTime?>
    {
        internal override DateTime? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableDateTime();
        }
        internal override void WriteValue(BinaryFormatter formatter, DateTime? value)
        {
            formatter.WriteNullableDateTime(value);
        }
    }

    class StringSerializer : ValueSerializer<String>
    {
        internal override String ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadString();
        }
        internal override void WriteValue(BinaryFormatter formatter, String value)
        {
            formatter.WriteString(value);
        }
    }


    class ValueWithAddressSerializer<TValue> : ValueSerializer<ValueWithAddressIndexKey<TValue>>
    {
        ValueSerializer<TValue> ValueSerializer { get; set; }

        internal ValueWithAddressSerializer(ValueSerializer<TValue> valueSerializer){
            this.ValueSerializer = valueSerializer;
        }

        internal override void WriteValue(BinaryFormatter formatter, ValueWithAddressIndexKey<TValue> value)
        {
            formatter.WriteInt64(value.A);
            this.ValueSerializer.WriteValue(formatter, value.V);
        }

        internal override ValueWithAddressIndexKey<TValue> ReadValue(BinaryFormatter formatter)
        {
            var result = new ValueWithAddressIndexKey<TValue>();
            result.A = formatter.ReadInt64();
            result.V = this.ValueSerializer.ReadValue(formatter);
            return result;
        }
    }

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

            formatter.WriteInt32(node.Degree);

            WriteEntries(formatter, node.EntryList.Entries);
            WriteChildrenAddresses(formatter, node.ChildrenAddresses);

            return writer.GetTrimmedBuffer();
        }

        public Node<TKey> Deserialize(byte[] bytes)
        {
            var reader = new BufferReader(bytes);
            var formatter = new BinaryFormatter(reader);

            formatter.ReadByte(); //Read format version;

            var degree = formatter.ReadInt32();
            var node = new Node<TKey>(degree);

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
            if(!ValueSerializers.ContainsKey(typeof(TKey))){
                if (typeof(TKey).IsConstructedGenericType)
                {
                    var genericType = typeof(TKey).GetGenericTypeDefinition();
                    if (genericType == typeof(ValueWithAddressIndexKey<>))
                    {
                        ValueSerializers[typeof(TKey)] = BuildValueWithAddressSerializer(typeof(TKey));
                    }
                }
            }

            this.ValueSerializer = ValueSerializers[typeof(TKey)];
        }

        ValueSerializer BuildValueWithAddressSerializer(Type valueWithAddressType)
        {
            var typeOfValue = valueWithAddressType.GenericTypeArguments[0];
            var innerValueSerializer = ValueSerializers[typeOfValue];

            var valueType = typeof(TKey).GenericTypeArguments[0];

            var serializerConstructor = typeof(ValueWithAddressSerializer<>).GetGenericTypeDefinition()
                .GetTypeInfo()
                .MakeGenericType(new Type[]{ valueType })
                .GetTypeInfo()
                .DeclaredConstructors.First();

            var serializer = serializerConstructor.Invoke(new object[]{ innerValueSerializer });

            return (ValueSerializer)serializer;
        }

        static Dictionary<Type, ValueSerializer> _valueSerializers;
        static Dictionary<Type, ValueSerializer> ValueSerializers
        {
            get  {
                if (_valueSerializers == null)
                {
                    _valueSerializers = new Dictionary<Type, ValueSerializer>
                    {
                        {typeof(Boolean), new BooleanSerializer()},
                        {typeof(Boolean?), new NullableBooleanSerializer()},
                        {typeof(Byte), new ByteSerializer()},
                        {typeof(Byte?), new NullableByteSerializer()},
                        {typeof(Int16), new Int16Serializer()},
                        {typeof(Int16?), new NullableInt16Serializer()},
                        {typeof(Int32), new Int32Serializer()},
                        {typeof(Int32?), new NullableInt32Serializer()},
                        {typeof(Int64), new Int64Serializer()},
                        {typeof(Int64?), new NullableInt64Serializer()},
                        {typeof(Decimal), new DecimalSerializer()},
                        {typeof(Decimal?), new NullableDecimalSerializer()},
                        {typeof(Single), new SingleSerializer()},
                        {typeof(Single?), new NullableSingleSerializer()},
                        {typeof(Double), new DoubleSerializer()},
                        {typeof(Double?), new NullableDoubleSerializer()},
                        {typeof(DateTime), new DateTimeSerializer()},
                        {typeof(DateTime?), new NullableDateTimeSerializer()},
                        {typeof(String), new StringSerializer()}
                    };
                }
                return _valueSerializers;
            }
        }
    }
}

