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
            IAllocationStrategy allocationStrategy,
            StorageEngine<T> storageEngine
        )
        {
            Session = session;
            StorageEngine = storageEngine;
            Serializer = serializer;
            AllocationStrategy = allocationStrategy;
            JournalEnabled = true; //journal by default
        }

        #region internal methods
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

        internal CollectionMetaDataRecord GetMetaDataRecord()
        {
            var bytes = StorageEngine.Read(0, CollectionMetaDataRecord.ByteSize);
            return CollectionMetaDataRecord.FromBytes(bytes);
        }

        internal void WithMetaDataRecord(Action<CollectionMetaDataRecord> action)
        {
            var metaDataRecord = GetMetaDataRecord();
            action(metaDataRecord);
            SaveMetaDataRecord (metaDataRecord);
        }

        internal void DisableJournal()
        {
            JournalEnabled = false;
            StorageEngine.DisableJournal();
        }

        internal void AppendObject(T obj)
        {
            var record = new Record();
            var data = Serializer.Serialize(obj);
            record.Header.DataSize = data.Length;
            record.Header.AllocatedDataSize = AllocationStrategy.CalculateSize(record);
            record.Data = new byte[record.Header.AllocatedDataSize];
            data.CopyTo(record.Data, 0);                               
            AppendRecord(record);
        }

        internal void AppendRecord (Record record){
            WithMetaDataRecord((metaDataRecord) => {
                ReuseEmptyRecordHeader(record, metaDataRecord);
                AppendRecord(record, metaDataRecord.DataListEndPoints); 
            });                
        }

        internal void UpdateObject(Record record, T obj)
        {
            var bytes = Serializer.Serialize(obj);
            if (bytes.Length >= record.Header.AllocatedDataSize )
            {
                ReleaseRecord(record);
                AppendObject(obj); 
            }
            else 
            {   
                record.Data = new byte[record.Header.AllocatedDataSize];
                bytes.CopyTo(record.Data, 0);
                record.Header.DataSize = bytes.Length;
                StorageEngine.Write(record.Header.Address, record.AsBytes ());
            }
        }

        internal void ReleaseRecord(Record record)
        {                  
            WithMetaDataRecord ((metaDataRecord) => {
                //remove from list
                RemoveRecord(record, metaDataRecord.DataListEndPoints); 
                //append to empty list
                AppendRecord(record, metaDataRecord.EmptyListEndPoints);

                metaDataRecord.Sanitize();
            });                
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
            var allBytes = StorageEngine.Read(address, header.TotalRecordSize);
            var record =  Record.FromBytes(address, allBytes);
            return record;
        }            

        void AppendRecord (Record record, ListEndPoints listEndPoints)
        {
            var operation = new RecordListAppendOperation(
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
            
        void SaveMetaDataRecord(CollectionMetaDataRecord record)
        {
            StorageEngine.Write(0, record.AsBytes());
        }


        #endregion
    }
}

