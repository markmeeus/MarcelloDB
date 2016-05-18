using System;
using NUnit.Framework;
using MarcelloDB.Records;

namespace MarcelloDB.Test.Records
{
    [TestFixture]
    public class IndexMetaRecordTest
    {
        [Test]
        public void ChangeRootNodeAddress_Makes_Dirty()
        {
            var record = new IndexMetaRecord();
            record.RootNodeAddress = 1;
            Assert.IsTrue(record.IsDirty);
        }

        [Test]
        public void SetRootNodeAddress_ToSameValue_KeepsItClean()
        {
            var record = new IndexMetaRecord();
            record.RootNodeAddress = 1;
            record.Clean();
            record.RootNodeAddress = 1;
            Assert.IsFalse(record.IsDirty);
        }

        [Test]
        public void ChangeNumberOfNodes_Makes_Dirty()
        {
            var record = new IndexMetaRecord();
            record.NumberOfNodes = 1;
            Assert.IsTrue(record.IsDirty);
        }

        [Test]
        public void SetNumberOfNodes_ToSameValue_KeepsItClean()
        {
            var record = new IndexMetaRecord();
            record.NumberOfNodes = 1;
            record.Clean();
            record.NumberOfNodes = 1;
            Assert.IsFalse(record.IsDirty);
        }


        [Test]
        public void ChangeNumberOfEntries_Makes_Dirty()
        {
            var record = new IndexMetaRecord();
            record.NumberOfEntries = 1;
            Assert.IsTrue(record.IsDirty);
        }

        [Test]
        public void SetNumberOfEntriesToSameValue_KeepsItClean()
        {
            var record = new IndexMetaRecord();
            record.NumberOfEntries = 1;
            record.Clean();
            record.NumberOfEntries = 1;
            Assert.IsFalse(record.IsDirty);
        }


        [Test]
        public void ChangeTotalAllocatedSize_Makes_Dirty()
        {
            var record = new IndexMetaRecord();
            record.TotalAllocatedSize = 1;
            Assert.IsTrue(record.IsDirty);
        }

        [Test]
        public void SetTotalAllocatedSize_ToSameValue_KeepsItClean()
        {
            var record = new IndexMetaRecord();
            record.TotalAllocatedSize = 1;
            record.Clean();
            record.TotalAllocatedSize = 1;
            Assert.IsFalse(record.IsDirty);
        }


        [Test]
        public void ChangeTotalAllocatedDataSize_Makes_Dirty()
        {
            var record = new IndexMetaRecord();
            record.TotalAllocatedDataSize = 1;
            Assert.IsTrue(record.IsDirty);
        }

        [Test]
        public void SetTotalAllocatedDataSize_ToSameValue_KeepsItClean()
        {
            var record = new IndexMetaRecord();
            record.TotalAllocatedDataSize = 1;
            record.Clean();
            record.TotalAllocatedDataSize = 1;
            Assert.IsFalse(record.IsDirty);
        }


        [Test]
        public void Clean_Makes_It_Clean()
        {
            var record = new IndexMetaRecord();
            record.TotalAllocatedDataSize = 1;
            record.Clean();
            Assert.IsFalse(record.IsDirty);
        }

    }
}