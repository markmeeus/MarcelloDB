using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Index.LazyNode.ConcreteValues;

namespace MarcelloDB.Test.Index.LazyNode.ConcreteValues
{
    [TestFixture]
    public class LazyInt32ValueTest
    {
        [Test]
        public void Loads_Value_From_Bytes()
        {
            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //Some data before
                .WriteByte(LazyValue<int>.TYPEID_INT32) //TTYPEID
                .WriteInt32(456) //Actual value
                .WriteInt32(789) //Some data after
                .GetTrimmedBuffer();
            Assert.AreEqual(456, new LazyInt32Value(bytes, sizeof(Int32)).Value);
        }

        [Test]
        public void ToBytes_FromBytes()
        {
            var bytes = new LazyInt32Value(123).ToBytes();
            Assert.AreEqual(123, new LazyInt32Value(bytes, 0).Value);
        }
    }
}

