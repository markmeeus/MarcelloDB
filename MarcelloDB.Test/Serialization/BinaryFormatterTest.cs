using System;
using NUnit.Framework;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class BinaryFormatterTest
    {       
        [Test]
        public void Start_Writes_Version_To_Buffer_Writer()
        {
            BinaryFormatter formatter;
            BufferWriter writer;

            writer = new BufferWriter(new byte[0]);
            formatter = new BinaryFormatter(writer);

            formatter
                .Start();
            var bytes = writer.GetTrimmedBuffer();
            Assert.AreEqual(BinaryFormatter.VERSION, bytes[0]);
        }                  

        [Test]
        public void ReadWriteBool()
        {
            TestReadWrite<bool>(true,
                (f, v)  => f.WriteBool(v),
                (f)     => f.ReadBool());
        }

        [Test]
        public void ReadWriteNullableBoolNull()
        {
            TestReadWrite<bool?>(null,
                (f, v) => f.WriteNullableBool(v),
                (f) => f.ReadNullableBool());            
        }

        [Test]
        public void ReadWriteNullableBoolTrue()
        {
            TestReadWrite<bool?>(true,
                (f, v) => f.WriteNullableBool(v),
                (f) => f.ReadNullableBool());
        }

        [Test]
        public void AssertBool()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("true"),
                (f) => f.ReadBool());
        }

        [Test]
        public void ReadWriteNullableBoolFalse()
        {
            TestReadWrite<bool?>(false,
                (f, v)  => f.WriteNullableBool(v),
                (f)     => f.ReadNullableBool());
        }

        [Test]
        public void ReadWriteByte()
        {
            TestReadWrite<byte>((byte)123,
                (f, v)  => f.WriteByte(v),
                (f)     => f.ReadByte());
        }

        [Test]
        public void ReadWriteNullableByteNull()
        {
            TestReadWrite<byte?>((byte?)null,
                (f, v)  => f.WriteNullableByte(v),
                (f)     => f.ReadNullableByte());
        }

        [Test]
        public void ReadWriteNullableByteValue()
        {
            TestReadWrite<byte?>((byte?)123,
                (f, v)  => f.WriteNullableByte(v),
                (f)     => f.ReadNullableByte());
        }

        [Test]
        public void ReadWriteInt16()
        {
            TestReadWrite<Int16>(123,
                (f, v)  => f.WriteInt16(v),
                (f)     => f.ReadInt16());
        }

        [Test]
        public void ReadWriteNullableInt16Null()
        {
            TestReadWrite<Int16?>(null,
                (f, v)  => f.WriteNullableInt16(v),
                (f)     => f.ReadNullableInt16());
        }

        [Test]
        public void ReadWriteNullableInt16Value()
        {
            TestReadWrite<Int16?>(123,
                (f, v)  => f.WriteNullableInt16(v),
                (f)     => f.ReadNullableInt16());
        }

        [Test]
        public void ReadWriteInt32()
        {
            TestReadWrite<Int32>(123,
                (f, v)  => f.WriteInt32(v),
                (f)     => f.ReadInt32());
        }

        [Test]
        public void ReadWriteNullableInt32Null()
        {
            TestReadWrite<Int32?>(null,
                (f, v)  => f.WriteNullableInt32(v),
                (f)     => f.ReadNullableInt32());
        }

        [Test]
        public void ReadWriteNullableInt32Value()
        {
            TestReadWrite<Int32?>(123,
                (f, v)  => f.WriteNullableInt32(v),
                (f)     => f.ReadNullableInt32());
        }

        [Test]
        public void ReadWriteInt64()
        {
            TestReadWrite<Int64>(123,
                (f, v)  => f.WriteInt64(v),
                (f)     => f.ReadInt64());
        }

        [Test]
        public void ReadWriteNullableInt64Null()
        {
            TestReadWrite<Int64?>(null,
                (f, v)  => f.WriteNullableInt64(v),
                (f)     => f.ReadNullableInt64());
        }

        [Test]
        public void ReadWriteNullableInt64Value()
        {
            TestReadWrite<Int64?>(123,
                (f, v)  => f.WriteNullableInt64(v),
                (f)     => f.ReadNullableInt64());
        }         

        [Test]
        public void ReadWriteDecimal()
        {            
            TestReadWrite<Decimal>(0.1m, 
                (f, v)  => f.WriteDecimal(v), 
                (f)     => f.ReadDecimal());
        }
            
        [Test]
        public void ReadWriteNullableDecimalNull()
        {                        
            TestReadWrite<Decimal?>(null,
                (f, v)  => f.WriteNullableDecimal(v),
                (f)     => f.ReadNullableDecimal());
        }

        [Test]
        public void ReadWriteNullableDecimalValue()
        {            
            TestReadWrite<Decimal?>(0.1m,
                (f, v)  => f.WriteNullableDecimal(v),
                (f)     => f.ReadNullableDecimal());
        }

        [Test]
        public void ReadWriteSingle()
        {            
            TestReadWrite<Single>((Single)0.1, 
                (f, v)  => f.WriteSingle(v), 
                (f)     => f.ReadSingle());
        }

        [Test]
        public void ReadWriteNullableSingleNull()
        {            
            TestReadWrite<Single?>(null, 
                (f, v)  => f.WriteNullableSingle(v), 
                (f)     => f.ReadNullableSingle());
        }

        [Test]
        public void ReadWriteNullableSingleValue()
        {            
            TestReadWrite<Single?>((Single)0.1, 
                (f, v)  => f.WriteNullableSingle(v), 
                (f)     => f.ReadNullableSingle());
        }

        [Test]
        public void ReadWriteDouble()
        {            
            TestReadWrite<Double>((Double)0.1, 
                (f, v)  => f.WriteDouble(v), 
                (f)     => f.ReadDouble());
        }

        [Test]
        public void ReadWriteNullableDoubleNull()
        {            
            TestReadWrite<Double?>(null, 
                (f, v)  => f.WriteNullableDouble(v), 
                (f)     => f.ReadNullableDouble());
        }

        [Test]
        public void ReadWriteNullableDoubleValue()
        {            
            TestReadWrite<Double?>((Double)0.1, 
                (f, v)  => f.WriteNullableDouble(v), 
                (f)     => f.ReadNullableDouble());
        }

        [Test]
        public void ReadWriteDateTime()
        {
            var dateTime = new DateTime(2016, 05, 02, 08, 03, 55);
            TestReadWrite<DateTime>(dateTime,
                (f, v) => f.WriteDateTime(dateTime),
                (f) => f.ReadDateTime());            
        }

        [Test]
        public void ReadWriteNullableDateTimeNull()
        {
            TestReadWrite<DateTime?>(null,
                (f, v)  => f.WriteNullableDateTime(v),
                (f)     => f.ReadNullableDateTime());            
        }

        [Test]
        public void ReadWriteNullableDateTimeValue()
        {
            var dateTime = new DateTime(2016, 05, 02, 08, 03, 55);
            TestReadWrite<DateTime?>(dateTime,
                (f, v)  => f.WriteNullableDateTime(v),
                (f)     => f.ReadNullableDateTime());            
        }   
            
        [Test]
        public void ReadWriteString()
        {
            TestReadWrite<string>("I'm a string",
                (f, v) => f.WriteString(v),
                (f) => f.ReadString());            
        }

        [Test]
        public void ReadWriteStringNull()
        {
            TestReadWrite<string>(null,
                (f, v) => f.WriteString(v),
                (f) => f.ReadString());            
        }

        void TestReadWrite<T>(T testValue, 
            Action<BinaryFormatter, T> writeValue, 
            Func<BinaryFormatter, T> readValue)
        {
            BinaryFormatter formatter;
            BufferWriter writer;

            writer = new BufferWriter(new byte[0]);
            formatter = new BinaryFormatter(writer);

            formatter
                .Start();
            writeValue(formatter, testValue);

            var bytes = writer.GetTrimmedBuffer();
            var reader = new  BufferReader(bytes);
            T readFromBytesValue = readValue(new BinaryFormatter(reader));

            Assert.AreEqual(testValue, readFromBytesValue);
        }

        void AssertTypeMismatchThrows(
            Action<BinaryFormatter> writeValue, 
            Action<BinaryFormatter> readValue)
        {
            BinaryFormatter formatter;
            BufferWriter writer;

            writer = new BufferWriter(new byte[0]);
            formatter = new BinaryFormatter(writer);

            formatter
                .Start();
            writeValue(formatter);

            var bytes = writer.GetTrimmedBuffer();
            var reader = new  BufferReader(bytes);
            formatter = new BinaryFormatter(reader);
            Assert.Throws<InvalidOperationException>(() => readValue(formatter));
        }
    }
}

