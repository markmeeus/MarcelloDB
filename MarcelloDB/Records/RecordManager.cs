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

    internal class RecordManager : IRecordManager, ITransactor
    {
        RecordIndex<EmptyRecordIndexKey> _emptyRecordIndex;

        CollectionFileRoot _root;

        Record _rootRecord;

        Session Session { get; set; }

        StorageEngine StorageEngine { get;set; }

        IAllocationStrategy AllocationStrategy { get; set; }

        internal RecordManager(
            IAllocationStrategy allocationStrategy,
            StorageEngine storageEngine
        )
        {
            this.StorageEngine = storageEngine;
            this.AllocationStrategy = allocationStrategy;
        }

        RecordIndex<EmptyRecordIndexKey> EmptyRecordIndex
        {
            get
            {
                if (_emptyRecordIndex == null)
                {
                    _emptyRecordIndex = RecordIndex.Create<EmptyRecordIndexKey>(
                        this,
                        RecordIndex.EMPTY_RECORDS_BY_SIZE,
                        canReuseRecycledRecords:false); //do not reuse records for this index. It will try to reuse for building itself.
                }
                return _emptyRecordIndex;
            }
        }

        CollectionFileRoot Root
        {
            get
            {
                if (_root == null)
                {
                    LoadCollectionFileRoot();
                }
                return _root;
            }

        }

        #region IRecordManager implementation
        public Record GetRecord(Int64 address)
        {
            return ReadEntireRecord(address);
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
                record.Header.AllocatedDataSize = AllocationStrategy.CalculateSize(data.Length);
                record.Data = data;
                WriteRecordAtHead(record);

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

            return result;
        }

        public void Recycle(Int64 address)
        {
            var recordHeader = ReadRecordHeader(address);

            var emptyRecordIndexKey = new EmptyRecordIndexKey
            {
                A = recordHeader.Address, S = recordHeader.AllocatedDataSize
            };
            this.EmptyRecordIndex.Register(
                emptyRecordIndexKey,
                recordHeader.Address);
        }

        public void RegisterNamedRecordAddress(string name, Int64 recordAddress, bool reuseRecycledRecord = true)
        {
            var namedRecordIndexRecord = GetNamedRecordIndexRecord();

            var namedRecordIndex = NamedRecordsIndex.FromBytes(namedRecordIndexRecord.Data);

            namedRecordIndex.NamedRecordIndexes.Remove(name);
            namedRecordIndex.NamedRecordIndexes.Add(name, recordAddress);

            var updateRecord = UpdateRecord(namedRecordIndexRecord, namedRecordIndex.ToBytes(), reuseRecycledRecord);

            this.Root.NamedRecordIndexAddress = updateRecord.Header.Address;
        }

        public Int64 GetNamedRecordAddress(string name)
        {
            var namedRecordIndex = GetNamedRecordIndex();
            if (namedRecordIndex.NamedRecordIndexes.ContainsKey(name))
            {
                return  namedRecordIndex.NamedRecordIndexes[name];
            }

            return 0;
        }
        #endregion //IRecordManager implementation

        #region ITransactor implementation

        public void SaveState()
        {
            SaveCollectionRoot();
        }

        public void RollbackState()
        {
            _root = null;
            _rootRecord = null;
            _emptyRecordIndex = null;
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

        void WriteRecordAtHead (Record record)
        {
            record.Header.Address = this.Root.Head;
            this.Root.Head += record.Header.TotalRecordSize;

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
            return GetRecord(this.Root.NamedRecordIndexAddress);
        }

        void EnsureNamedRecordIndex()
        {
            if (this.Root.NamedRecordIndexAddress == 0)
            {
                var namedRecordIndex = new NamedRecordsIndex();
                var namedRecordIndexRecord = AppendRecord(
                    namedRecordIndex.ToBytes(), reuseRecycledRecord:false);

                this.Root.NamedRecordIndexAddress =
                    namedRecordIndexRecord.Header.Address;
            }
        }

        Record ReuseRecycledRecord(int minimumLength)
        {
            var walker = this.EmptyRecordIndex.GetWalker();

            var entry = walker.Next();
            while (entry != null &&
                ((EmptyRecordIndexKey)entry.Key).S < minimumLength)
            {
                entry = walker.Next();
            }
            if (entry != null)
            {
                this.EmptyRecordIndex.UnRegister(entry.Key);
                return GetRecord(entry.Pointer);
            }
            return null;
        }

        void LoadCollectionFileRoot(){
            var rootAddress = ReadRootAddress();
            if (rootAddress > 0)
            {
                _rootRecord = this.GetRecord(rootAddress);
                _root = CollectionFileRoot.Deserialize(_rootRecord.Data);
            }
            else
            {
                _root = new CollectionFileRoot();
            }
        }

        void SaveCollectionRoot()
        {
            if (this.Root.IsDirty)
            {
                byte[] data = this.Root.Serialize();
                if (_rootRecord == null)
                {
                    _rootRecord = this.AppendRecord(data, true);
                }
                else
                {
                    _rootRecord = this.UpdateRecord(_rootRecord, data, true);
                }

                WriteRootAddress(_rootRecord.Header.Address);
            }
            this.Root.Clean();
        }

        Int64 ReadRootAddress()
        {
            return new BufferReader(
                this.StorageEngine.Read(0, sizeof(Int64))
            ).ReadInt64();
        }

        void WriteRootAddress(Int64 address)
        {
            this.StorageEngine.Write(
                0,
                new BufferWriter(new byte[sizeof(Int64)]).WriteInt64(address).GetTrimmedBuffer()
            );
        }


    }
}

