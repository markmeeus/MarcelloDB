using System;
using NUnit.Framework;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class BinaryFormatterTest
    {
        BinaryFormatter _formatter;
        BufferWriter _writer;

        [SetUp]
        public void Initialize()
        {
            _writer = new BufferWriter(new byte[0]);
            _formatter = new BinaryFormatter(_writer);
        }

        [Test]
        public void Start_Writes_Version_To_Buffer_Writer()
        {
            _formatter
                .Start();
            var bytes = _writer.GetTrimmedBuffer();
            Assert.AreEqual(BinaryFormatter.VERSION, bytes[0]);
        }                  

        [Test]
        public void ReadWriteBool()
        {
            _formatter
                .Start()
                .WriteBool(true);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readBool = new BinaryFormatter(reader)
                .ReadBool();            

            Assert.AreEqual(true, readBool);
        }

        [Test]
        public void ReadWriteNullableBoolNull()
        {
            _formatter
                .Start()
                .WriteNullableBool((bool?)null);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readBool = new BinaryFormatter(reader)
                .ReadNullableBool();            

            Assert.AreEqual(null, readBool);
        }

        [Test]
        public void ReadWriteNullableBoolTrue()
        {
            _formatter
                .Start()
                .WriteNullableBool((bool?)true);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readBool = new BinaryFormatter(reader)
                .ReadNullableBool();            

            Assert.AreEqual(true, readBool);
        }

        [Test]
        public void ReadWriteNullableBoolFalse()
        {
            _formatter
                .Start()
                .WriteNullableBool((bool?)false);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readBool = new BinaryFormatter(reader)
                .ReadNullableBool();            

            Assert.AreEqual(false, readBool);
        }

        [Test]
        public void ReadWriteByte()
        {
            _formatter
                .Start()
                .WriteByte(123);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readByte = new BinaryFormatter(reader)
                .ReadByte();            

            Assert.AreEqual(123, readByte);
        }

        [Test]
        public void ReadWriteNullableByteNull()
        {
            _formatter
                .Start()
                .WriteNullableByte(null);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readByte = new BinaryFormatter(reader)
                .ReadNullableByte();            

            Assert.AreEqual(null, readByte);
        }

        [Test]
        public void ReadWriteNullableByteValue()
        {
            _formatter
                .Start()
                .WriteNullableByte(123);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readByte = new BinaryFormatter(reader)
                .ReadNullableByte();            

            Assert.AreEqual(123, readByte);
        }

        [Test]
        public void ReadWriteInt16()
        {
            _formatter
                .Start()
                .WriteInt16(123);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readInt = new BinaryFormatter(reader)
                .ReadInt16();                        
            Assert.AreEqual(123, readInt);
        }

        [Test]
        public void ReadWriteInt32()
        {
            _formatter
                .Start()
                .WriteInt32(123);
            
            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readInt = new BinaryFormatter(reader)
                .ReadInt32();            

            Assert.AreEqual(123, readInt);
        }

        [Test]
        public void ReadWriteInt64()
        {
            _formatter
                .Start()
                .WriteInt64(123);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readInt = new BinaryFormatter(reader)
                .ReadInt64();            

            Assert.AreEqual(123, readInt);
        }

        [Test]
        public void ReadWriteDateTime()
        {
            var dateTime = new DateTime(2016, 05, 02, 08, 03, 55);
            _formatter
                .Start()
                .WriteDateTime(dateTime);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readInt = new BinaryFormatter(reader)
                .ReadDateTime();            

            Assert.AreEqual(dateTime, readInt);
        }

        [Test]
        public void ReadInt64_From_Int32()
        {
            _formatter
                .Start()
                .WriteInt32(123);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readInt = new BinaryFormatter(reader)
                .ReadInt64();            

            Assert.AreEqual(123, readInt);
        }

        [Test]
        public void ReadWriteString()
        {
            _formatter
                .Start()
                .WriteString("I'm really a string");

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readString = new BinaryFormatter(reader)
                .ReadString();            

            Assert.AreEqual("I'm really a string", readString);
        }

        [Test]
        public void ReadString_From_Int64()
        {
            _formatter
                .Start()
                .WriteInt64(123);

            var bytes = _writer.GetTrimmedBuffer();
            var reader = new BufferReader(bytes);
            var readString = new BinaryFormatter(reader)
                .ReadString();

            Assert.AreEqual("123", readString);             
        }
    }
}

