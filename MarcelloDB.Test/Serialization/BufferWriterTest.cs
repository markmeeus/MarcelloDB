using System;
using NUnit.Framework;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class BufferWriterTest
    {
        Marcello Session { get; set; }

        BufferWriter _writer;

        [SetUp]
        public void Initialize()
        {
            this.Session = new Marcello(new InMemoryStreamProvider());

            _writer = new BufferWriter(this.Session, this.Session.ByteBufferManager.Create(1024), true);
        }

        [Test]
        public void Write_Int32_Little_Endian()
        {
            _writer.WriteInt32((Int32)0x11223344);
            Assert.AreEqual(0x44, _writer.Buffer.Bytes[0]);
            Assert.AreEqual(0x33, _writer.Buffer.Bytes[1]);
            Assert.AreEqual(0x22, _writer.Buffer.Bytes[2]);
            Assert.AreEqual(0x11, _writer.Buffer.Bytes[3]);
        }

        [Test]
        public void Write_Two_Ints()
        {
            _writer.WriteInt32((Int32)0x11223344);
            _writer.WriteInt32((Int32)0x55667788);
            Assert.AreEqual(0x88, _writer.Buffer.Bytes[4]);
            Assert.AreEqual(0x77, _writer.Buffer.Bytes[5]);
            Assert.AreEqual(0x66, _writer.Buffer.Bytes[6]);
            Assert.AreEqual(0x55, _writer.Buffer.Bytes[7]);
        }

        [Test]
        public void Write_Int64_Little_Endian()
        {
            _writer.WriteInt64((Int64)0x1122334455667788);
            Assert.AreEqual(0x88, _writer.Buffer.Bytes[0]);
            Assert.AreEqual(0x77, _writer.Buffer.Bytes[1]);
            Assert.AreEqual(0x66, _writer.Buffer.Bytes[2]);
            Assert.AreEqual(0x55, _writer.Buffer.Bytes[3]);
            Assert.AreEqual(0x44, _writer.Buffer.Bytes[4]);
            Assert.AreEqual(0x33, _writer.Buffer.Bytes[5]);
            Assert.AreEqual(0x22, _writer.Buffer.Bytes[6]);
            Assert.AreEqual(0x11, _writer.Buffer.Bytes[7]);
        }

        [Test]
        public void Write_Int_Little_Endian_On_Big_Endian_System()
        {
            var writer = new BufferWriter(this.Session, this.Session.ByteBufferManager.Create(1024), false);
            writer.WriteInt32((Int32)0X44332211);
            //on a big endian systems, bytes should be reordered
            Assert.AreEqual(0x44, writer.Buffer.Bytes[0]);
            Assert.AreEqual(0x33, writer.Buffer.Bytes[1]);
            Assert.AreEqual(0x22, writer.Buffer.Bytes[2]);
            Assert.AreEqual(0x11, writer.Buffer.Bytes[3]);
        }

        [Test]
        public void Buffer_Grows_When_Overrun()
        {
            var writer = new BufferWriter(this.Session, this.Session.ByteBufferManager.Create(12), true);
            writer.WriteInt64((Int64)123); //position at 8
            writer.WriteInt64((Int64)456); 
            Assert.AreEqual(16, writer.Buffer.Length);
        }

        [Test]
        public void Buffer_Grows_When_Overrun_Edge_Case()
        {
            var writer = new BufferWriter(this.Session, this.Session.ByteBufferManager.Create(8), true);
            writer.WriteInt64((Int64)123); //position at 8
            writer.WriteInt64((Int64)456); 
            Assert.AreEqual(16, writer.Buffer.Length);
        }

        [Test]
        public void Get_Trimmed_Buffer_Returns_Correct_Buffer()
        {
            var writer = new BufferWriter(this.Session, this.Session.ByteBufferManager.Create(8), true);
            writer.WriteInt64((Int64)123);
            Assert.AreEqual(8, writer.GetTrimmedBuffer().Length); //8 bytes
        }

        [Test]
        public void Get_Trimmed_Buffer_Trims_Buffer()
        {
            var writer = new BufferWriter(this.Session, this.Session.ByteBufferManager.Create(1024), true);
            writer.WriteInt64((Int64)123);
            Assert.AreEqual(8, writer.GetTrimmedBuffer().Length); //8 bytes
        }
    }
}

