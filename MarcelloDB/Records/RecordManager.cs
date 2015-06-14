using System;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Serialization;
using MarcelloDB.Storage;
using MarcelloDB.Records;
using MarcelloDB.Index;
using MarcelloDB.Transactions;
using MarcelloDB.Collections;

namespace MarcelloDB.Records
{
    internal interface IRecordManager
    {
        Record GetRecord(Int64 address);

        Record AppendRecord(byte[] data, bool reuseRecycledRecord = true);

        Record UpdateRecord(Record record, byte[] data, bool reuseRecycledRecord = true);

        void Recycle(Int64 address);

        void RegisterNamedRecordAddress(string name, Int64 recordAddress, bool reuseRecycledRecord = true);

        Int64 GetNamedRecordAddress(string name);
    }

    internal class TransactionState
    {
        internal CollectionRoot CollectionRoot { get; set; }

        internal RecordIndex<EmptyRecordIndexKey> EmptyRecordIndex{ get; set;}
    }

    internal class RecordManager : IRecordManager, ITransactor
    {
        Session Session { get; set; }

        StorageEngine StorageEngine { get;set; }

        IAllocationStrategy AllocationStrategy { get; set; }

        TransactionState TransactionState { get; set; }

        CollectionFile CollectionFile { get; set; }

        internal RecordManager(
            CollectionFile collectionFile,
            IAllocationStrategy allocationStrategy,
            StorageEngine storageEngine
        )
        {
            this.CollectionFile = collectionFile;
            this.StorageEngine = storageEngine;
            this.AllocationStrategy = allocationStrategy;
            this.ResetTransactionState();
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

        public Record AppendRecord(byte[] data, bool reuseRecycledRecord = true)
        {
            Record record = null;
            if (reuseRecycledRecord)
            {
                record = ReuseRecycledRecord(data.Length);
            }

            if (record == null)
            {
                //append
                record = new Record();

                WithCollectionRoot(() =>
                    {
                        record.Header.AllocatedDataSize = AllocationStrategy.CalculateSize(data.Length);
                        record.Data = data;

                        WriteRecordAtHead(record);
                    });
            }
            else //reuse
            {
                UpdateRecord(record, data);
            }
            return record;
        }

        public Record UpdateRecord(Record record, byte[] data, bool reuseRecycledRecord = true)
        {
            Record result = null;

            WithCollectionRoot(() =>
                {
                    if (data.Length > record.Header.AllocatedDataSize )
                    {
                        result = AppendRecord(data, reuseRecycledRecord);
                        if(reuseRecycledRecord){
                            Recycle(record.Header.Address);
                        }
                    }
                    else
                    {
                        record.Data = data;

                        var bytes = record.AsBytes();
                        StorageEngine.Write(record.Header.Address, bytes);
                        result = record;
                    }

                });

            return result;
        }

        public void Recycle(Int64 address)
        {
            var recordHeader = ReadRecordHeader(address);
            var emptyRecordIndex = GetEmptyRecordIndex();
            var emptyRecordIndexKey = new EmptyRecordIndexKey
            {
                A = recordHeader.Address, S = recordHeader.AllocatedDataSize
            };
            emptyRecordIndex.Register(
                emptyRecordIndexKey,
                recordHeader.Address);
        }

        public void RegisterNamedRecordAddress(string name, Int64 recordAddress, bool reuseRecycledRecord = true)
        {
            WithCollectionRoot(() =>
                {
                    var namedRecordIndexRecord = GetNamedRecordIndexRecord();

                    var namedRecordIndex = NamedRecordsIndex.FromBytes(namedRecordIndexRecord.Data);

                    namedRecordIndex.NamedRecordIndexes.Remove(name);
                    namedRecordIndex.NamedRecordIndexes.Add(name, recordAddress);

                    var updateRecord = UpdateRecord(namedRecordIndexRecord, namedRecordIndex.ToBytes(), reuseRecycledRecord);

                    this.CollectionFile.GetRoot().NamedRecordIndexAddress = updateRecord.Header.Address;
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
        #endregion

        Record ReadEntireRecord(Int64 address)
        {
            var header = ReadRecordHeader(address);
            //only read real data. (not all alocated data)
            var allBytes = StorageEngine.Read(address, RecordHeader.ByteSize + header.DataSize);
            return Record.FromBytes(address, allBytes);
        }

        RecordHeader ReadRecordHeader(Int64 address)
        {
           return RecordHeader.FromBytes(address, StorageEngine.Read(address, RecordHeader.ByteSize));
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
                var bytes = StorageEngine.Read(0, CollectionRoot.MaxByteSize);
                TransactionState.CollectionRoot = CollectionRoot.FromBytes(bytes);
            }

        }

        void SaveCollectionRoot()
        {
            if (TransactionState.CollectionRoot.Dirty)
            {
                //StorageEngine.Write(0, TransactionState.CollectionRoot.AsBytes());
                TransactionState.CollectionRoot.Clean();
            }
        }

        void ResetTransactionState()
        {
            this.TransactionState = new TransactionState();
        }

        void WriteRecordAtHead (Record record)
        {
            record.Header.Address = this.CollectionFile.GetRoot().Head;
            this.CollectionFile.GetRoot().Head += record.Header.TotalRecordSize;

            var bytes = record.AsBytes();

            StorageEngine.Write (record.Header.Address,bytes );
        }

        NamedRecordsIndex GetNamedRecordIndex()
        {
            return NamedRecordsIndex.FromBytes(GetNamedRecordIndexRecord().Data);
        }

        Record GetNamedRecordIndexRecord()
        {
            EnsureNamedRecordIndex();
            return GetRecord(this.CollectionFile.GetRoot().NamedRecordIndexAddress);
        }

        void EnsureNamedRecordIndex()
        {
            if (this.CollectionFile.GetRoot().NamedRecordIndexAddress == 0)
            {
                var namedRecordIndex = new NamedRecordsIndex();
                var namedRecordIndexRecord = AppendRecord(
                    namedRecordIndex.ToBytes(), reuseRecycledRecord:false);

                this.CollectionFile.GetRoot().NamedRecordIndexAddress =
                    namedRecordIndexRecord.Header.Address;
            }
        }

        RecordIndex<EmptyRecordIndexKey> GetEmptyRecordIndex()
        {
            if (TransactionState.EmptyRecordIndex == null)
            {
                TransactionState.EmptyRecordIndex = RecordIndex.Create<EmptyRecordIndexKey>(
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
                ((EmptyRecordIndexKey)entry.Key).S < minimumLength)
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

