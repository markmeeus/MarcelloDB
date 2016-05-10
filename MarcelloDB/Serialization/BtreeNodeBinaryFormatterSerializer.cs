using System;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using System.Collections.Generic;
using MarcelloDB.Records;

namespace MarcelloDB
{
    internal class BTreeNodeBinaryFormatterSerializer<TKey> : IObjectSerializer<Node<TKey>>
    {        
        class ReadWriteEntryFunctions
        {
            internal Action<BinaryFormatter, object> WriteEntryKey { get; set; }
            internal Func<BinaryFormatter, object> ReadEntryKey { get; set; }
        }


        const byte BTREE_NODE_FORMAT_VERSION = 1;

        ReadWriteEntryFunctions ReadWriteEntryFuncs { get; set; }

        public BTreeNodeBinaryFormatterSerializer()
        {
            SetReadWriteKeyFunctions();
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
            entry.Key = (TKey) this.ReadWriteEntryFuncs.ReadEntryKey(formatter);
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
            this.ReadWriteEntryFuncs.WriteEntryKey(formatter, entry.Key);
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

        void SetReadWriteKeyFunctions()
        {         
            this.ReadWriteEntryFuncs = ReadWriteEntryFunctionsDict[typeof(TKey)];
        }

        static void WriteValueWithAddress<TVal>(BinaryFormatter formatter, ValueWithAddressIndexKey<TVal> key)
        {
            formatter.WriteInt64(key.A);

            var writeFunc = ReadWriteEntryFunctionsDict[typeof(TVal)].WriteEntryKey;
            writeFunc(formatter, key.V);                
        }

        static ValueWithAddressIndexKey<TVal> ReadValueWithAddress<TVal>(BinaryFormatter formatter)
        {
            var result = new ValueWithAddressIndexKey<TVal>();
            result.A = formatter.ReadInt64();
            var readFunc = ReadWriteEntryFunctionsDict[typeof(TVal)].ReadEntryKey;
            result.V = (TVal)readFunc(formatter);
            return result;
        }

        static Dictionary<Type, ReadWriteEntryFunctions> _readWriteEntryFunctionsDict;
        static Dictionary<Type, ReadWriteEntryFunctions> ReadWriteEntryFunctionsDict
        {
            get {
                if (_readWriteEntryFunctionsDict == null)
                {
                    _readWriteEntryFunctionsDict = new Dictionary<Type, ReadWriteEntryFunctions>()
                    {
                        {typeof(Boolean), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteBool((Boolean)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadBool()
                            }},
                        {typeof(Boolean?), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteNullableBool((Boolean?)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadNullableBool()
                            }},
                        {typeof(Byte), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteByte((Byte)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadByte()
                            }},
                        {typeof(Byte?), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteNullableByte((Byte?)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadNullableByte()
                            }},
                        {typeof(Int16), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteInt16((Int16)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadInt16()
                            }},
                        {typeof(Int16?), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteNullableInt16((Int16?)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadNullableInt16()
                            }},
                        {typeof(Int32), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteInt32((Int32)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadInt32()
                            }},
                        {typeof(Int32?), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteNullableInt32((Int32?)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadNullableInt32()
                            }},
                        {typeof(Int64), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteInt64((Int64)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadInt64()
                            }},
                        {typeof(Int64?), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteNullableInt64((Int64?)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadNullableInt64()
                            }},
                        {typeof(Decimal), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteDecimal((Decimal)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadDecimal()
                            }},
                        {typeof(Decimal?), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) =>
                                    formatter.WriteNullableDecimal((Decimal?)(object)key),
                                ReadEntryKey = (formatter) =>
                                    formatter.ReadNullableDecimal()
                            }},
                        {typeof(Single), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteSingle((Single)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadSingle()
                            }},
                        {typeof(Single?), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteNullableSingle((Single?)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadNullableSingle()
                            }},
                        {typeof(Double), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteDouble((Double)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadDouble()
                            }},
                        {typeof(Double?), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteNullableDouble((Double?)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadNullableDouble()
                            }},
                        {typeof(DateTime), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteDateTime((DateTime)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadDateTime()
                            }},
                        {typeof(DateTime?), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteNullableDateTime((DateTime?)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadNullableDateTime()
                            }},
                        {typeof(String), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => formatter.WriteString((String)(object)key),
                                ReadEntryKey = (formatter) => formatter.ReadString()
                            }},
                        {typeof(ValueWithAddressIndexKey<Boolean>), new ReadWriteEntryFunctions
                            {
                                WriteEntryKey = (formatter, key) => {
                                    WriteValueWithAddress<Boolean>(formatter, (ValueWithAddressIndexKey<Boolean>)(object)key);
                                },
                                ReadEntryKey = (formatter) => ReadValueWithAddress<Boolean>(formatter)                                                                        
                            }},
                     };
                }
                return _readWriteEntryFunctionsDict;
            }
        }            
    }
}

