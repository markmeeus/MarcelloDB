using System;
using NUnit.Framework;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class BufferReaderTest
    {
        Marcello Session { get; set; }

        [SetUp]
        public void Initialize()
        { 
            this.Session = new Marcello(new InMemoryStreamProvider());
        }

        [Test]
        public void ReadInt32_Little_Endian()
        {
            var buffer = this.Session.ByteBufferManager.FromBytes(new byte[]{ 0x11, 0x22, 0x33, 0x44 });
            var reader = new BufferReader(this.Session, buffer , true);
            Int32 readInt = reader.ReadInt32();
            Assert.AreEqual(0x44332211, readInt);
        }

        [Test]
        public void ReadInt32_Twice()
        {
            var buffer = this.Session.ByteBufferManager.FromBytes(
                new byte[]
                {
                    0x11, 0x22, 0x33, 0x44,
                    0x22, 0x33, 0x44, 0x55,
                });
            var reader = new BufferReader(this.Session, buffer, true);
            reader.ReadInt32();
            Int32 readInt = reader.ReadInt32();
            Assert.AreEqual(0x55443322, readInt);
        }

        [Test]
        public void ReadInt64_Little_Endian()
        {
            var buffer = this.Session.ByteBufferManager.FromBytes(
                new byte[]
                {
                    0x11, 0x11, 0x22, 0x22,
                    0x33, 0x33, 0x44, 0x55,
                });
            var reader = new BufferReader(this.Session, buffer, true);
            Int64 readInt = reader.ReadInt64();
            Assert.AreEqual(0x5544333322221111, readInt);
        }
            
        [Test]
        public void ReadInt32_Little_Endian_On_Big_Endian_System()
        {
            var buffer = this.Session.ByteBufferManager.FromBytes(new byte[]{ 0x11, 0x22, 0x33, 0x44 });
            var reader = new BufferReader(this.Session, buffer , false);
            var readInt = reader.ReadInt32();
            Assert.AreEqual(0x11223344, readInt);
        }

        [Test]
        public void ReadInt64_Little_Endian_On_Big_Endian_System()
        {
            var buffer = this.Session.ByteBufferManager.FromBytes(
                new byte[]{ 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18});
            var reader = new BufferReader(this.Session, buffer , false);

            var readInt = reader.ReadInt64();
            Assert.AreEqual(0x1112131415161718, readInt);
        }
    }
}

