using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Records;

namespace MarcelloDB.Test.Records
{
    [TestFixture]
    public class ValueWithAddressIndexKeyTest
    {
        [SetUp]
        public void Initialize()
        {
        }

        [Test]
        public void Can_Be_Serialized()
        {
            var key = new ValueWithAddressIndexKey{A = 10, V = 20 };
            var serializer = new BsonSerializer<ValueWithAddressIndexKey>();
            var deserialized = serializer.Deserialize(
                serializer.Serialize(key)
            );
            Assert.AreEqual(key.A, deserialized.A);
            Assert.AreEqual(key.V, deserialized.V);
        }

        [Test]
        public void Compares_On_Value_First()
        {
            var key1 = new ValueWithAddressIndexKey{V = 1, A = 2 };
            var key2 = new ValueWithAddressIndexKey{V = 2, A = 1 };
            Assert.IsTrue(key1.CompareTo(key2) < 0);
        }

        [Test]
        public void Compares_On_Address_When_Size_Is_Equal()
        {
            var key1 = new ValueWithAddressIndexKey{V = 1, A = 1 };
            var key2 = new ValueWithAddressIndexKey{V = 1, A = 2 };
            Assert.IsTrue(key1.CompareTo(key2) < 0);
        }
        [Test]
        public void Considers_Equal_When_Address_Missing()
        {
            var key1 = new ValueWithAddressIndexKey{V = 1, A = 1 };
            var key2 = new ValueWithAddressIndexKey{V = 1};
            Assert.AreEqual(0, key1.CompareTo(key2));
            Assert.AreEqual(0, key2.CompareTo(key1));
            Assert.IsTrue(key1.Equals(key2));
            Assert.IsTrue(key2.Equals(key1));
        }
    }
}

