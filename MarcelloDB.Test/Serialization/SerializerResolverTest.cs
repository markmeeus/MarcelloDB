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
                .SerializerFor<Node<EmptyRecordIndexKey, Int64>>();
            Assert.AreEqual(typeof(EmptyRecordIndexNodeSerializer), serializer.GetType());
        }

        [Test]
        public void SerializerForEmptyRecordIndexNodeType_Returns_Specific_Serializer()
        {
            var serializer = new SerializerResolver().SerializerFor<Node<EmptyRecordIndexKey, Int64>>();
            Assert.AreEqual(typeof(EmptyRecordIndexNodeSerializer), serializer.GetType());
        }
    }
}

