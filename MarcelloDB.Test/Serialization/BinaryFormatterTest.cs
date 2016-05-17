using System;
using NUnit.Framework;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class BinaryFormatterTest
    {
        [Test]
        public void Constructor_Writes_Version_To_Buffer_Writer()
        {
            BufferWriter writer;
            #pragma warning disable 0219 //formatter constuctor is under test
            BinaryFormatter formatter;
            #pragma warning restore 0219



            writer = new BufferWriter(new byte[0]);
            formatter = new BinaryFormatter(writer);

            var bytes = writer.GetTrimmedBuffer();
            Assert.AreEqual(BinaryFormatter.FORMATTER_VERSION, bytes[0]);
        }

        [Test]
        public void ReadWriteBool()
        {
            TestReadWrite<bool>(true,
                (f, v)  => f.WriteBool(v),
                (f)     => f.ReadBool());
        }

        [Test]
        public void AssertBool()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("true"),
                (f) => f.ReadBool());
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
        public void ReadWriteNullableBoolFalse()
        {
            TestReadWrite<bool?>(false,
                (f, v)  => f.WriteNullableBool(v),
                (f)     => f.ReadNullableBool());
        }

        [Test]
        public void AssertNullableBool()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("true"),
                (f) => f.ReadNullableBool());
        }

        [Test]
        public void ReadWriteByte()
        {
            TestReadWrite<byte>((byte)123,
                (f, v)  => f.WriteByte(v),
                (f)     => f.ReadByte());
        }

        [Test]
        public void AssertByte()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("byte"),
                (f) => f.ReadByte());
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
        public void AssertNullableByte()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("nullable byte"),
                (f) => f.ReadNullableByte());
        }

        [Test]
        public void ReadWriteInt16()
        {
            TestReadWrite<Int16>(123,
                (f, v)  => f.WriteInt16(v),
                (f)     => f.ReadInt16());
        }

        [Test]
        public void AssertInt16()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("Int16"),
                (f) => f.ReadInt16());
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
        public void AssertNullableInt16()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("NullableInt16"),
                (f) => f.ReadNullableInt16());
        }

        [Test]
        public void ReadWriteInt32()
        {
            TestReadWrite<Int32>(123,
                (f, v)  => f.WriteInt32(v),
                (f)     => f.ReadInt32());
        }

        [Test]
        public void AssertInt32()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("Int32"),
                (f) => f.ReadInt32());
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
        public void AssertNullableInt32()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("NullableInt32"),
                (f) => f.ReadNullableInt32());
        }

        [Test]
        public void ReadWriteInt64()
        {
            TestReadWrite<Int64>(123,
                (f, v)  => f.WriteInt64(v),
                (f)     => f.ReadInt64());
        }

        [Test]
        public void AssertInt64()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("Int64"),
                (f) => f.ReadInt64());
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
        public void AssertNullableInt64()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("NullableInt64"),
                (f) => f.ReadNullableInt64());
        }

        [Test]
        public void ReadWriteDecimal()
        {
            TestReadWrite<Decimal>(0.1m,
                (f, v)  => f.WriteDecimal(v),
                (f)     => f.ReadDecimal());
        }

        [Test]
        public void AssertDecimal()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("Decimal"),
                (f) => f.ReadDecimal());
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
        public void AssertNullableDecimal()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("NullableDecimal"),
                (f) => f.ReadNullableDecimal());
        }

        [Test]
        public void ReadWriteSingle()
        {
            TestReadWrite<Single>((Single)0.1,
                (f, v)  => f.WriteSingle(v),
                (f)     => f.ReadSingle());
        }

        [Test]
        public void AssertSingle()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("Single"),
                (f) => f.ReadSingle());
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
        public void AssertNullableSingle()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("NullableSingle"),
                (f) => f.ReadNullableSingle());
        }

        [Test]
        public void ReadWriteDouble()
        {
            TestReadWrite<Double>((Double)0.1,
                (f, v)  => f.WriteDouble(v),
                (f)     => f.ReadDouble());
        }

        [Test]
        public void AssertDouble()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("Double"),
                (f) => f.ReadDouble());
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
        public void AssertNullableDouble()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("NullableDouble"),
                (f) => f.ReadNullableDouble());
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
        public void AssertDateTime()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("Decimal"),
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
        public void AssertNullableDateTime()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteString("NullableDateTime"),
                (f) => f.ReadNullableDateTime());
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

        [Test]
        public void AssertString()
        {
            AssertTypeMismatchThrows(
                (f) => f.WriteInt32(123),
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

            writeValue(formatter);

            var bytes = writer.GetTrimmedBuffer();
            var reader = new  BufferReader(bytes);
            formatter = new BinaryFormatter(reader);
            Assert.Throws<InvalidOperationException>(() => readValue(formatter));
        }
    }
}

