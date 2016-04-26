using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Index.LazyNode.ConcreteValues;
using System.Text;

namespace MarcelloDB.Test.Index.LazyNode.ConcreteValues
{
    [TestFixture]
    public class LazyStringValueTest
    {
        [Test]
        public void Loads_Value_From_Bytes()
        {
            var stringBytes = Encoding.UTF8.GetBytes("Some string");
            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(0)
                .WriteByte(LazyValue<string>.TYPEID_STRING)
                .WriteInt32(stringBytes.Length)
                .WriteBytes(stringBytes)
                .GetTrimmedBuffer();
            Assert.AreEqual("Some string", new LazyStringValue(bytes, sizeof(Int32)).Value);
        }

        [Test]
        public void ToBytes_FromBytes()
        {
            var bytes = new LazyStringValue("Some string").ToBytes();
            var value = new LazyStringValue(bytes, 0).Value;
            Assert.AreEqual("Some string", value);
        }

        [Test]
        public void ByteSize()
        {
            var stringBytes = Encoding.UTF8.GetBytes("Some string");
            Assert.AreEqual(sizeof(byte) + sizeof(Int32) + stringBytes.Length, new LazyStringValue("Some string").ByteSize);
        }
    }
}

