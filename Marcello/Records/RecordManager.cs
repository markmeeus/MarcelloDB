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

        CollectionMetaDataRecord MetaDataRecord{ get; set; }

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

            WithMetaDataRecord(() =>
                {
                    record = ReadEntireRecord(address);
                });

            return record;
        }

        public Record AppendRecord(byte[] data, bool hasObject = false)
        {
            var record = new Record();

            WithMetaDataRecord(() =>
                {
                    record.Header.DataSize = data.Length;
                    record.Header.AllocatedDataSize = AllocationStrategy.CalculateSize(record);
                    record.Header.HasObject = hasObject;
                    record.Data = new byte[record.Header.DataSize];
                    data.CopyTo(record.Data, 0);


                    ReuseEmptyRecordHeader(record, this.MetaDataRecord);
                    AppendRecordToList(record, this.MetaDataRecord.DataListEndPoints); 
                    SaveMetaDataRecord();
                });

            return record;
        }

        public Record UpdateRecord(Record record, byte[] data)
        {
            Record result = null;

            WithMetaDataRecord(() =>
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
            WithMetaDataRecord(() =>
                {
                    //remove from list
                    RemoveRecord(record, this.MetaDataRecord.DataListEndPoints); 
                    //append to empty list
                    AppendRecordToList(record, this.MetaDataRecord.EmptyListEndPoints);

                    this.MetaDataRecord.Sanitize();
                });
        }

        public void RegisterNamedRecordAddress(string name, Int64 recordAddress)
        {       
            WithMetaDataRecord(() =>
                {
                    var namedRecordIndexRecord = GetNamedRecordIndexRecord();

                    var namedRecordIndex = NamedRecordsIndex.FromBytes(namedRecordIndexRecord.Data);

                    namedRecordIndex.NamedRecordIndexes.Remove(name);
                    namedRecordIndex.NamedRecordIndexes.Add(name, recordAddress);

                    var updateRecord = UpdateRecord(namedRecordIndexRecord, namedRecordIndex.ToBytes());

                    this.MetaDataRecord.NamedRecordIndexAddress = updateRecord.Header.Address;
                });
        }   
                   
        public Int64 GetNamedRecordAddress(string name)
        {
            Int64 result = 0;
            WithMetaDataRecord(() => 
                {
                    var namedRecordIndex = GetNamedRecordIndex();
                    if (namedRecordIndex.NamedRecordIndexes.ContainsKey(name))
                    {
                        result = namedRecordIndex.NamedRecordIndexes[name];
                    }
                });
            return result;
        }
        #endregion

        #region internal methods

        /// <summary>
        /// LEAKY ABSTRACTION, FIX AFTER INDEX ENUMERATION
        /// </summary>
        /// <returns>The first record.</returns>
        internal Record GetFirstRecord()
        {
            LoadMetaDataRecord();
            var firstRecordAddress = this.MetaDataRecord.DataListEndPoints.StartAddress;
            if (firstRecordAddress > 0) {
                return ReadEntireRecord(firstRecordAddress);
            }
            SaveMetaDataRecord();
            return null;
        }

        /// <summary>
        /// LEAKY ABSTRACTION, FIX AFTER INDEX ENUMERATION
        /// </summary>
        /// <returns>The first record.</returns>
        internal Record GetNextRecord(Record record)
        {
            LoadMetaDataRecord();
            if (record.Header.Next > 0) {
                return ReadEntireRecord(record.Header.Next);
            }
            SaveMetaDataRecord();
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
            var allBytes = StorageEngine.Read(address, RecordHeader.ByteSize + header.AllocatedDataSize);
            var record =  Record.FromBytes(address, allBytes);
            return record;
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

        void ReuseEmptyRecordHeader(Record record, CollectionMetaDataRecord metaDataRecord)
        {        
            return;
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

        NamedRecordsIndex GetNamedRecordIndex(){
            return NamedRecordsIndex.FromBytes(GetNamedRecordIndexRecord().Data);
        }

        Record GetNamedRecordIndexRecord(){
        
            if (this.MetaDataRecord.NamedRecordIndexAddress > 0)
            {
                return GetRecord(this.MetaDataRecord.NamedRecordIndexAddress);
            }
            else
            {
                var namedRecordIndex = new NamedRecordsIndex();
                var namedRecordIndexRecord = AppendRecord(namedRecordIndex.ToBytes());

                this.MetaDataRecord.NamedRecordIndexAddress = namedRecordIndexRecord.Header.Address;

                return namedRecordIndexRecord;
            }
        }

        void WithMetaDataRecord(Action action)
        {
            LoadMetaDataRecord();
            action();
            SaveMetaDataRecord();
        }

        void LoadMetaDataRecord()
        {        
            if (this.MetaDataRecord != null)
                return;

            var bytes = StorageEngine.Read(0, CollectionMetaDataRecord.ByteSize);
            this.MetaDataRecord = CollectionMetaDataRecord.FromBytes(bytes);
        }

        void SaveMetaDataRecord()
        {
            StorageEngine.Write(0, this.MetaDataRecord.AsBytes());
        }
        #endregion
    }
}

