using System;
using System.Text;
using System.Collections.Generic;

namespace MarcelloDB.Serialization
{
    internal class BinaryFormatter
    {
        internal enum LengthSize
        {
            SingleByte  = 1,
            TwoBytes    = 2,
            FourBytes   = 3
        }
        internal enum TypeID
        {
            Bool                = 1,
            NullableBool        = 2,
            Byte                = 3,
            NullableByte        = 4,
            Int16               = 5,
            NullableInt16       = 6,
            Int32               = 7,
            NullableInt32       = 8,
            Int64               = 9,
            NullableInt64       = 10,
            Decimal             = 11,
            NullableDecimal     = 12,
            Single              = 13,
            NullableSingle      = 14,
            Double              = 15,
            NullableDouble      = 16,
            DateTime            = 17,
            NullableDateTime    = 18,
            String              = 19
        }

        internal const byte FORMATTER_VERSION = 1;

        BufferWriter Writer { get; set; }

        BufferReader Reader { get; set; }

        internal BinaryFormatter(BufferWriter writer)
        {
            this.Writer = writer;
            this.Writer.WriteByte(FORMATTER_VERSION);
        }

        internal BinaryFormatter(BufferReader reader)
        {
            this.Reader = reader;
            this.Reader.ReadByte(); //VERSION
        }

        internal BinaryFormatter WriteBool(bool value)
        {
            WriteTypeID(TypeID.Bool);
            this.Writer.WriteByte(BoolToByte(value));
            return this;
        }

        internal bool ReadBool()
        {
            ReadValidTypeID(TypeID.Bool);
            var boolAsByte = this.Reader.ReadByte();
            return boolAsByte != 0x00;
        }

        public BinaryFormatter WriteNullableBool(bool? value)
        {
            WriteTypeID(TypeID.NullableBool);
            WriteNullableValue(value.HasValue, () => this.Writer.WriteByte(BoolToByte(value.Value)));
            return this;
        }

        public bool? ReadNullableBool()
        {
            ReadValidTypeID(TypeID.NullableBool);
            return ReadNullable<bool?>(()=>this.Reader.ReadByte() != 0x00);
        }

        internal BinaryFormatter WriteByte(byte value)
        {
            WriteTypeID(TypeID.Byte);
            this.Writer.WriteByte(value);
            return this;
        }

        internal byte ReadByte()
        {
            ReadValidTypeID(TypeID.Byte);
            return this.Reader.ReadByte();
        }

        internal BinaryFormatter WriteNullableByte(byte? value)
        {
            WriteTypeID(TypeID.NullableByte);
            WriteNullableValue(value.HasValue, () => this.Writer.WriteByte(value.Value));
            return this;
        }

        internal byte? ReadNullableByte()
        {
            ReadValidTypeID(TypeID.NullableByte);
            return ReadNullable<byte?>(()=>this.Reader.ReadByte());
        }

        internal BinaryFormatter WriteInt16(Int16 value)
        {
            WriteTypeID(TypeID.Int16);
            this.Writer.WriteInt16(value);
            return this;
        }

        internal Int16 ReadInt16()
        {
            ReadValidTypeID(TypeID.Int16);
            return this.Reader.ReadInt16();
        }

        internal BinaryFormatter WriteNullableInt16(Int16? value)
        {
            WriteTypeID(TypeID.NullableInt16);
            WriteNullableValue(value.HasValue, () => this.Writer.WriteInt16(value.Value));
            return this;
        }

        internal Int16? ReadNullableInt16()
        {
            ReadValidTypeID(TypeID.NullableInt16);
            return ReadNullable<Int16?>(() => this.Reader.ReadInt16());
        }

        internal BinaryFormatter WriteInt32(Int32 value)
        {
            WriteTypeID(TypeID.Int32);
            this.Writer.WriteInt32(value);
            return this;
        }

        internal Int32 ReadInt32()
        {
            ReadValidTypeID(TypeID.Int32);
            return this.Reader.ReadInt32();
        }

        internal BinaryFormatter WriteNullableInt32(Int32? value)
        {
            WriteTypeID(TypeID.NullableInt32);
            WriteNullableValue(value.HasValue, () => this.Writer.WriteInt32(value.Value));
            return this;
        }

