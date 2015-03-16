using System;
using NUnit.Framework;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class BufferReaderTest
    {
        [SetUp]
        public void Initialize()
        { 
        }

        [Test]
        public void ReadInt32_Little_Endian()
        {
            var reader = new BufferReader(new byte[]{0x11, 0x22, 0x33, 0x44}, true);
            Int32 readInt = reader.ReadInt32();
            Assert.AreEqual(0x44332211, readInt);
        }

        [Test]
        public void ReadInt32_Twice()
        {
            var reader = new BufferReader(new byte[]
                {
                    0x11, 0x22, 0x33, 0x44,
                    0x22, 0x33, 0x44, 0x55,
                }, true);
            reader.ReadInt32();
            Int32 readInt = reader.ReadInt32();
            Assert.AreEqual(0x55443322, readInt);
        }

        [Test]
        public void ReadInt64_Little_Endian()
        {
            var reader = new BufferReader(new byte[]
                {
                    0x11, 0x11, 0x22, 0x22,
                    0x33, 0x33, 0x44, 0x55,
                }, true);
            Int64 readInt = reader.ReadInt64();
            Assert.AreEqual(0x5544333322221111, readInt);
        }
            
        [Test]
        public void ReadInt32_Little_Endian_On_Big_Endian_System()
        {
            var reader = new BufferReader(new byte[] {0x11, 0x22, 0x33, 0x44}, false);
            var readInt = reader.ReadInt32();
            Assert.AreEqual(0x11223344, readInt);
        }

        [Test]
        public void ReadInt64_Little_Endian_On_Big_Endian_System()
        {
            var reader = new BufferReader(
                new byte[] {0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18}, false);
            var readInt = reader.ReadInt64();
            Assert.AreEqual(0x1112131415161718, readInt);
        }
    }
}

