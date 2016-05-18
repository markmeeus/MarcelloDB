using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using System.Linq;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class CollectionFileRootSerializerTest
    {
        CollectionFileRootSerializer _serializer;

        [SetUp]
        public void Initialize()
        {
            _serializer = new CollectionFileRootSerializer();
        }

        [Test]
        public void SerializesCollectionFileRoot()
        {
            var root = new CollectionFileRoot();
            root.Head = 456;
            root.NamedRecordIndexAddress = 7;
            root.FormatVersion = 8;

            var deserialized = _serializer.Deserialize(_serializer.Serialize(root));

            Assert.AreEqual(456, deserialized.Head);
            Assert.AreEqual(8, deserialized.FormatVersion);
        }

        [Test]
        public void DeserializedShouldNotBeDirty()
        {
            var root = new CollectionFileRoot();
            root.Head = 456;
            root.NamedRecordIndexAddress = 7;
            root.FormatVersion = 8;

            var deserialized = _serializer.Deserialize(_serializer.Serialize(root));

            Assert.IsFalse(deserialized.IsDirty);
        }
    }
}

