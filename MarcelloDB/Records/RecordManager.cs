﻿using System;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Serialization;
using MarcelloDB.Storage;
using MarcelloDB.Records.__;
using MarcelloDB.Index;

namespace MarcelloDB.Records
{
    internal interface IRecordManager
    {
        Record GetRecord(Int64 address);

        Record AppendRecord(byte[] data, bool hasObject = false, bool reuseRecycledRecord = true);

        Record UpdateRecord(Record record, byte[] data);

        void Recycle(Int64 address);
            
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

        public Record AppendRecord(byte[] data, bool hasObject = false, bool reuseRecycledRecord = true)
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
                        record.Header.DataSize = data.Length;
                        record.Header.AllocatedDataSize = AllocationStrategy.CalculateSize(record);
                        record.Header.HasObject = hasObject;
                        record.Data = new byte[record.Header.DataSize];

                        data.CopyTo(record.Data, 0);

                        AppendRecordToList(record);                                       
                    });
            }
            else //reuse
            {
                UpdateRecord(record, data);
            }
            return record;
        }

        public Record UpdateRecord(Record record, byte[] data)
        {
            Record result = null;

            WithCollectionRoot(() =>
                {
                    if (data.Length >= record.Header.AllocatedDataSize )
                    {
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

        public void Recycle(Int64 address)
        {
            var recordHeader = ReadRecordHeader(address);
            var emptyRecordIndex = GetEmptyRecordIndex();
            emptyRecordIndex.Register(
                recordHeader.AllocatedDataSize, 
                recordHeader.Address);
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
            var header = ReadRecordHeader(address);
            var allBytes = StorageEngine.Read(address, RecordHeader.ByteSize + header.AllocatedDataSize);
            var record =  Record.FromBytes(address, allBytes);
            return record;
        }

        RecordHeader ReadRecordHeader(Int64 address)
        {
           return RecordHeader.FromBytes(address, StorageEngine.Read(address, RecordHeader.ByteSize));
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

        void AppendRecordToList (Record record)
        {
            record.Header.Address = this.CollectionRoot.Head;
            this.CollectionRoot.Head += record.Header.TotalRecordSize;
            StorageEngine.Write (record.Header.Address, record.AsBytes ());
        }            
            
        NamedRecordsIndex GetNamedRecordIndex()
        {
            return NamedRecordsIndex.FromBytes(GetNamedRecordIndexRecord().Data);
        }

        Record GetNamedRecordIndexRecord()
        {
            EnsureNamedRecordIndex();
            return GetRecord(this.CollectionRoot.NamedRecordIndexAddress);
        }

        void EnsureNamedRecordIndex()
        {
            if (this.CollectionRoot.NamedRecordIndexAddress == 0)
            {
                var namedRecordIndex = new NamedRecordsIndex();
                var namedRecordIndexRecord = AppendRecord(
                    namedRecordIndex.ToBytes(), reuseRecycledRecord:false);

                this.CollectionRoot.NamedRecordIndexAddress = 
                    namedRecordIndexRecord.Header.Address;
            }

        }

        RecordIndex GetEmptyRecordIndex()
        {
            return RecordIndex.Create(
                this, 
                RecordIndex.EMPTY_RECORDS_BY_SIZE, 
                canReuseRecycledRecords:false); //do not reuse records for this index. It will try to reuse for building itself.
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

