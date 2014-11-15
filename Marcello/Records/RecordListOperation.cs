using System;
using System.Collections.Generic;


namespace Marcello.Records
{
    internal abstract class RecordListOperation
    {
        List<Record> _touchedRecords;

        internal ListEndPoints ListEndPoints { get; set; }

        internal Int64 InitialAddress { get; set; }

        internal Func<Int64, Record> LoadRecord { get; set; } 

        internal Record Record { get; set; }

        internal IEnumerable<Record> TouchedRecords { get { return _touchedRecords; }}

        internal RecordListOperation(ListEndPoints listEndPoints, 
            Int64 initialAddress, 
            Func<Int64, Record> LoadRecordFunc)
        {
            this.ListEndPoints = listEndPoints;
            this.InitialAddress = initialAddress;
            this.LoadRecord = LoadRecordFunc;
            _touchedRecords = new List<Record>();
        }

        internal abstract void Apply();

        protected Record Touch(Record record)
        {
            if (!_touchedRecords.Contains (record)) 
            {
                _touchedRecords.Add (record);
            }
            return record;
        }

        protected Record FetchRecord(Int64 address)
        {
            if (address > 0) 
            {
                return LoadRecord (address);
            }
            return null;
        }
    }

    internal class RecordListAppendOperation : RecordListOperation
    {
        internal RecordListAppendOperation (ListEndPoints listEndPoints, 
                                           Int64 initialAddress, 
                                           Func<Int64, Record> loadRecordFunc)
            : base (listEndPoints, initialAddress, loadRecordFunc){}


        internal override void Apply ()
        {
            var firstRecord = FetchRecord(ListEndPoints.StartAddress);
            if (firstRecord == null) {
                if (this.Record.Header.Address == 0) 
                {
                    Touch (this.Record).Header.Address = this.InitialAddress;
                }
                this.ListEndPoints.StartAddress = this.Record.Header.Address;
            }
            else 
            {
                //get the last record and link the new one in
                var lastRecord = FetchRecord(ListEndPoints.EndAddress);
                if (this.Record.Header.Address == 0) 
                {
                    Touch(this.Record).Header.Address = lastRecord.Header.Address + lastRecord.Header.AllocatedSize;
                }
                Touch(this.Record).Header.Previous = lastRecord.Header.Address;
                Touch(lastRecord).Header.Next = this.Record.Header.Address;
            }

            this.ListEndPoints.EndAddress = this.Record.Header.Address;
        }
    }

    internal class RecordListReleaseOperation : RecordListOperation
    {
        internal RecordListReleaseOperation(ListEndPoints listEndPoints, 
            Int64 initialAddress, 
            Func<Int64, Record> loadRecordFunc)
            :base(listEndPoints, initialAddress, loadRecordFunc)
        {

        }

        internal override void Apply ()
        {
            var previousRecord = FetchRecord(this.Record.Header.Previous);
            var nextRecord = FetchRecord (this.Record.Header.Next);

            if (previousRecord == null && nextRecord == null) //only record 
            {
                this.ListEndPoints.StartAddress = 0;
                this.ListEndPoints.EndAddress = 0;
            } 

            if(previousRecord == null && nextRecord != null) //first record            
            {
                this.ListEndPoints.StartAddress = nextRecord.Header.Address;
                Touch(nextRecord).Header.Previous = 0; 
            }

            if (nextRecord == null && previousRecord != null) //last record
            { 
                this.ListEndPoints.EndAddress = previousRecord.Header.Address;
                Touch(previousRecord).Header.Next = 0;
            }
            if (nextRecord != null && previousRecord != null) //somewhere in the middle
            {
                Touch(previousRecord).Header.Next = nextRecord.Header.Address;
                Touch(nextRecord).Header.Previous = previousRecord.Header.Address;
            }
        }
    }       
}

