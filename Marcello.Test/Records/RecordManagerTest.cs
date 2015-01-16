using System;
using NUnit.Framework;
using Marcello.Records;
using Marcello.Test.Classes;
using Marcello.Storage;
using Marcello.AllocationStrategies;

namespace Marcello.Test.Records
{
    [TestFixture]
    public class RecordManagerTest
    {
        InMemoryStreamProvider _streamProvider;
        RecordManager<Article> _recordManager;
        Marcello _marcello;
        [SetUp]
        public void Initialze()
        {
            _streamProvider = new InMemoryStreamProvider();
            _marcello = new Marcello(_streamProvider);

            _recordManager = new RecordManager<Article>(
                new DoubleSizeAllocationStrategy(),
                new StorageEngine<Article>(_marcello));
            _recordManager.DisableJournal();
        }

        [Test]
        public void AppendRecordReturnsRecord(){
            var record = _recordManager.AppendRecord(new byte[0]);
            Assert.NotNull(record);
        }

        [Test]
        public void GetAppendedRecord(){
            var record = _recordManager.AppendRecord(new byte[3]{ 1, 2, 3 });
            var readRecord = _recordManager.GetRecord(record.Header.Address);
            Assert.AreEqual(new byte[3]{ 1, 2, 3 }, readRecord.Data);
        }

        [Test]
        public void UpdateRecordReturnsUpdatedRecord(){
            var record = _recordManager.AppendRecord(new byte[3] { 1, 2, 3 });
            record = _recordManager.UpdateRecord(record, new byte[3]{ 4, 5, 6 });
            Assert.AreEqual(new byte[3]{ 4, 5, 6}, record.Data);
        }

        [Test]
        public void AppendRecordAssignsAddress()
        {
            var record = _recordManager.AppendRecord(new byte[0]);
            Assert.Greater(record.Header.Address, 0);
        }

        [Test]
        public void AppendRecordUpdatesRecord(){
            var record = _recordManager.AppendRecord(new byte[3]{ 1, 2, 3 });
            _recordManager.UpdateRecord(record, new byte[3] { 4, 5, 6 });
            var readRecord = _recordManager.GetRecord(record.Header.Address);
            Assert.AreEqual(new byte[3]{ 4, 5, 6}, readRecord.Data);
        }

        [Test] 
        public void UpdateRecordDoesntIncreaeDataSize(){
            var record = _recordManager.AppendRecord(new byte[3]{ 1, 2, 3 });
            var streamLength =  GetStreamLength();
            _recordManager.UpdateRecord(record, new byte[3] { 4, 5, 6 });
            var newStreamLength =  GetStreamLength();
            Assert.AreEqual(streamLength, newStreamLength);           
        }            

        [Test]
        public void AppendAfterReleaseReusesStorage(){
            var record = _recordManager.AppendRecord(new byte[3]{ 1, 2, 3 });
            _recordManager.ReleaseRecord(record);
            var streamLength = GetStreamLength();
            _recordManager.AppendRecord(new byte[]{ 4, 5, 6});
            var newStreamLength = GetStreamLength();
            Assert.AreEqual(streamLength, newStreamLength);           
        }


        [Test]
        public void GetNamedRecordReturnsNulWhenNotRegistered(){
            var result = _recordManager.GetNamedRecordAddress("Test");
            Assert.AreEqual(0, result, "Should be nul");
        }

        [Test]
        public void StoresNamedRecordAddress(){
            _recordManager.RegisterNamedRecordAddress("Test", 123);
            Assert.AreEqual(123, _recordManager.GetNamedRecordAddress("Test"));
        }

        [Test]
        public void StoreMultipleNamedRecordAddress(){
            _recordManager.RegisterNamedRecordAddress("Test1", 123);
            _recordManager.RegisterNamedRecordAddress("Test2", 456);
            Assert.AreEqual(123, _recordManager.GetNamedRecordAddress("Test1"));
            Assert.AreEqual(456, _recordManager.GetNamedRecordAddress("Test2"));
        }

        Int64 GetStreamLength(){
            return ((InMemoryStream)_streamProvider.GetStream("Article")).BackingStream.Length;
        }
    }
}

