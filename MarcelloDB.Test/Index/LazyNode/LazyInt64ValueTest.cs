using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Index.LazyNode.ConcreteValues;

namespace MarcelloDB.Test.Index.LazyNode.ConcreteValues
{
    [TestFixture]
    public class LazyInt64ValueTest
    {
        [Test]
        public void Loads_Value_From_Bytes()
        {
            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(0)
                .WriteByte(LazyValue<int>.TYPEID_INT64)
                .WriteInt64(123)
                .GetTrimmedBuffer();
            Assert.AreEqual(123, new LazyInt64Value(bytes, sizeof(Int32)).Value);
        }

        [Test]
        public void ToBytes_FromBytes()
        {
            var bytes = new LazyInt64Value(123).ToBytes();
            var value = new LazyInt64Value(bytes, 0).Value;
            Assert.AreEqual(123, value);
        }
    }
}

