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
        public void Set_Head_To_Different_Value_Makes_It_Dirty()
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
        public void Clean_Resets_IsDirty()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.Clean();
            Assert.IsFalse(collectionFileRoot.IsDirty);
        }

        [Test]
        public void Sets_FormatVersion()
        {
            Assert.AreEqual(2, CollectionFileRoot.Create().FormatVersion);
        }

        [Test]
        public void Throws_When_FormatVersion_Conflicts()
        {
            var collectionFileRoot = CollectionFileRoot.Create();
            collectionFileRoot.FormatVersion = 0;

            Assert.Throws<InvalidOperationException>(() =>
                {
                    collectionFileRoot.Validate();
                });
        }
    }
}

