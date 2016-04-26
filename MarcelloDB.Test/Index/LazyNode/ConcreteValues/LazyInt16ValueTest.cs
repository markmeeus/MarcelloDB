using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Index.LazyNode.ConcreteValues;

namespace MarcelloDB.Test.Index.LazyNode.ConcreteValues
{
    [TestFixture]
    public class LazyInt16ValueTest
    {
        [Test]
        public void Loads_Value_From_Bytes()
        {            
            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //Some data before
                .WriteByte(LazyValue<object>.TYPEID_INT16) //TYPEID
                .WriteInt16(456) //Actual data
                .WriteInt32(789) //Some data after
                .GetTrimmedBuffer();
            Assert.AreEqual(456, new LazyInt16Value(bytes, sizeof(Int32)).Value);
        }

        [Test]
        public void ToBytes_FromBytes()
        {            
            var bytes = new LazyInt16Value(123).ToBytes();
            Assert.AreEqual(123, new LazyInt16Value(bytes, 0).Value);
        }

        [Test]
        public void ByteSize()
        {
            Assert.AreEqual(sizeof(byte) + 2, new LazyInt16Value(123).ByteSize);
        }
    }
}