        internal Int32? ReadNullableInt32()
        {
            ReadValidTypeID(TypeID.NullableInt32);
            return ReadNullable<Int32?>(()=>this.Reader.ReadInt32());
        }

        internal BinaryFormatter WriteInt64(Int64 value)
        {
            this.WriteTypeID(TypeID.Int64);
            this.Writer.WriteInt64(value);
            return this;
        }

        internal Int64 ReadInt64()
        {
            ReadValidTypeID(TypeID.Int64);
            return this.Reader.ReadInt64();
        }

        internal BinaryFormatter WriteNullableInt64(Int64? value)
        {
            WriteTypeID(TypeID.NullableInt64);
            WriteNullableValue(value.HasValue, () => this.Writer.WriteInt64(value.Value));
            return this;
        }

        internal Int64? ReadNullableInt64()
        {
            ReadValidTypeID(TypeID.NullableInt64);
            return ReadNullable<Int64?>(()=>this.Reader.ReadInt64());
        }

        internal BinaryFormatter WriteDecimal(Decimal value)
        {
            WriteTypeID(TypeID.Decimal);
            this.Writer.WriteBytes(DecimalToBytes(value));
            return this;
        }

        internal Decimal ReadDecimal()
        {
            ReadValidTypeID(TypeID.Decimal);
            return BytesToDecimal(this.Reader.ReadBytes(4 * 4));
        }

        internal BinaryFormatter WriteNullableDecimal(Decimal? value)
        {
            WriteTypeID(TypeID.NullableDecimal);
            WriteNullableValue(value.HasValue,
                () => this.Writer.WriteBytes(DecimalToBytes(value.Value)));
            return this;
        }

        internal Decimal? ReadNullableDecimal()
        {
            ReadValidTypeID(TypeID.NullableDecimal);
            return ReadNullable<Decimal?>(() => BytesToDecimal(this.Reader.ReadBytes(4 * 4)));
        }

        internal BinaryFormatter WriteSingle(Single value)
        {
            WriteTypeID(TypeID.Single);
            this.Writer.WriteBytes(BitConverter.GetBytes(value));
            return this;
        }

        internal Single ReadSingle()
        {
            ReadValidTypeID(TypeID.Single);
            return BitConverter.ToSingle(this.Reader.ReadBytes(4), 0);
        }

        internal BinaryFormatter WriteNullableSingle(Single? value)
        {
            WriteTypeID(TypeID.NullableSingle);
            WriteNullableValue(value.HasValue,
                () => this.Writer.WriteBytes(BitConverter.GetBytes(value.Value)));
            return this;
        }

        internal Single? ReadNullableSingle()
        {
            ReadValidTypeID(TypeID.NullableSingle);
            return ReadNullable<Single?>(() => BitConverter.ToSingle(this.Reader.ReadBytes(4), 0));
        }

        internal BinaryFormatter WriteDouble(Double value)
        {
            WriteTypeID(TypeID.Double);
            this.Writer.WriteBytes(BitConverter.GetBytes(value));
            return this;
        }

        internal Double ReadDouble()
        {
            ReadValidTypeID(TypeID.Double);
            return BitConverter.ToDouble(this.Reader.ReadBytes(8), 0);
        }

        internal BinaryFormatter WriteNullableDouble(Double? value)
        {
            WriteTypeID(TypeID.NullableDouble);
            WriteNullableValue(value.HasValue,
                () => this.Writer.WriteBytes(BitConverter.GetBytes(value.Value)));
            return this;
        }

        internal Double? ReadNullableDouble()
        {
            ReadValidTypeID(TypeID.NullableDouble);
            return ReadNullable<Double?>(() => BitConverter.ToDouble(this.Reader.ReadBytes(8), 0));
        }

        internal BinaryFormatter WriteDateTime(DateTime dateTime)
        {
            this.WriteTypeID(TypeID.DateTime);
            this.Writer.WriteInt64(dateTime.ToBinary());
            return this;
        }

        internal DateTime ReadDateTime()
        {
            ReadValidTypeID(TypeID.DateTime);
            return DateTime.FromBinary(this.Reader.ReadInt64());
        }

        internal BinaryFormatter WriteNullableDateTime(DateTime? value)
        {
            WriteTypeID(TypeID.NullableDateTime);
            WriteNullableValue(value.HasValue, () => this.Writer.WriteInt64(value.Value.ToBinary()));
            return this;
        }

