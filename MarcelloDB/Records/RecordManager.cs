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
using System.Linq;

namespace MarcelloDB.Records
{
    internal interface IRecordManager
    {
        Record GetRecord(Int64 address);

        Record AppendRecord (
            byte [] data,
            IAllocationStrategy allocationStrategy,
            bool allowRecordReuse = true);

        Record UpdateRecord(
            Record record,
            byte[] data,
            IAllocationStrategy allocationStrategy,
            bool allowRecordReuse = true);

        void RegisterEmpty(Int64 address);

        void RegisterNamedRecordAddress(
            string name,
            Int64 recordAddress,
            bool allowRecordReuse = true);

        Int64 GetNamedRecordAddress(string name);
    }

    internal class RecordManager : SessionBoundObject, IRecordManager, ITransactor
    {
        bool _recordReuseEnabled = true;

        EmptyRecordIndex _emptyRecordIndex;

        List<Int64> _recordsToRegisterEmptyRecordIndex;

        Dictionary<Int64, EmptyRecordIndexKey> _reusedRecords = new Dictionary<Int64, EmptyRecordIndexKey> ();

        CollectionFileRoot _root;

        Record _rootRecord;

        StorageEngine StorageEngine { get;set; }

        internal RecordManager(
            Session session,
            StorageEngine storageEngine
        ):base(session)
        {
            this.StorageEngine = storageEngine;
            _recordsToRegisterEmptyRecordIndex = new List<Int64>();
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
                        this.Session.SerializerResolver.SerializerFor<Node<EmptyRecordIndexKey>>());
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

        public Record AppendRecord(byte [] data,
                                   IAllocationStrategy allocationStrategy,
                                   bool allowRecordReuse = true)
        {
            Record record = null;
            int maxAllocationSize = allocationStrategy.CalculateSize (data.Length);
            if(allowRecordReuse){
                record = ReuseEmptyRecord (data.Length, maxAllocationSize);    
            }

            if (record == null)
            {
                //append
                record = new Record();
                record.Header.AllocatedDataSize = maxAllocationSize;
                record.Data = data;
                WriteRecordAtHead(record);

            }
            else //reuse
            {
                UpdateRecord(record, data, allocationStrategy);
            }
            return record;
        }

        public Record UpdateRecord(Record record, 
                                   byte[] data, 
                                   IAllocationStrategy allocationStrategy,
                                   bool allowRecordReuse = true)
        {
            if (data.Length > record.Header.AllocatedDataSize )
            {
                RegisterEmpty(record.Header.Address);
                return AppendRecord(data, allocationStrategy, allowRecordReuse);
            }
            else
            {
                record.Data = data;
                StorageEngine.Write(record.Header.Address, record.AsBytes());
                return record;
            }
        }

        public void RegisterEmpty(Int64 address)
        {            
            if(!_reusedRecords.ContainsKey(address)){         
                _recordsToRegisterEmptyRecordIndex.Add (address);    
            }else{
                _reusedRecords.Remove(address);
            }
        }

        public void RegisterNamedRecordAddress(string name, 
                                               Int64 recordAddress, 
                                               bool allowRecordReuse = true)
        {
            var namedRecordIndex = GetNamedRecordIndex();
            if (!namedRecordIndex.NamedRecordIndexes.ContainsKey(name)
               || namedRecordIndex.NamedRecordIndexes[name] != recordAddress)
            {
                var namedRecordIndexRecord = GetNamedRecordIndexRecord();
                namedRecordIndex.NamedRecordIndexes.Remove(name);
                namedRecordIndex.NamedRecordIndexes.Add(name, recordAddress);
                var allocationStrategy = this.Session.AllocationStrategyResolver.StrategyFor(namedRecordIndex);
                var updatedRecord = UpdateRecord(
                    namedRecordIndexRecord,
                    this.Session.SerializerResolver.SerializerFor<NamedRecordsIndex>().Serialize(namedRecordIndex),
                    allocationStrategy,
                    allowRecordReuse);

                this.Root.NamedRecordIndexAddress = updatedRecord.Header.Address;
            }
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
            UpdateEmptyRecordIndex();
            SaveCollectionRoot ();
            _recordReuseEnabled = true;
            _emptyRecordIndex = null;
            _reusedRecords.Clear ();
            _recordsToRegisterEmptyRecordIndex.Clear();
        }

