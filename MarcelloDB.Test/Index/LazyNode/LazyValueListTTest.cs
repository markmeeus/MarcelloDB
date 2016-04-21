using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace MarcelloDB.Test.Index.LazyNode
{
    [TestFixture]
    public class LazyValueListTTest
    {
        [Test]
        public void Construct_With_Bytes_Int32()
        {            
            var writer = new BufferWriter(new byte[0]);
            var bytes = writer
                .WriteInt32(123) //some data before
                .WriteInt32(3) // 3 items in the list
                .WriteByte(LazyValue<object>.TYPEID_INT32)
                .WriteInt32(123)
                .WriteByte(LazyValue<object>.TYPEID_INT32)
                .WriteInt32(456)
                .WriteByte(LazyValue<object>.TYPEID_INT32)
                .WriteInt32(789)
                .GetTrimmedBuffer();

            var list = new LazyValueList<Int32>(bytes, sizeof(Int32));
            Assert.AreEqual(new List<Int32>{ 123, 456, 789 }, list.ToList());
        }

        [Test]
        public void Construct_With_Bytes_Int64()
        {            
            var writer = new BufferWriter(new byte[0]);
            var bytes = writer
                .WriteInt32(123) //some data before
                .WriteInt32(3) // 3 items in the list
                .WriteByte(LazyValue<object>.TYPEID_INT64)
                .WriteInt64(123)
                .WriteByte(LazyValue<object>.TYPEID_INT64)
                .WriteInt64(456)
                .WriteByte(LazyValue<object>.TYPEID_INT64)
                .WriteInt64(789)
                .GetTrimmedBuffer();

            var list = new LazyValueList<Int64>(bytes, sizeof(Int32));
            Assert.AreEqual(new List<Int64>{ 123, 456, 789 }, list.ToList());
        }
    }
}

