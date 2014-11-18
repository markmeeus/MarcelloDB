using System;
using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

using Marcello.Records;

namespace Marcello.Test.Records
{
    [TestFixture]
    public class RecordListOperationTest
    {
        [SetUp]
        public void Initialize()
        {

        }

        [Test]
        public void Apend_First_Record_Sets_StartAddress()
        {
            var record = new Record();
            var operation = CreateEmptyRecordListAppendOperation(record);
            operation.Apply();
            Assert.AreEqual(record.Header.Address, operation.ListEndPoints.StartAddress);
        }

        [Test]
        public void Append_First_Record_Sets_EndAddress()
        {
            var record = new Record();
            var operation = CreateEmptyRecordListAppendOperation(record);
            operation.Apply();
            Assert.AreEqual(record.Header.Address, operation.ListEndPoints.EndAddress);
        }            

        [Test]
        public void Append_First_Records_Assigns_Initial_Address()
        {
            var record = new Record();
            var operation = CreateEmptyRecordListAppendOperation(record);
            operation.Apply();
            Assert.AreEqual(operation.InitialAddress, record.Header.Address);
        }            

        [Test]
        public void Append_Record_Item_Doesnt_Override_Address()
        {
            var record = new Record();
            record.Header.Address = 20;
            var operation = CreateEmptyRecordListAppendOperation(record);
            operation.Apply();
            Assert.AreEqual(20, record.Header.Address);
        }

        [Test]
        public void Append_Second_Record_Does_Not_Change_StartAddress()
        {         
            var operation = CreateListAppendOperationWithItems(1, new Record());
            var originalStart = operation.ListEndPoints.StartAddress;
            operation.Apply();
            Assert.AreEqual(originalStart, operation.ListEndPoints.StartAddress); 
        }

        [Test]
        public void Append_Record_Sets_Records_Address()
        {
            var record = new Record();
            var operation = CreateListAppendOperationWithItems(1, record);
            operation.Apply();
            var firstRecord = operation.LoadRecord (operation.ListEndPoints.StartAddress);
            Assert.AreEqual(firstRecord.Header.Address + RecordHeader.ByteSize + firstRecord.Header.AllocatedSize,
                record.Header.Address); 
        }

        [Test]
        public void Append_Record_Sets_Doesnt_Override_Address()
        {
            var record = new Record();
            record.Header.Address = 300;
            var operation = CreateListAppendOperationWithItems(1, record);
            operation.Apply();
            Assert.AreEqual(300, record.Header.Address); 
        }

        [Test]
        public void Append_Second_Record_Sets_EndAddress()
        {
            var record = new Record();
            var operation = CreateListAppendOperationWithItems(1, record);
            operation.Apply();
            Assert.AreEqual(record.Header.Address, operation.ListEndPoints.EndAddress); 
        }

        [Test]
        public void Append_Second_Record_Links_First_Record_To_Second()
        {
            var record = new Record();
            var operation = CreateListAppendOperationWithItems (1, record);
            operation.Apply();
            var firstRecord = operation.LoadRecord(100);
            Assert.AreEqual(record.Header.Address, firstRecord.Header.Next); 
        }

        [Test]
        public void Append_Second_Record_Links_To_First()
        {
            var record = new Record();
            var operation = CreateListAppendOperationWithItems(1, record);
            operation.Apply ();
            var firstRecord = operation.LoadRecord(100);
            Assert.AreEqual(firstRecord.Header.Address, record.Header.Previous); 
        }

        [Test]
        public void Append_First_Record_Marks_As_Touched()
        {
            var record = new Record();
            var operation = CreateListAppendOperationWithItems(1, record);
            operation.Apply();
            Assert.Contains(record, operation.TouchedRecords.ToList());
        }

        [Test]
        public void Append_Second_Record_Marks_First_As_Touched()
        {
            var record = new Record();
            var operation = CreateListAppendOperationWithItems(1, record);
            operation.Apply();
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            Assert.Contains(firstRecord, operation.TouchedRecords.ToList());
        }

        [Test]
        public void Release_Only_Record_Resets_StartAddress()
        {
            var operation = CreateListReleaseOperationWithItems(1);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            operation.Record = firstRecord;
            operation.Apply();
            Assert.AreEqual(0, operation.ListEndPoints.StartAddress);
        }

        [Test]
        public void Release_Only_Record_Resets_EndAddress()
        {
            var operation = CreateListReleaseOperationWithItems(1);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            operation.Record = firstRecord;
            operation.Apply();
            Assert.AreEqual (0, operation.ListEndPoints.EndAddress);
        }
            
        [Test]
        public void Release_First_Record_Sets_StartAddress()
        {
            var operation = CreateListReleaseOperationWithItems(2);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            operation.Record = firstRecord;
            operation.Apply();
            Assert.AreEqual(secondRecord.Header.Address, operation.ListEndPoints.StartAddress);
        }

