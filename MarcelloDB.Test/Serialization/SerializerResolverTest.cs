using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Records;
using MarcelloDB.Index;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class SerializerResolverTest
    {
        [Test]
        public void SerializerFor_Object_Returns_BSONSerializer()
        {
            var serializer = new SerializerResolver().SerializerFor<object>();
            Assert.AreEqual(typeof(BsonSerializer<object>), serializer.GetType());
        }

        [Test]
        public void SerializerFor_IndexMetaRecord_Returns_Correct_Serializer()
        {
            var serializer = new SerializerResolver().SerializerFor<IndexMetaRecord>();
            Assert.AreEqual(typeof(IndexMetaRecordSerializer), serializer.GetType());
        }

        [Test]
        public void SerializerFor_EmptyRecordIndexNode_Returns_Specific_Serializer()
        {
            var serializer = new SerializerResolver()
                .SerializerFor<Node<EmptyRecordIndexKey>>();
            Assert.AreEqual(typeof(EmptyRecordIndexNodeSerializer), serializer.GetType());
        }

        [Test]
        public void SerializerForEmptyRecordIndexNodeType_Returns_Specific_Serializer()
        {
            var serializer = new SerializerResolver().SerializerFor<Node<EmptyRecordIndexKey>>();
            Assert.AreEqual(typeof(EmptyRecordIndexNodeSerializer), serializer.GetType());
        }

        [Test]
        public void SerializerForBtreeNodeInt64_Returns_FormatterSerializer()
        {
            var serializer = new SerializerResolver().SerializerFor<Node<Int64>>();
            Assert.AreEqual(typeof(BTreeNodeBinaryFormatterSerializer<Int64>), serializer.GetType());
        }

        [Test]
        public void SerializerForBtreeNodeValueWithAddress_Returns_FormatterSerializer()
        {
            var serializer = new SerializerResolver().SerializerFor<Node<ValueWithAddressIndexKey<String>>>();
            Assert.AreEqual(typeof(BTreeNodeBinaryFormatterSerializer<ValueWithAddressIndexKey<String>>), serializer.GetType());
        }

        class CustomIndexKey{ }

        [Test]
        public void SerializerForBtreeNodeCustomType_Returns_Bson_Serializer()
        {
            var serializer = new SerializerResolver().SerializerFor<Node<CustomIndexKey>>();
            Assert.AreEqual(typeof(BTreeNodeBsonSerializer<CustomIndexKey>), serializer.GetType());
        }
    }
}

