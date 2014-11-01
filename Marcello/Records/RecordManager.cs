using System;

namespace Marcello
{
    internal class RecordManager<T>
    {   
        Marcello Session { get; set; }

        StorageEngine<T> StorageEngine { get;set; }

        IObjectSerializer<T> Serializer { get; set; }

        IAllocationStrategy AllocationStrategy { get; set; }
               
        internal RecordManager(Marcello session,
            StorageEngine<T> storageEngine,
            IObjectSerializer<T> serializer,
            IAllocationStrategy allocationStrategy
        )
        {
            Session = session;
            StorageEngine = new StorageEngine<T>(Session.StreamProvider);
            Serializer = serializer;
            AllocationStrategy = allocationStrategy;
        }

        internal Record GetFirstRecord()
        {
            var firstRecordAddress = GetMetaDataRecord().FirstRecordAddress;
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

        internal Record GetPreviousRecord(Record record)
        {
            if (record.Header.Previous > 0) {
                return ReadEntireRecord(record.Header.Previous);
            }
            return null;
        }
            
        internal Record GetLastRecord()
        {
            var lastRecordAddress = GetMetaDataRecord().LastRecordAddress;
            if (lastRecordAddress > 0) {
                return ReadEntireRecord (lastRecordAddress);
            }
            return null;
        }

        #region private methods
        public void AppendObject(T obj)
        {
            var record = new Record();
            var data = Serializer.Serialize(obj);
            record.Header.DataSize = data.Length;

            record.Header.AllocatedSize = AllocationStrategy.CalculateSize(record);


            record.Data = new byte[record.Header.AllocatedSize];
            data.CopyTo(record.Data, 0);

            var lastRecord = GetLastRecord();
            if (lastRecord != null) 
            {
                record.Header.Address = lastRecord.Header.Address + lastRecord.Header.AllocatedSize;
                record.Header.Previous = lastRecord.Header.Address;
                lastRecord.Header.Next = record.Header.Address;         
                WriteHeader(lastRecord);
            }
            else 
            {
                record.Header.Address = CollectionMetaDataRecord.ByteSize; //first record starts after the metadata record         
                SetFirstRecord(record);
            }

            StorageEngine.Write(record.Header.Address, record.AsBytes());
            SetLastRecord (record);
        }

        public void UpdateObject(Record record, T obj)
        {
            var bytes = Serializer.Serialize(obj);
            if (bytes.Length > record.Header.AllocatedSize) 
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
            var previousRecord = GetPreviousRecord(record);
            var nextRecord = GetNextRecord(record);

            if (previousRecord != null && nextRecord != null) 
            {
                //record in the middle, link previous to next and vice versa
                nextRecord.Header.Previous = previousRecord.Header.Address;
                previousRecord.Header.Next = nextRecord.Header.Address;
            }

            if (previousRecord != null && nextRecord == null) 
            {
                //no next record, so this was the last one
                previousRecord.Header.Next = 0;
                SetLastRecord(previousRecord);
            }

            if (previousRecord == null && nextRecord != null) 
            {
                //first record
                nextRecord.Header.Previous = 0;
                SetFirstRecord(nextRecord);
            }

            if (previousRecord == null && nextRecord == null) 
            {
                //this was the last one
                SetEmpty();
            }

            if (nextRecord != null) 
            {
                WriteHeader(nextRecord);
            }

            if (previousRecord != null) 
            {
                WriteHeader(previousRecord);
            }
        }

        void WriteHeader(Record record)
        {
            StorageEngine.Write (record.Header.Address, record.Header.AsBytes());
        }

        Record ReadEntireRecord(Int64 address)
        {
            var header = RecordHeader.FromBytes(address, StorageEngine.Read (address, RecordHeader.ByteSize));
            var allBytes = StorageEngine.Read(address, header.AllocatedSize);
            var record =  Record.FromBytes(address, allBytes);
            return record;
        }

        void SetFirstRecord(Record record)
        {
            UpdateMetaData(r => r.FirstRecordAddress = record.Header.Address);
        }

        void SetLastRecord(Record record)
        {
            UpdateMetaData(r => r.LastRecordAddress = record.Header.Address);
        }

        void SetEmpty()
        {
            UpdateMetaData((r) => {
                r.LastRecordAddress = 0;
                r.FirstRecordAddress = 0; 
            });
        }

        void UpdateMetaData(Action<CollectionMetaDataRecord> updateAction)
        {
            var metaData = GetMetaDataRecord();
            updateAction(metaData);
            SaveMetaDataRecord(metaData);
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

