using System;
using NUnit.Framework;
using Marcello.Records;
using Marcello.Test.Classes;

namespace Marcello.Test.Records
{
    [TestFixture]
    public class DirectRecordAccessorTest
    {
        Marcello _session;

        [SetUp]
        public void Initialize(){
            _session = new Marcello(new InMemoryStreamProvider());

        }

        [Test]
        public void Reads_Data_At_Begin(){
            var record = new RecordBuilder().Build(new byte[8]{1,2,3,4,5,6,7,8});
            _session.Collection<Article>().RecordManager.AppendRecord(record);
            var accessor = new DirectRecordAccessor(
                record.Header.Address, 
                _session.Collection<Article>().RecordManager
            );
            Assert.AreEqual(new byte[]{1,2,3,4}, accessor.Read(0, 4));
        }

        [Test]
        public void Reads_Data_At_End(){
            var record = new RecordBuilder().Build(new byte[8]{1,2,3,4,5,6,7,8});
            _session.Collection<Article>().RecordManager.AppendRecord(record);
            var accessor = new DirectRecordAccessor(
                record.Header.Address, 
                _session.Collection<Article>().RecordManager
            );
            Assert.AreEqual(new byte[]{5,6,7,8}, accessor.Read(4, 4));
        }

        [Test]
        public void Reads_Data_In_The_Middle(){
            var record = new RecordBuilder().Build(new byte[8]{1,2,3,4,5,6,7,8});
            _session.Collection<Article>().RecordManager.AppendRecord(record);
            var accessor = new DirectRecordAccessor(
                record.Header.Address, 
                _session.Collection<Article>().RecordManager
            );
            Assert.AreEqual(new byte[]{3,4,5,6}, accessor.Read(2, 4));
        }

        [Test]
        public void Throws_When_Reading_Index_Out_Of_Range(){
            var record = new RecordBuilder().Build(new byte[8]{1,2,3,4,5,6,7,8});
            _session.Collection<Article>().RecordManager.AppendRecord(record);
            var accessor = new DirectRecordAccessor(
                record.Header.Address, 
                _session.Collection<Article>().RecordManager
            );

            Assert.Throws<IndexOutOfRangeException>(() => accessor.Read(10, 1));
        }

        [Test]
        public void Throws_When_Reading_Index_And_Length_Out_Of_Range(){
            var record = new RecordBuilder().Build(new byte[8]{1,2,3,4,5,6,7,8});
            _session.Collection<Article>().RecordManager.AppendRecord(record);
            var accessor = new DirectRecordAccessor(
                record.Header.Address, 
                _session.Collection<Article>().RecordManager
            );

            Assert.Throws<IndexOutOfRangeException>(() => accessor.Read(4, 5));
        }            

        [Test]
        public void Writes_Data_At_Begin(){
            var record = new RecordBuilder().Build(new byte[8]{1,2,3,4,5,6,7,8});
            _session.Collection<Article>().RecordManager.AppendRecord(record);
            var accessor = new DirectRecordAccessor(
                record.Header.Address, 
                _session.Collection<Article>().RecordManager
            );
            accessor.Write(0, new byte[]{ 9, 8, 7, 6 });

            Assert.AreEqual(new byte[]{9,8,7,6}, 
                _session.Collection<Article>().StorageEngine.Read(record.Header.Address + RecordHeader.ByteSize, 4)
            );
        }

        [Test]
        public void Writes_Data_At_End(){
            var record = new RecordBuilder().Build(new byte[8]{1,2,3,4,5,6,7,8});
            _session.Collection<Article>().RecordManager.AppendRecord(record);
            var accessor = new DirectRecordAccessor(
                record.Header.Address, 
                _session.Collection<Article>().RecordManager
            );
            accessor.Write(4, new byte[]{ 9, 8, 7, 6 });

            Assert.AreEqual(new byte[]{9,8,7,6}, 
                _session.Collection<Article>().StorageEngine.Read(record.Header.Address + RecordHeader.ByteSize + 4, 4)
            );
        }

        [Test]
        public void Throws_When_Writing_Out_Of_Range(){
            var record = new RecordBuilder().Build(new byte[8]{1,2,3,4,5,6,7,8});
            _session.Collection<Article>().RecordManager.AppendRecord(record);
            var accessor = new DirectRecordAccessor(
                record.Header.Address, 
                _session.Collection<Article>().RecordManager
            );
            Assert.Throws<IndexOutOfRangeException>(()=>accessor.Write(5, new byte[]{ 9, 8, 7, 6 }));
        }
    }
}

