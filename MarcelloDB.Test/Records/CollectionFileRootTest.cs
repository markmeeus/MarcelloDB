using System;
using MarcelloDB.Records;
using System.Collections.Generic;
using NUnit.Framework;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Records
{
    [TestFixture]
    public class CollectionFileRootTest
    {
        [SetUp]
        public void Initialize()
        {

        }

        [Test]
        public void Create_Creates_A_New_Instance()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            Assert.AreEqual(CollectionFileRoot.INITIAL_HEAD, collectionFileRoot.Head);
        }

        [Test]
        public void Set_Collection_Root_Adds_Collection_Root_Address()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.SetCollectionRootAddress("collection", 123);
            Assert.AreEqual(123, collectionFileRoot.CollectionRootAddress("collection"));
        }

        [Test]
        public void Set_Collection_Root_Updates_Collection_Root_Address()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.SetCollectionRootAddress("collection", 123);
            collectionFileRoot.SetCollectionRootAddress("collection", 456);
            Assert.AreEqual(456, collectionFileRoot.CollectionRootAddress("collection"));
        }

        [Test]
        public void Is_Dirty_When_New()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            Assert.IsTrue(collectionFileRoot.IsDirty);
        }

        [Test]
        public void Set_Head_To_Same_Value_Keeps_It_Clean()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.Head = 1;
            collectionFileRoot.Clean();
            collectionFileRoot.Head = 1;
            Assert.IsFalse(collectionFileRoot.IsDirty);
        }

        [Test]
        public void Set_Head_To_Different_Value_Keeps_It_Clean()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.Head = 1;
            collectionFileRoot.Clean();
            collectionFileRoot.Head = 2;
            Assert.IsTrue(collectionFileRoot.IsDirty);
        }

        [Test]
        public void Is_Clean_When_NamedRecordIndex_Set_To_Same_Value()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.NamedRecordIndexAddress = 3;
            collectionFileRoot.Clean();
            collectionFileRoot.NamedRecordIndexAddress = 3;
            Assert.IsFalse(collectionFileRoot.IsDirty);
        }


        [Test]
        public void Is_Dirty_When_NamedRecordIndexAddress_Changed()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.NamedRecordIndexAddress = 2;
            collectionFileRoot.Clean();
            collectionFileRoot.NamedRecordIndexAddress = 3;
            Assert.IsTrue(collectionFileRoot.IsDirty);
        }

        [Test]
        public void Set_Collection_Root_Makes_Dirty_If_New()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.Clean();
            collectionFileRoot.SetCollectionRootAddress("collection", 123);
            Assert.IsTrue(collectionFileRoot.IsDirty);
        }

        [Test]
        public void Set_Collection_Root_Makes_Dirty_If_Changed()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.Clean();
            collectionFileRoot.SetCollectionRootAddress("collection", 123);
            collectionFileRoot.SetCollectionRootAddress("collection", 456);
            Assert.IsTrue(collectionFileRoot.IsDirty);
        }

        [Test]
        public void Set_Collection_Root_Stays_Clean_If_Not_Changed()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.SetCollectionRootAddress("collection", 123);
            collectionFileRoot.Clean();
            collectionFileRoot.SetCollectionRootAddress("collection", 123);
            Assert.IsFalse(collectionFileRoot.IsDirty);
        }

        [Test]
        public void Clean_Resets_IsDirty()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.Clean();
            Assert.IsFalse(collectionFileRoot.IsDirty);
        }

        [Test]
        public void Serializes_Ok()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.SetCollectionRootAddress("123", 123);
            collectionFileRoot.SetCollectionRootAddress("456", 456);

            var serializer = new SerializerResolver().SerializerFor<CollectionFileRoot>();
            var deserialized =   serializer.Deserialize(serializer.Serialize(collectionFileRoot));

            Assert.AreEqual(123, deserialized.CollectionRootAddress("123"));
            Assert.AreEqual(456, deserialized.CollectionRootAddress("456"));
        }
    }
}

