using System;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Serialization;
using MarcelloDB.Storage;
using MarcelloDB.Records.__;
using MarcelloDB.Index;
using MarcelloDB.Transactions;
using MarcelloDB.Buffers;

namespace MarcelloDB.Records
{
    internal interface IRecordManager
    {
        Record GetRecord(Int64 address);

        Record AppendRecord(ByteBuffer buffer, bool hasObject = false, bool reuseRecycledRecord = true);

        Record UpdateRecord(Record record, ByteBuffer buffer, bool reuseRecycledRecord = true);

        void Recycle(Int64 address);
            
        void RegisterNamedRecordAddress(string name, Int64 recordAddress, bool reuseRecycledRecord = true);

        Int64 GetNamedRecordAddress(string name);
    }

    internal class TransactionState
    {
        internal CollectionRoot CollectionRoot { get; set; }

        internal RecordIndex EmptyRecordIndex{ get; set;}
    }

    internal class RecordManager<T> : IRecordManager, ITransactor
    {   
        Marcello Session { get; set; }

        StorageEngine<T> StorageEngine { get;set; }

        IAllocationStrategy AllocationStrategy { get; set; }

        bool JournalEnabled { get; set; }

        TransactionState TransactionState { get; set; }

        internal RecordManager(
            Marcello session,
            IAllocationStrategy allocationStrategy,
            StorageEngine<T> storageEngine
        )
        {         
            this.Session = session;
            StorageEngine = storageEngine;
            AllocationStrategy = allocationStrategy;
            JournalEnabled = true; //journal by default
            ResetTransactionState();
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

        public Record AppendRecord(ByteBuffer buffer, bool hasObject = false, bool reuseRecycledRecord = true)
        {
            Record record = null;
            if (reuseRecycledRecord)
            {
                record = ReuseRecycledRecord(buffer.Length);
            }

            if (record == null)
            {
                //append
                record = new Record();
            
                WithCollectionRoot(() =>
                    {
                        record.Header.AllocatedDataSize = AllocationStrategy.CalculateSize(buffer.Length);
                        record.Data = buffer;
                        record.Header.HasObject = hasObject;

                        AppendRecordToList(record);                                       
                    });
            }
            else //reuse
            {
                UpdateRecord(record, buffer);
            }
            return record;
        }

        public Record UpdateRecord(Record record, ByteBuffer buffer, bool reuseRecycledRecord = true)
        {
            Record result = null;

            WithCollectionRoot(() =>
                {
                    if (buffer.Length > record.Header.AllocatedDataSize )
                    {
                        result = AppendRecord(buffer, record.Header.HasObject, reuseRecycledRecord); 
                        if(reuseRecycledRecord){
                            Recycle(record.Header.Address);
                        }
                    }
                    else 
                    {   
                        record.Data = buffer;

                        buffer = record.AsBuffer(this.Session);
                        StorageEngine.Write(record.Header.Address, buffer);
                        result = record;
                    }

                });

            return result;
        }

        public void Recycle(Int64 address)
        {
            var recordHeader = ReadRecordHeader(address);
            var emptyRecordIndex = GetEmptyRecordIndex();
            emptyRecordIndex.Register(
                recordHeader.AllocatedDataSize, 
                recordHeader.Address);
        }

        public void RegisterNamedRecordAddress(string name, Int64 recordAddress, bool reuseRecycledRecord = true)
        {       
            WithCollectionRoot(() =>
                {
                    var namedRecordIndexRecord = GetNamedRecordIndexRecord();

                    var namedRecordIndex = NamedRecordsIndex.FromBuffer(namedRecordIndexRecord.Data);

                    namedRecordIndex.NamedRecordIndexes.Remove(name);
                    namedRecordIndex.NamedRecordIndexes.Add(name, recordAddress);

                    var updateRecord = UpdateRecord(
                        namedRecordIndexRecord, 
                        namedRecordIndex.ToBuffer(this.Session), 
                        reuseRecycledRecord);

                    TransactionState.CollectionRoot.NamedRecordIndexAddress = updateRecord.Header.Address;
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

        #region ITransactor implementation

        public void SaveState()
        {
            SaveCollectionRoot();
        }

        public void RollbackState()
        {
            ResetTransactionState();
        }

        public void CleanUp()
        {
        }
        #endregion
            
        internal void DisableJournal()
        {
            JournalEnabled = false;
            StorageEngine.DisableJournal();
        }
            
        void WriteHeader(Record record)
        {
            var bytes = record.Header.AsBuffer(this.Session);
            StorageEngine.Write(record.Header.Address, bytes);
        }            

        Record ReadEntireRecord(Int64 address)
        {
            var header = ReadRecordHeader(address);
            //only read real data. (not all alocated data)
            var allBytes = StorageEngine.Read(address, RecordHeader.ByteSize + header.DataSize);
            var buffer = this.Session.ByteBufferManager.FromBytes(allBytes);
            var record =  Record.FromBuffer(this.Session, address, buffer);
            return record;
        }

        RecordHeader ReadRecordHeader(Int64 address)
        {
            var bytes = StorageEngine.Read(address, RecordHeader.ByteSize);
            var buffer = this.Session.ByteBufferManager.FromBytes(bytes);
            return RecordHeader.FromBuffer(this.Session, address, buffer);
        }

        void WithCollectionRoot(Action action)
        {
            //makes sure the collection root is loaded into the transaction state
            LoadCollectionRoot();
            action();
        }

        void LoadCollectionRoot()
        {        
            if (TransactionState.CollectionRoot == null)
            {
                var bytes = StorageEngine.Read(0, CollectionRoot.ByteSize);
                var buffer = this.Session.ByteBufferManager.FromBytes(bytes);
                TransactionState.CollectionRoot = CollectionRoot.FromBuffer(this.Session, buffer);                 
            }

        }

        void SaveCollectionRoot()
        {
            StorageEngine.Write(0, TransactionState.CollectionRoot.AsBuffer(this.Session));
        }
        
        void ResetTransactionState()
        {
            this.TransactionState = new TransactionState();
        }

        void AppendRecordToList (Record record)
        {
            record.Header.Address = TransactionState.CollectionRoot.Head;
            TransactionState.CollectionRoot.Head += record.Header.TotalRecordSize;

            var buffer = record.AsBuffer(this.Session);

            StorageEngine.Write (record.Header.Address, buffer);
        }            
            
        NamedRecordsIndex GetNamedRecordIndex()
        {
            return NamedRecordsIndex.FromBuffer(GetNamedRecordIndexRecord().Data);
        }

        Record GetNamedRecordIndexRecord()
        {
            EnsureNamedRecordIndex();
            return GetRecord(TransactionState.CollectionRoot.NamedRecordIndexAddress);
        }

        void EnsureNamedRecordIndex()
        {
            if (TransactionState.CollectionRoot.NamedRecordIndexAddress == 0)
            {
                var namedRecordIndex = new NamedRecordsIndex();
                var namedRecordIndexRecord = AppendRecord(
                    namedRecordIndex.ToBuffer(this.Session), 
                    reuseRecycledRecord:false);

                TransactionState.CollectionRoot.NamedRecordIndexAddress = 
                    namedRecordIndexRecord.Header.Address;
            }

        }

        RecordIndex GetEmptyRecordIndex()
        {
            if (TransactionState.EmptyRecordIndex == null)
            {
                TransactionState.EmptyRecordIndex = RecordIndex.Create(
                    this.Session, 
                    this,
                    RecordIndex.EMPTY_RECORDS_BY_SIZE, 
                    canReuseRecycledRecords:false); //do not reuse records for this index. It will try to reuse for building itself.
            }
            return TransactionState.EmptyRecordIndex;
        }

        Record ReuseRecycledRecord(int minimumLength)
        {
            var emptyRecordIndex = GetEmptyRecordIndex();
            var walker = emptyRecordIndex.GetWalker();

            var entry = walker.Next();
            while (entry != null && 
                new ObjectComparer().Compare(entry.Key, minimumLength) <= 0)
            {
                entry = walker.Next();
            }
            if (entry != null)
            {         
                emptyRecordIndex.UnRegister(entry.Key);
                return GetRecord(entry.Pointer);
            }
            return null;
        }                                          
    }
}

