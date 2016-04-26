using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using System.Collections.Generic;
using System.Linq;
using MarcelloDB.Index.LazyNode.ConcreteValues;
using MarcelloDB.Records;

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

        [Test]
        public void FromBytes_ToBytes_Int16()
        {
            var list = new LazyValueList<Int16>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //some data before
                .WriteBytes(list.ToBytes())
                .WriteInt32(456)
                .GetTrimmedBuffer();

            list = new LazyValueList<Int16>(bytes, sizeof(Int32));

            Assert.AreEqual(new List<Int16>{ 1, 2, 3 }, list.ToList());
        }

        [Test]
        public void FromBytes_ToBytes_Int32()
        {
            var list = new LazyValueList<Int32>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //some data before
                .WriteBytes(list.ToBytes())
                .WriteInt32(456)
                .GetTrimmedBuffer();

            list = new LazyValueList<Int32>(bytes, sizeof(Int32));

            Assert.AreEqual(new List<Int32>{ 1, 2, 3 }, list.ToList());
        }

        [Test]
        public void FromBytes_ToBytes_Int64()
        {
            var list = new LazyValueList<Int64>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //some data before
                .WriteBytes(list.ToBytes())
                .WriteInt32(456)
                .GetTrimmedBuffer();

            list = new LazyValueList<Int64>(bytes, sizeof(Int32));

            Assert.AreEqual(new List<Int64>{ 1, 2, 3 }, list.ToList());
        }

        [Test]
        public void FromBytes_ToBytes_Guid()
        {
            var list = new LazyValueList<Guid>();
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var guid3 = Guid.NewGuid();
            list.Add(guid1);
            list.Add(guid2);
            list.Add(guid3);

            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //some data before
                .WriteBytes(list.ToBytes())
                .WriteInt32(456)
                .GetTrimmedBuffer();

            list = new LazyValueList<Guid>(bytes, sizeof(Int32));

            Assert.AreEqual(new List<Guid>{ guid1, guid2, guid3 }, list.ToList());
        }


        [Test]
        public void FromBytes_ToBytes_String()
        {
            var list = new LazyValueList<string>();
            list.Add("First");
            list.Add("Second");
            list.Add("Third");

            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //some data before
                .WriteBytes(list.ToBytes())
                .WriteInt32(456)
                .GetTrimmedBuffer();

            list = new LazyValueList<string>(bytes, sizeof(Int32));

            Assert.AreEqual(new List<string>{ "First", "Second", "Third" }, list.ToList());
        }


        [Test]
        public void FromBytes_ToBytes_String_With_Address()
        {
            var list = new LazyValueList<ValueWithAddressIndexKey<string>>();
            list.Add(new ValueWithAddressIndexKey<string>{ V = "First", A= 1});
            list.Add(new ValueWithAddressIndexKey<string>{ V = "Second", A= 2});
            list.Add(new ValueWithAddressIndexKey<string>{ V = "Third", A= 3});

            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //some data before
                .WriteBytes(list.ToBytes())
                .WriteInt32(456)
                .GetTrimmedBuffer();

            list = new LazyValueList<ValueWithAddressIndexKey<string>>(bytes, sizeof(Int32));

            Assert.AreEqual(new List<string>{ "First", "Second", "Third" }, list.ToList()
                .Select((i)=>i.V));
        }

        [Test]
        public void FromBytes_ToBytes_Int32_With_Address()
        {
            var list = new LazyValueList<ValueWithAddressIndexKey<Int32>>();
            list.Add(new ValueWithAddressIndexKey<Int32>{ V = 1, A= 1});
            list.Add(new ValueWithAddressIndexKey<Int32>{ V = 2, A= 2});
            list.Add(new ValueWithAddressIndexKey<Int32>{ V = 3, A= 3});

            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //some data before
                .WriteBytes(list.ToBytes())
                .WriteInt32(456)
                .GetTrimmedBuffer();

            list = new LazyValueList<ValueWithAddressIndexKey<Int32>>(bytes, sizeof(Int32));

            Assert.AreEqual(new List<Int32>{ 1, 2, 3 }, list.ToList()
                .Select((i)=>i.V));
        }

        [Test]
        public void FromBytes_ToBytes_Int64_With_Address()
        {
            var list = new LazyValueList<ValueWithAddressIndexKey<Int64>>();
            list.Add(new ValueWithAddressIndexKey<Int64>{ V = 1, A= 1});
            list.Add(new ValueWithAddressIndexKey<Int64>{ V = 2, A= 2});
            list.Add(new ValueWithAddressIndexKey<Int64>{ V = 3, A= 3});

            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //some data before
                .WriteBytes(list.ToBytes())
                .WriteInt32(456)
                .GetTrimmedBuffer();

            list = new LazyValueList<ValueWithAddressIndexKey<Int64>>(bytes, sizeof(Int32));

            Assert.AreEqual(new List<Int32>{ 1, 2, 3 }, list.ToList()
                .Select((i)=>i.V));
        }

        [Test]
        public void FromBytes_ToBytes_Guid_With_Address()
        {
            var list = new LazyValueList<ValueWithAddressIndexKey<Guid>>();

            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var guid3 = Guid.NewGuid();

            list.Add(new ValueWithAddressIndexKey<Guid>{ V = guid1, A= 1});
            list.Add(new ValueWithAddressIndexKey<Guid>{ V = guid2, A= 2});
            list.Add(new ValueWithAddressIndexKey<Guid>{ V = guid3, A= 3});

            var bytes = new BufferWriter(new byte[0])
                .WriteInt32(123) //some data before
                .WriteBytes(list.ToBytes())
                .WriteInt32(456)
                .GetTrimmedBuffer();

            list = new LazyValueList<ValueWithAddressIndexKey<Guid>>(bytes, sizeof(Int32));

            Assert.AreEqual(new List<Guid>{ guid1, guid2, guid3 }, list.ToList()
                .Select((i)=>i.V));
        }
    }
}