        public void RollbackState()
        {
            _recordReuseEnabled = true;
            _root = null;
            _rootRecord = null;
            _emptyRecordIndex = null;
            _namedRecordIndex = null;
            _recordsToRegisterEmptyRecordIndex.Clear();
            _reusedRecords.Clear();
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

        NamedRecordsIndex _namedRecordIndex;
        NamedRecordsIndex GetNamedRecordIndex()
        {
            if (_namedRecordIndex == null)
            {
                _namedRecordIndex = this.Session.SerializerResolver.SerializerFor<NamedRecordsIndex>()
                    .Deserialize(GetNamedRecordIndexRecord().Data);
            }
            return _namedRecordIndex;

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
                        Session.SerializerResolver.SerializerFor<NamedRecordsIndex> ().Serialize (namedRecordIndex),
                        allocationStrategy,
                        false);

                this.Root.NamedRecordIndexAddress =
                    namedRecordIndexRecord.Header.Address;
            }
        }

        Record ReuseEmptyRecord(int minimumLength, int maximumLength)
        {
            if(!_recordReuseEnabled){
                return null;
            }
            var walker = this.EmptyRecordIndex.GetWalker();

            var range = new BTreeWalkerRange<EmptyRecordIndexKey>();
            range.SetStartAt(new EmptyRecordIndexKey{S = minimumLength, A = 0});
            range.SetEndAt (new EmptyRecordIndexKey { S = maximumLength, A = Int64.MaxValue });
            walker.SetRange(range);

            var entry = walker.Next();
            while(entry != null && _reusedRecords.ContainsKey(entry.Pointer)){
                entry = walker.Next ();
            }
            if (entry != null)
            {
                _reusedRecords[entry.Pointer] = entry.Key;
                return GetRecord (entry.Pointer);
            } 
            return null;
        }

        void UpdateEmptyRecordIndex(){
            // Reusing records may cause nodes from the empty record index to be recycled.
            // If this happens, the _recordToRecycle list contains new records.
            // The other direction is also possible, a recycled record may cause 
            // a new empty record index node to reuse an old one.
            // In the second round, we avoid record reuse while registering recycled records.
            // This way, an infinite loop is avoided
            while(!(_reusedRecords.Count == 0 && _recordsToRegisterEmptyRecordIndex.Count == 0)){
                RegisterRecycledRecordsInEmptyRecordIndex();
                UnRegisterReusedRecordsFromEmptyRecordIndex();
                _recordReuseEnabled = false;
            }
           
        }

        void UnRegisterReusedRecordsFromEmptyRecordIndex()
        {
            var keys = _reusedRecords.Values.ToArray();
            _reusedRecords.Clear ();

            foreach(var key in keys){                
                this.EmptyRecordIndex.UnRegister(key);                   
            }
        }

        void RegisterRecycledRecordsInEmptyRecordIndex()
        {
            var recyclingRecords = _recordsToRegisterEmptyRecordIndex.ToArray();

            if (recyclingRecords.Length > 0) {
                _recordsToRegisterEmptyRecordIndex.Clear ();

                foreach (var address in recyclingRecords) {
                    var recordHeader = ReadRecordHeader (address);

                    var emptyRecordIndexKey = new EmptyRecordIndexKey {
                        A = recordHeader.Address, S = recordHeader.AllocatedDataSize
                    };

                    this.EmptyRecordIndex.Register (
                        emptyRecordIndexKey,
                        recordHeader.Address);
                }

                recyclingRecords = _recordsToRegisterEmptyRecordIndex.ToArray ();
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
            //Root record may become dirty while saving it. (Head may get updated)
            while(this.Root.IsDirty)
            {
                this.Root.Clean();
                byte[] data = this.Session.SerializerResolver.SerializerFor<CollectionFileRoot>()
                    .Serialize(this.Root);

                var allocationStrategy = this.Session.AllocationStrategyResolver.StrategyFor(this.Root);
                if (_rootRecord == null)
                {
                    _rootRecord = this.AppendRecord(data, allocationStrategy, false);
                }
                else
                {
                    _rootRecord = this.UpdateRecord (_rootRecord, data, allocationStrategy, false);
                }

                WriteRootAddress(_rootRecord.Header.Address);
            }

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

