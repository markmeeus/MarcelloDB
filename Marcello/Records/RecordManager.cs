using System;
using Marcello.AllocationStrategies;
using Marcello.Serialization;
using Marcello.Storage;
using Marcello.Records.__;

namespace Marcello.Records
{
    internal interface IRecordManager
    {
        Record GetRecord(Int64 address);

        Record AppendRecord(byte[] data, bool hasObject = false);

        Record UpdateRecord(Record record, byte[] data);

        void ReleaseRecord(Record record);

        void RegisterNamedRecordAddress(string name, Int64 recordAddress);

        Int64 GetNamedRecordAddress(string name);
    }

    internal class RecordManager<T> : IRecordManager
    {   
        StorageEngine<T> StorageEngine { get;set; }

        IAllocationStrategy AllocationStrategy { get; set; }

        bool JournalEnabled { get; set; }

        CollectionRoot CollectionRoot{ get; set; }

        internal RecordManager(
            IAllocationStrategy allocationStrategy,
            StorageEngine<T> storageEngine
        )
        {
            StorageEngine = storageEngine;
            AllocationStrategy = allocationStrategy;
            JournalEnabled = true; //journal by default
        }

        #region IRecordManager implementation
        public Record GetRecord(Int64 address){

            Record record = null;

            WithCollectionRoot(() =>
                {
                    record = ReadEntireRecord(address);
                });

            return record;
        }

        public Record AppendRecord(byte[] data, bool hasObject = false)
        {
            var record = new Record();

            WithCollectionRoot(() =>
                {
                    record.Header.DataSize = data.Length;
                    record.Header.AllocatedDataSize = AllocationStrategy.CalculateSize(record);
                    record.Header.HasObject = hasObject;
                    record.Data = new byte[record.Header.DataSize];

                    data.CopyTo(record.Data, 0);

                    ReuseEmptyRecordHeader(record);
                    AppendRecordToList(record, this.CollectionRoot.DataListEndPoints);                                       
                });

            return record;
        }

        public Record UpdateRecord(Record record, byte[] data)
        {
            Record result = null;

            WithCollectionRoot(() =>
                {
                    if (data.Length >= record.Header.AllocatedDataSize )
                    {
                        ReleaseRecord(record);
                        result = AppendRecord(data, record.Header.HasObject); 
                    }
                    else 
                    {   
                        record.Data = data;
                        record.Header.DataSize = data.Length;
                        StorageEngine.Write(record.Header.Address, record.AsBytes());
                        result = record;
                    }

                });

            return result;
        }

        public void ReleaseRecord(Record record)
        {    
            WithCollectionRoot(() =>
                {
                    //remove from list
                    RemoveRecord(record, this.CollectionRoot.DataListEndPoints); 
                    //append to empty list
                    AppendRecordToList(record, this.CollectionRoot.EmptyListEndPoints);

                    this.CollectionRoot.Sanitize();
                });
        }

        public void RegisterNamedRecordAddress(string name, Int64 recordAddress)
        {       
            WithCollectionRoot(() =>
                {
                    var namedRecordIndexRecord = GetNamedRecordIndexRecord();

                    var namedRecordIndex = NamedRecordsIndex.FromBytes(namedRecordIndexRecord.Data);

                    namedRecordIndex.NamedRecordIndexes.Remove(name);
                    namedRecordIndex.NamedRecordIndexes.Add(name, recordAddress);

                    var updateRecord = UpdateRecord(namedRecordIndexRecord, namedRecordIndex.ToBytes());

                    this.CollectionRoot.NamedRecordIndexAddress = updateRecord.Header.Address;
                });
        }   
                   
        public Int64 GetNamedRecordAddress(string name)
        {
            Int64 result = 0;
            WithCollectionRoot(() => 
                {
                    var namedRecordIndex = GetNamedRecordIndex();
                    if (namedRecordIndex.NamedRecordIndexes.ContainsKey(name))
                    {
                        result = namedRecordIndex.NamedRecordIndexes[name];
                    }
                });
            return result;
        }
        #endregion //IRecordManager implementation               
            
        internal void DisableJournal()
        {
            JournalEnabled = false;
            StorageEngine.DisableJournal();
        }
            
        void WriteHeader(Record record)
        {
            StorageEngine.Write(record.Header.Address, record.Header.AsBytes());
        }            

        Record ReadEntireRecord(Int64 address)
        {
            var header = RecordHeader.FromBytes(address, StorageEngine.Read(address, RecordHeader.ByteSize));
            var allBytes = StorageEngine.Read(address, RecordHeader.ByteSize + header.AllocatedDataSize);
            var record =  Record.FromBytes(address, allBytes);
            return record;
        }
            
        void WithCollectionRoot(Action action)
        {
            LoadCollectionRoot();
            action();
            SaveCollectionRoot();
        }

        void LoadCollectionRoot()
        {        
            var bytes = StorageEngine.Read(0, CollectionRoot.ByteSize);
            this.CollectionRoot = CollectionRoot.FromBytes(bytes);
        }

        void SaveCollectionRoot()
        {
            StorageEngine.Write(0, this.CollectionRoot.AsBytes());
        }

        void AppendRecordToList (Record record, ListEndPoints listEndPoints)
        {
            var operation = new RecordListAppendOperation(
                listEndPoints, 
                CollectionRoot.ByteSize, address =>  {
                return ReadEntireRecord (address);
            });
            operation.Record = record;
            operation.Apply ();
            foreach (var touchedRecord in operation.TouchedRecords) {
                WriteHeader (touchedRecord);
            }

            StorageEngine.Write (record.Header.Address, record.AsBytes ());
        }

        private void ReuseEmptyRecordHeader(Record record)
        {        
            return;
            if (CollectionRoot.EmptyListEndPoints.StartAddress > 0) 
            {
                var emptyRecord = ReadEntireRecord(CollectionRoot.EmptyListEndPoints.StartAddress);
                while(emptyRecord != null)
                {
                    if(emptyRecord.Header.AllocatedDataSize > record.Header.DataSize)
                    {
                        //copy header
                        record.Header = emptyRecord.Header;
                        RemoveRecord(emptyRecord, CollectionRoot.EmptyListEndPoints);
                        return;
                    }
                    if (emptyRecord.Header.Next > 0) 
                    {
                        emptyRecord = ReadEntireRecord(emptyRecord.Header.Next);
                    } 
                    else 
                    {
                        emptyRecord = null;
                    }
                }
            }
        }

        void RemoveRecord (Record record, ListEndPoints listEndPoints)
        {         
            var operation = new RecordListReleaseOperation (
                listEndPoints, 
                CollectionRoot.ByteSize, 
                address =>  {
                    return ReadEntireRecord (address);
                }
            );
            operation.Record = record;
            operation.Apply ();
            foreach (var touchedRecord in operation.TouchedRecords) {
                WriteHeader (touchedRecord);
            }
        }            

        NamedRecordsIndex GetNamedRecordIndex()
        {
            return NamedRecordsIndex.FromBytes(GetNamedRecordIndexRecord().Data);
        }

        Record GetNamedRecordIndexRecord()
        {
        
            if (this.CollectionRoot.NamedRecordIndexAddress > 0)
            {
                return GetRecord(this.CollectionRoot.NamedRecordIndexAddress);
            }
            else
            {
                var namedRecordIndex = new NamedRecordsIndex();
                var namedRecordIndexRecord = AppendRecord(namedRecordIndex.ToBytes());

                this.CollectionRoot.NamedRecordIndexAddress = namedRecordIndexRecord.Header.Address;

                return namedRecordIndexRecord;
            }
        }
    }
}

