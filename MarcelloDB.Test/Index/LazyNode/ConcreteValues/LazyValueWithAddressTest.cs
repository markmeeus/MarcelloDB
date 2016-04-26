using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Records;
using MarcelloDB.Index.LazyNode.ConcreteValues;

namespace MarcelloDB.Test.Index.LazyNode.ConcreteValues
{
    [TestFixture]
    public class LazyValueWithAddressTest
    {
        [Test]
        public void Loads_Value_From_Bytes()
        {
            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(0)
                .WriteByte(LazyValue<object>.TYPEID_VALUE_WITH_ADDRESS_INDEX_KEY)
                .WriteInt64(123)
                .WriteByte(LazyValue<object>.TYPEID_INT32)
                .WriteInt32(456)
                .GetTrimmedBuffer();
            Assert.AreEqual(123, new LazyValueWithAddress<Int32>(bytes, sizeof(Int32)).Value.A);
            Assert.AreEqual(456, new LazyValueWithAddress<Int32>(bytes, sizeof(Int32)).Value.V);
        }

        [Test]
        public void ToBytes_FromBytes()
        {
            var key = new ValueWithAddressIndexKey<Int32>();
            key.A = 123;
            key.V = 456;
            var bytes = new LazyValueWithAddress<Int32>(key).ToBytes();
            var value = new LazyValueWithAddress<Int32>(bytes, 0).Value;
            Assert.AreEqual(123, value.A);
            Assert.AreEqual(456, value.V);
        }

        [Test]
        public void ByteSize()
        {
            var key = new ValueWithAddressIndexKey<string>();
            key.A = 123;
            key.V = "12345";
            Assert.AreEqual(sizeof(byte) + sizeof(Int32) + sizeof(Int64) + sizeof(byte) + 5,
                new LazyValueWithAddress<string>(key).ByteSize);
        }
    }
}

