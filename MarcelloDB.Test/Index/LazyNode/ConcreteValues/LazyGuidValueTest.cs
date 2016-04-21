using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Index.LazyNode.ConcreteValues;

namespace MarcelloDB.Test.Index.LazyNode.ConcreteValues
{
    [TestFixture]
    public class LazyGuidValueTest
    {
        [Test]
        public void Loads_Value_From_Bytes()
        {
            var guid = Guid.NewGuid();
            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //Some data before
                .WriteByte(LazyValue<object>.TYPEID_GUID) //TTYPEID
                .WriteBytes(guid.ToByteArray()) //Actual value
                .WriteInt32(789) //Some data after
                .GetTrimmedBuffer();
            Assert.AreEqual(guid, new LazyGuidValue(bytes, sizeof(Int32)).Value);
        }

        [Test]
        public void ToBytes_FromBytes()
        {
            var guid = Guid.NewGuid();
            var bytes = new LazyGuidValue(guid).ToBytes();
            Assert.AreEqual(guid, new LazyGuidValue(bytes, 0).Value);
        }

        [Test]
        public void ByteSize()
        {
            Assert.AreEqual(sizeof(byte) + 16, new LazyGuidValue(Guid.NewGuid()).ByteSize);
        }
    }
}