        [Test]
        public void Release_First_Record_Unlinks_Second_Record()
        {
            var operation = CreateListReleaseOperationWithItems(2);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            operation.Record = firstRecord;
            operation.Apply();
            Assert.AreEqual(0, secondRecord.Header.Previous);
        }

        [Test]
        public void Release_First_Record_Marks_Record_As_Touched()
        {
            var operation = CreateListReleaseOperationWithItems(2);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            operation.Record = firstRecord;
            operation.Apply();
            Assert.Contains(secondRecord, operation.TouchedRecords.ToList());
        }

        [Test]
        public void Release_Last_Record_Sets_End_Address()
        {
            var operation = CreateListReleaseOperationWithItems(2);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            operation.Record = secondRecord;
            operation.Apply();
            Assert.AreEqual(firstRecord.Header.Address, operation.ListEndPoints.EndAddress);
        }

        [Test]
        public void Release_Last_Record_Unlinks_Previous_Record()
        {
            var operation = CreateListReleaseOperationWithItems(2);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            operation.Record = secondRecord;
            operation.Apply();
            Assert.AreEqual(0, firstRecord.Header.Next);
        }

        [Test]
        public void Release_Last_Record_Marks_Previous_As_Touched()
        {
            var operation = CreateListReleaseOperationWithItems(2);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            operation.Record = secondRecord;
            operation.Apply();
            Assert.Contains(firstRecord, operation.TouchedRecords.ToList());
        }

        [Test]
        public void Release_Middle_Record_Relinks_Previous_To_Next()
        {
            var operation = CreateListReleaseOperationWithItems(3);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            var thirdRecord =  operation.LoadRecord(secondRecord.Header.Next);
            operation.Record = secondRecord;
            operation.Apply();
            Assert.AreEqual(thirdRecord.Header.Address, firstRecord.Header.Next);
        }

        [Test]
        public void Release_Middle_Record_Relinks_Next_To_Previous()
        {
            var operation = CreateListReleaseOperationWithItems(3);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            var thirdRecord =  operation.LoadRecord(secondRecord.Header.Next);
            operation.Record = secondRecord;
            operation.Apply();
            Assert.AreEqual(firstRecord.Header.Address, thirdRecord.Header.Previous);
        }

        [Test]
        public void Release_Middle_Record_Marks_Previous_As_Touched()
        {
            var operation = CreateListReleaseOperationWithItems(3);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            operation.Record = secondRecord;
            operation.Apply();
            Assert.Contains(firstRecord, operation.TouchedRecords.ToList());
        }

        [Test]
        public void Release_Middle_Record_Marks_Next_As_Touched()
        {
            var operation = CreateListReleaseOperationWithItems(3);
            var firstRecord = operation.LoadRecord(operation.ListEndPoints.StartAddress);
            var secondRecord = operation.LoadRecord(firstRecord.Header.Next);
            var thirdRecord =  operation.LoadRecord(secondRecord.Header.Next);
            operation.Record = secondRecord;
            operation.Apply();
            Assert.Contains(thirdRecord, operation.TouchedRecords.ToList());
        }

        #region private methods
        RecordListAppendOperation CreateEmptyRecordListAppendOperation(Record record)
        {
            var operation =  new RecordListAppendOperation (
                new ListEndPoints (0, 0), 
                10, 
                (address) => {
                    return null;
                }
            );

            operation.Record = record;

            return operation;
        }

        RecordListAppendOperation CreateListAppendOperationWithItems(int number, Record record)
        {
            var records = BuildRecords(number);

            var operation =  new RecordListAppendOperation(
                new ListEndPoints(100, number * 100), 
                10,
                (address) => {
                    return records[address];
                });

            operation.Record = record;

            return operation;
        }

        RecordListReleaseOperation CreateListReleaseOperationWithItems(int number)
        {
            var records = BuildRecords (number);

            return new RecordListReleaseOperation(
                new ListEndPoints(100, number * 100), 
                10,
                (address) => {
                    return records[address];
                });
        }
            
        static Dictionary<long, Record> BuildRecords (int number)
        {
            var records = new Dictionary<Int64, Record> ();
            for (int i = 1; i <= number; i++) {
                var existingRecord = new Record ();
                existingRecord.Header.Address = i * 100;
                if (i > 0) {
                    existingRecord.Header.Previous = (i - 1) * 100;
                }
                if (i < number) {
                    existingRecord.Header.Next = (i + 1) * 100;
                }
                existingRecord.Header.AllocatedSize = 100;
                existingRecord.Header.DataSize = 20;
                records.Add (existingRecord.Header.Address, existingRecord);
            }
            return records;
        }
        #endregion
    }
}