        internal DateTime? ReadNullableDateTime()
        {
            ReadValidTypeID(TypeID.NullableDateTime);
            return ReadNullable<DateTime?>(()=>DateTime.FromBinary(this.Reader.ReadInt64()));
        }

        internal void WriteString(string value)
        {
            this.WriteTypeID(TypeID.String);

            if(value != null)
            {
                WriteStringValue(value);
            }
            else
            {
                WriteStringNull();
            }
        }

        internal string ReadString()
        {
            this.Reader.ReadByte(); //Read typeID
            var lengthSize = (LengthSize)Reader.ReadByte();
            Int32 length;
            switch (lengthSize)
            {
                case LengthSize.SingleByte:
                    length = Reader.ReadByte();
                    break;
                case LengthSize.TwoBytes:
                    length = Reader.ReadInt16();
                    break;
                case LengthSize.FourBytes:
                    length = Reader.ReadInt32();
                    break;

                default:
                    throw new InvalidOperationException("Unknown LengthSize: " + lengthSize.ToString());
            }
            if (length == -1)
            {
                return null;
            }
            else
            {
                var bytes = this.Reader.ReadBytes(length);
                return Encoding.UTF8.GetString(bytes, 0, length);
            }
        }

        void WriteStringValue(string value)
        {
            byte[] bytes;
            bytes = Encoding.UTF8.GetBytes(value);
            var length = bytes.Length;
            if (length <= byte.MaxValue)
            {
                this.Writer.WriteByte((byte)LengthSize.SingleByte);
                this.Writer.WriteByte((byte)length);
            }
            else if( length <= Int16.MaxValue)
            {
                this.Writer.WriteByte((byte)LengthSize.TwoBytes);
                this.Writer.WriteInt16((Int16)length);
            }
            else if(length <= Int32.MaxValue)
            {
                this.Writer.WriteByte((byte)LengthSize.FourBytes);
                this.Writer.WriteInt32((Int32)length);
            }
            else
            {
                throw new InvalidOperationException("String too long: " +  length.ToString());
            }
            this.Writer.WriteBytes(bytes);
        }

        void WriteStringNull()
        {
            this.Writer.WriteByte((byte)LengthSize.TwoBytes);
            this.Writer.WriteInt16(-1);
        }

        void WriteTypeID(TypeID typeID)
        {
            this.Writer.WriteByte((byte)typeID);
        }

        TypeID ReadTypeID()
        {
            return (TypeID)this.Reader.ReadByte();
        }

        byte BoolToByte(bool value){
            return value ? (byte)0x01 : (byte)0x00;
        }

        internal Decimal BytesToDecimal(byte[] bytes)
        {
            var byteGroups = new Int32[4];

            byteGroups[0] = BitConverter.ToInt32(bytes, 0);
            byteGroups[1] = BitConverter.ToInt32(bytes, 4);
            byteGroups[2] = BitConverter.ToInt32(bytes, 8);
            byteGroups[3] = BitConverter.ToInt32(bytes, 12);

            return new Decimal(byteGroups);
        }

        byte[] DecimalToBytes(Decimal value)
        {
            Int32[] byteGroups = Decimal.GetBits(value);
            var bytes = new List<byte>();

            bytes.AddRange(BitConverter.GetBytes(byteGroups[0]));
            bytes.AddRange(BitConverter.GetBytes(byteGroups[1]));
            bytes.AddRange(BitConverter.GetBytes(byteGroups[2]));
            bytes.AddRange(BitConverter.GetBytes(byteGroups[3]));

            return bytes.ToArray();
        }

        void WriteNullableValue(bool hasValue, Action writeValue)
        {
            this.Writer.WriteByte(BoolToByte(hasValue));
            if (hasValue)
            {
                writeValue();
            }
        }

        T ReadNullable<T>(Func<T> readValue)
        {
            var hasValueAsByte = this.Reader.ReadByte();
            if (hasValueAsByte != 0x00)
            {
                return readValue();
            }
            else
            {
                return default(T);
            }
        }

        void ReadValidTypeID(TypeID wantedTypeID)
        {
            var typeID = (TypeID)this.Reader.ReadByte();
            if (typeID != wantedTypeID)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot read {0} as {1}", typeID, wantedTypeID)
                );
            }
        }
    }
}

