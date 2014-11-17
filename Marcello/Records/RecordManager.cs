using System;
using Marcello.AllocationStrategies;
using Marcello.Serialization;
using Marcello.Storage;

namespace Marcello.Records
{
    internal class RecordManager<T>
    {   
        Marcello Session { get; set; }

        StorageEngine<T> StorageEngine { get;set; }

        IObjectSerializer<T> Serializer { get; set; }

        IAllocationStrategy AllocationStrategy { get; set; }

        bool JournalEnabled { get; set; }

        internal RecordManager(Marcello session,
            IObjectSerializer<T> serializer,
            IAllocationStrategy allocationStrategy
        )
        {
            Session = session;
            StorageEngine = new StorageEngine<T>(Session);
            Serializer = serializer;
            AllocationStrategy = allocationStrategy;
            JournalEnabled = true; //journal by default
        }

        internal Record GetFirstRecord()
        {
            var firstRecordAddress = GetMetaDataRecord().DataListEndPoints.StartAddress;
            if (firstRecordAddress > 0) {
                return ReadEntireRecord(firstRecordAddress);
            }
            return null;
        }

        internal Record GetNextRecord(Record record)
        {
            if (record.Header.Next > 0) {
                return ReadEntireRecord(record.Header.Next);
            }
            return null;
        }


        #region internal methods
        internal void DisableJournal()
        {
            JournalEnabled = false;
            StorageEngine.DisableJournal();
        }
        #endregion 

        #region private methods
        public void AppendObject(T obj)
        {
            var record = new Record();
            var data = Serializer.Serialize(obj);
            record.Header.DataSize = data.Length;
            record.Header.AllocatedSize = AllocationStrategy.CalculateSize(record);
            record.Data = new byte[record.Header.AllocatedSize];
            data.CopyTo(record.Data, 0);

            var metaDataRecord = this.GetMetaDataRecord (); 
            var operation = new RecordListAppendOperation (
                metaDataRecord.DataListEndPoints,
                CollectionMetaDataRecord.ByteSize,
                (address) => {
                    return ReadEntireRecord(address);
                });
            operation.Record = record;
            operation.Apply();

            foreach(var touchedRecord in operation.TouchedRecords){
                WriteHeader(touchedRecord);
            };

            StorageEngine.Write(record.Header.Address, record.AsBytes());
            SaveMetaDataRecord(metaDataRecord);
        }

        public void UpdateObject(Record record, T obj)
        {
            var bytes = Serializer.Serialize(obj);
            if (bytes.Length >= record.Header.AllocatedSize )
            {
                ReleaseRecord(record);
                AppendObject(obj); 
            }
            else 
            {   
                record.Data = new byte[record.Header.AllocatedSize];
                bytes.CopyTo(record.Data, 0);
                record.Header.DataSize = bytes.Length;
                StorageEngine.Write(record.Header.Address, record.AsBytes ());
            }
        }

        public void ReleaseRecord(Record record)
        {                  
            var metaDataRecord = this.GetMetaDataRecord (); 
            var operation = new RecordListReleaseOperation (
                metaDataRecord.DataListEndPoints,
                CollectionMetaDataRecord.ByteSize,
                (address) => {
                    return ReadEntireRecord(address);
                });
            operation.Record = record;
            operation.Apply();

            foreach(var touchedRecord in operation.TouchedRecords){
                WriteHeader(touchedRecord);
            };

            StorageEngine.Write(record.Header.Address, record.AsBytes());
            SaveMetaDataRecord(metaDataRecord);
        }
            
        void WriteHeader(Record record)
        {
            StorageEngine.Write(record.Header.Address, record.Header.AsBytes());
        }

        Record ReadEntireRecord(Int64 address)
        {
            var header = RecordHeader.FromBytes(address, StorageEngine.Read(address, RecordHeader.ByteSize));
            var allBytes = StorageEngine.Read(address, header.AllocatedSize);
            var record =  Record.FromBytes(address, allBytes);
            return record;
        }

        CollectionMetaDataRecord GetMetaDataRecord()
        {
            var bytes = StorageEngine.Read(0, CollectionMetaDataRecord.ByteSize);
            return CollectionMetaDataRecord.FromBytes(bytes);
        }

        void SaveMetaDataRecord(CollectionMetaDataRecord record)
        {
            StorageEngine.Write(0, record.AsBytes());
        }
        #endregion
    }
}

