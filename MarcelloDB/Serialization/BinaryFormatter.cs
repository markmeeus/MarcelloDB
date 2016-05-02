using System;
using System.Text;

namespace MarcelloDB.Serialization
{        
    internal class BinaryFormatter
    {
        internal enum LengthSize
        {
            SingleByte = 1,
            TwoBytes = 2,
            FourBytes = 3
        }
        internal enum TypeID
        {            
            Bool,
            NullableBool,
            Byte,
            NullableByte,
            Int16,
            NullableInt16,
            Int32,
            NullableInt32,
            Int64,
            NullableInt64,
            DateTime,
            NullableDateTime,
            String
        }
        internal class ValueContainer{}

        internal class ValueContainer<T> : ValueContainer
        {   
            internal T Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        internal class IntValue : ValueContainer<Int64>{}

        internal class StringValue : ValueContainer<string>{}

        internal const byte VERSION = 1;

        BufferWriter Writer { get; set; }

        BufferReader Reader { get; set; }

        internal BinaryFormatter(BufferWriter writer)
        {
            this.Writer = writer;
        }

        internal BinaryFormatter(BufferReader reader)
        {
            this.Reader = reader;
            this.Reader.ReadByte(); //read version
        }

        internal BinaryFormatter Start()
        {
            this.Writer.WriteByte(VERSION);
            return this;
        }

        internal BinaryFormatter WriteBool(bool value)
        {           
            this.WriteTypeID(TypeID.Bool);
            this.Writer.WriteByte(BoolToByte(value));
            return this;
        }

        internal bool ReadBool()
        {           
            this.Reader.ReadByte(); //Read TypeID
            var boolAsByte = this.Reader.ReadByte();
            return boolAsByte != 0x00;
        }

        public BinaryFormatter WriteNullableBool(bool? value)
        {               
            this.WriteTypeID(TypeID.NullableBool);
            this.Writer.WriteByte(BoolToByte(value.HasValue));
            if (value.HasValue)
            {
                this.Writer.WriteByte(BoolToByte(value.Value));
            }
            return this;
        }

        public bool? ReadNullableBool()
        {
            this.Reader.ReadByte(); //Read TypeID
            var hasValueAsByte = this.Reader.ReadByte();
            if (hasValueAsByte != 0x00)
            {
                return this.Reader.ReadByte() != 0x00;
            }
            else
            {
                return null;
            }
        }

        internal BinaryFormatter WriteByte(byte value)
        {
            this.WriteTypeID(TypeID.Byte);
            this.Writer.WriteByte(value);
            return this;
        }

        internal byte ReadByte()
        {
            return (byte)((IntValue)ReadValue()).Value;
        }

        public BinaryFormatter WriteNullableByte(byte? value)
        {
            this.WriteTypeID(TypeID.NullableByte);
            this.Writer.WriteByte(BoolToByte(value.HasValue));
            if (value.HasValue)
            {
                this.Writer.WriteByte(value.Value);
            }
            return this;       
        }

        public object ReadNullableByte()
        {
            this.Reader.ReadByte(); //Read TypeID
            var hasValueAsByte = this.Reader.ReadByte();
            if (hasValueAsByte != 0x00)
            {
                return this.Reader.ReadByte();
            }
            else
            {
                return null;
            }
        }
            
        internal BinaryFormatter WriteInt16(Int16 value)
        {
            this.WriteTypeID(TypeID.Int16);
            this.Writer.WriteInt16(value);
            return this;
        }

        internal Int16 ReadInt16()
        {
            return (Int16)((IntValue)ReadValue()).Value;
        }

        internal BinaryFormatter WriteInt32(Int32 value)
        {
            this.WriteTypeID(TypeID.Int32);
            this.Writer.WriteInt32(value);
            return this;
        }

        internal Int32 ReadInt32()
        {            
            return (Int32)((IntValue)ReadValue()).Value;
        }

        internal BinaryFormatter WriteInt64(Int64 value)
        {
            this.WriteTypeID(TypeID.Int64);
            this.Writer.WriteInt64(value);
            return this;
        }

        internal Int64 ReadInt64()
        {
            return ((IntValue)ReadValue()).Value;
        }

        internal BinaryFormatter WriteDateTime(DateTime dateTime)
        {
            this.WriteTypeID(TypeID.DateTime);
            this.Writer.WriteInt64(dateTime.ToBinary());
            return this;
        }

        internal DateTime ReadDateTime()
        {
            this.Reader.ReadByte(); //Read Type ID
            return new DateTime(this.Reader.ReadInt64());
        }

        internal void WriteString(string value)
        {
            this.WriteTypeID(TypeID.String);
            var bytes = Encoding.UTF8.GetBytes(value);
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

        internal string ReadString()
        {
            return ReadValue().ToString();
        }


        void WriteTypeID(TypeID typeID)
        {
            this.Writer.WriteByte((byte)typeID);
        }

        TypeID ReadTypeID()
        {
            return (TypeID)this.Reader.ReadByte();
        }

        ValueContainer ReadValue()
        {
            var typeID = (TypeID) this.Reader.ReadByte(); //Read TypeID
            if(IsIntType(typeID))
            {
                return new IntValue{Value = ReadIntValue(typeID)};
            }
            if (typeID == TypeID.String)
            {
                return new StringValue{ Value = ReadStringValue() };
            }
            throw new NotImplementedException();
        }
         
        bool IsIntType(TypeID typeID){        
            return                 
                typeID == TypeID.Byte || 
                typeID == TypeID.Int16 || 
                typeID == TypeID.Int32 || 
                typeID == TypeID.Int64;
        }

        Int64 ReadIntValue(TypeID typeID)
        {
            if (typeID == TypeID.Byte)
            {
                return (Int64)this.Reader.ReadByte();    
            }
            if (typeID == TypeID.Int16)
            {
                return (Int64)this.Reader.ReadInt16();    
            }
            if (typeID == TypeID.Int32)
            {
                return (Int64)this.Reader.ReadInt32();    
            }
            if(typeID == TypeID.Int64)
            {
                return this.Reader.ReadInt64();    
            }  
            throw new NotImplementedException();
        }

        string ReadStringValue()
        {
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
            var bytes = this.Reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes, 0, length);
        }

        byte BoolToByte(bool value){
            return value ? (byte)0x01 : (byte)0x00;
        }
    }
}

