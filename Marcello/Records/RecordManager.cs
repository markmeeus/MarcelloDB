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

        Record AppendRecord(byte[] data);

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
            return ReadEntireRecord(address);
        }
        public Record AppendRecord(byte[] data)
        {
            var record = new Record();
            record.Header.DataSize = data.Length;
            record.Header.AllocatedDataSize = AllocationStrategy.CalculateSize(record);
            record.Data = new byte[record.Header.AllocatedDataSize];
            data.CopyTo(record.Data, 0);

            WithMetaDataRecord((metaDataRecord) => {
                ReuseEmptyRecordHeader(record, metaDataRecord);
                AppendRecordToList(record, metaDataRecord.DataListEndPoints); 
            });
            return record;
        }

        public Record UpdateRecord(Record record, byte[] data)
        {
            if (data.Length >= record.Header.AllocatedDataSize )
            {
                ReleaseRecord(record);
                return AppendRecord(data); 
            }
            else 
            {   
                record.Data = data;
                record.Header.DataSize = data.Length;
                StorageEngine.Write(record.Header.Address, record.AsBytes());
                return record;
            }
        }

        public void ReleaseRecord(Record record)
        {                  
            WithMetaDataRecord ((metaDataRecord) => {
                //remove from list
                RemoveRecord(record, metaDataRecord.DataListEndPoints); 
                //append to empty list
                AppendRecordToList(record, metaDataRecord.EmptyListEndPoints);

                metaDataRecord.Sanitize();
            });                
        }

        public void RegisterNamedRecordAddress(string name, Int64 recordAddress)
        {       
            WithMetaDataRecord((metaDataRecord) =>
                {
                    var namedRecordIndexRecord = GetNamedRecordIndexRecord(metaDataRecord);
                    var namedRecordIndex = NamedRecordsIndex.FromBytes(namedRecordIndexRecord.Data);
                    namedRecordIndex.NamedRecordIndexes.Add(name, recordAddress);
                    UpdateRecord(namedRecordIndexRecord, namedRecordIndex.ToBytes());
                });
        }   

        NamedRecordsIndex GetNamedRecordIndex(CollectionMetaDataRecord metaDataRecord){
            return NamedRecordsIndex.FromBytes(GetNamedRecordIndexRecord(metaDataRecord).Data);
        }

        Record GetNamedRecordIndexRecord(CollectionMetaDataRecord metaDataRecord){

            if (metaDataRecord.NamedRecordIndexAddress > 0)
            {
                return GetRecord(metaDataRecord.NamedRecordIndexAddress);
            }
            else
            {
                var namedRecordIndex = new NamedRecordsIndex();
                var namedRecordIndexRecord = AppendRecord(namedRecordIndex.ToBytes());
                metaDataRecord.NamedRecordIndexAddress = namedRecordIndexRecord.Header.Address;
                return namedRecordIndexRecord;
            }

        }
        public Int64 GetNamedRecordAddress(string name)
        {
            var metaDataRecord = GetMetaDataRecord();
            return GetNamedRecordIndex(metaDataRecord).NamedRecordIndexes[name];
        }
        #endregion

        #region internal methods

        /// <summary>
        /// LEAKY ABSTRACTION, FIX AFTER INDEX ENUMERATION
        /// </summary>
        /// <returns>The first record.</returns>
        internal Record GetFirstRecord()
        {
            var firstRecordAddress = GetMetaDataRecord().DataListEndPoints.StartAddress;
            if (firstRecordAddress > 0) {
                return ReadEntireRecord(firstRecordAddress);
            }
            return null;
        }

        /// <summary>
        /// LEAKY ABSTRACTION, FIX AFTER INDEX ENUMERATION
        /// </summary>
        /// <returns>The first record.</returns>
        internal Record GetNextRecord(Record record)
        {
            if (record.Header.Next > 0) {
                return ReadEntireRecord(record.Header.Next);
            }
            return null;
        }            
            
        internal void DisableJournal()
        {
            JournalEnabled = false;
            StorageEngine.DisableJournal();
        }
        #endregion 

        #region private methods

        void WriteHeader(Record record)
        {
            StorageEngine.Write(record.Header.Address, record.Header.AsBytes());
        }            

        Record ReadEntireRecord(Int64 address)
        {
            var header = RecordHeader.FromBytes(address, StorageEngine.Read(address, RecordHeader.ByteSize));
            var allBytes = StorageEngine.Read(address, header.AllocatedDataSize);
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

        void AppendRecordToList (Record record, ListEndPoints listEndPoints)
        {
            var operation = new RecordListAppendOperation(
                listEndPoints, 
                CollectionMetaDataRecord.ByteSize, address =>  {
                return ReadEntireRecord (address);
            });
            operation.Record = record;
            operation.Apply ();
            foreach (var touchedRecord in operation.TouchedRecords) {
                WriteHeader (touchedRecord);
            }

            StorageEngine.Write (record.Header.Address, record.AsBytes ());
        }

        private void ReuseEmptyRecordHeader(Record record, CollectionMetaDataRecord metaDataRecord)
        {        
            if (metaDataRecord.EmptyListEndPoints.StartAddress > 0) 
            {
                var emptyRecord = ReadEntireRecord(metaDataRecord.EmptyListEndPoints.StartAddress);
                while(emptyRecord != null)
                {
                    if(emptyRecord.Header.AllocatedDataSize > record.Header.DataSize)
                    {
                        //copy header
                        record.Header = emptyRecord.Header;
                        RemoveRecord(emptyRecord, metaDataRecord.EmptyListEndPoints);
                        SaveMetaDataRecord(metaDataRecord);
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
                CollectionMetaDataRecord.ByteSize, 
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

        void WithMetaDataRecord(Action<CollectionMetaDataRecord> action)
        {
            var metaDataRecord = GetMetaDataRecord();
            action(metaDataRecord);
            SaveMetaDataRecord (metaDataRecord);
        }


        #endregion
    }
}

