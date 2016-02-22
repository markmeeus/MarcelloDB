using System;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Serialization;
using MarcelloDB.Storage;
using MarcelloDB.Records;
using MarcelloDB.Index;
using MarcelloDB.Transactions;
using MarcelloDB.Collections;
using System.Collections.Generic;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Records
{
    internal interface IRecordManager
    {
        Record GetRecord(Int64 address);

        Record AppendRecord(
            byte[] data,
            IAllocationStrategy allocationStrategy);

        Record UpdateRecord(
            Record record,
            byte[] data,
            IAllocationStrategy allocationStrategy);

        void Recycle(Int64 address);

        void RegisterNamedRecordAddress(
            string name,
            Int64 recordAddress);

        Int64 GetNamedRecordAddress(string name);
    }

    internal class RecordManager : SessionBoundObject, IRecordManager, ITransactor
    {
        EmptyRecordIndex _emptyRecordIndex;

        List<Int64> _recordsToRecycle;

        CollectionFileRoot _root;

        Record _rootRecord;

        StorageEngine StorageEngine { get;set; }

        internal RecordManager(
            Session session,
            StorageEngine storageEngine
        ):base(session)
        {
            this.StorageEngine = storageEngine;
            _recordsToRecycle = new List<Int64>();
        }

        RecordIndex<EmptyRecordIndexKey> EmptyRecordIndex
        {
            get
            {
                if (_emptyRecordIndex == null)
                {
                    _emptyRecordIndex = new Index.EmptyRecordIndex(
                        this.Session,
                        this,
                        RecordIndex.EMPTY_RECORDS_BY_SIZE,
                        this.Session.SerializerResolver.SerializerFor<Node<EmptyRecordIndexKey, Int64>>());
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

        public Record AppendRecord(byte[] data, IAllocationStrategy allocationStrategy)
        {
            Record record = null;

            record = ReuseRecycledRecord(data.Length);

            if (record == null)
            {
                //append
                record = new Record();
                record.Header.AllocatedDataSize = allocationStrategy.CalculateSize(data.Length);
                record.Data = data;
                WriteRecordAtHead(record);

            }
            else //reuse
            {
                UpdateRecord(record, data, allocationStrategy);
            }
            return record;
        }

        public Record UpdateRecord(Record record, byte[] data, IAllocationStrategy allocationStrategy)
        {
            if (data.Length > record.Header.AllocatedDataSize )
            {
                Recycle(record.Header.Address);
                return AppendRecord(data, allocationStrategy);
            }
            else
            {
                record.Data = data;
                StorageEngine.Write(record.Header.Address, record.AsBytes());
                return record;
            }
        }

        public void Recycle(Int64 address)
        {
            _recordsToRecycle.Add(address);
        }

        public void RegisterNamedRecordAddress(string name, Int64 recordAddress)
        {
            var namedRecordIndexRecord = GetNamedRecordIndexRecord();

            var namedRecordIndex = this.Session.SerializerResolver.SerializerFor<NamedRecordsIndex>()
                .Deserialize(namedRecordIndexRecord.Data);

            namedRecordIndex.NamedRecordIndexes.Remove(name);
            namedRecordIndex.NamedRecordIndexes.Add(name, recordAddress);
            var allocationStrategy = this.Session.AllocationStrategyResolver.StrategyFor(namedRecordIndex);
            var updateRecord = UpdateRecord(
                namedRecordIndexRecord,
                this.Session.SerializerResolver.SerializerFor<NamedRecordsIndex>().Serialize(namedRecordIndex),
                allocationStrategy);

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
            RegisterRecycledRecordsInEmptyRecordIndex();
            _emptyRecordIndex = null;
            _recordsToRecycle.Clear();
        }

        public void RollbackState()
        {
            _root = null;
            _rootRecord = null;
            _emptyRecordIndex = null;
            _recordsToRecycle.Clear();
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
            return this.Session.SerializerResolver.SerializerFor<NamedRecordsIndex>()
                .Deserialize(GetNamedRecordIndexRecord().Data);
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
                var allocationStrategy = this.Session.AllocationStrategyResolver.StrategyFor(namedRecordIndex);
                var namedRecordIndexRecord =
                    AppendRecord(
                        Session.SerializerResolver.SerializerFor<NamedRecordsIndex>().Serialize(namedRecordIndex),
                        allocationStrategy);

                this.Root.NamedRecordIndexAddress =
                    namedRecordIndexRecord.Header.Address;
            }
        }

        Record ReuseRecycledRecord(int minimumLength)
        {
            return UsingEmptyRecordIndex(()=>{
                var walker = this.EmptyRecordIndex.GetWalker();

                var range = new BTreeWalkerRange<EmptyRecordIndexKey>();
                range.SetStartAt(new EmptyRecordIndexKey{S = minimumLength, A = 0});
                walker.SetRange(range);

                var entry = walker.Next();

                if (entry != null)
                {
                    this.EmptyRecordIndex.UnRegister(entry.Key);
                    return GetRecord(entry.Pointer);
                }

                return null;
            });
        }

        void RegisterRecycledRecordsInEmptyRecordIndex()
        {
            var recyclingRecords = _recordsToRecycle.ToArray();
            //Recycling records may cause nodes from the empty record index to be recycled.
            //If this happesn, the _recordToRecycle list contains new records.
            while (recyclingRecords.Length > 0)
            {
                _recordsToRecycle.Clear();

                foreach (var address in recyclingRecords)
                {
                    var recordHeader = ReadRecordHeader(address);

                    var emptyRecordIndexKey = new EmptyRecordIndexKey
                    {
                        A = recordHeader.Address, S = recordHeader.AllocatedDataSize
                    };

                    UsingEmptyRecordIndex(() =>
                    {
                        this.EmptyRecordIndex.Register(
                            emptyRecordIndexKey,
                            recordHeader.Address);
                    });

                }

                recyclingRecords = _recordsToRecycle.ToArray();
            }
        }

        void LoadCollectionFileRoot(){
            var rootAddress = ReadRootAddress();
            if (rootAddress > 0)
            {
                _rootRecord = this.GetRecord(rootAddress);
                _root = this.Session.SerializerResolver.SerializerFor<CollectionFileRoot>()
                    .Deserialize(_rootRecord.Data);
                _root.Validate();
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
                byte[] data = this.Session.SerializerResolver.SerializerFor<CollectionFileRoot>()
                    .Serialize(this.Root);

                var allocationStrategy = this.Session.AllocationStrategyResolver.StrategyFor(this.Root);
                if (_rootRecord == null)
                {
                    _rootRecord = this.AppendRecord(data, allocationStrategy);
                }
                else
                {
                    _rootRecord = this.UpdateRecord(_rootRecord, data, allocationStrategy);
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

        bool usingEmptyRecordIndex;
        T UsingEmptyRecordIndex<T>(Func<T> func)
        {
            if (usingEmptyRecordIndex)
            {
                return default(T);
            }
            usingEmptyRecordIndex = true;
            var result = func();
            usingEmptyRecordIndex = false;
            return result;
        }

        void UsingEmptyRecordIndex(Action action)
        {
            UsingEmptyRecordIndex(()=>{
                action();
                return true;
            });
        }
    }
}

