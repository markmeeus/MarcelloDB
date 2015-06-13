using System;
using System.Collections.Generic;
using MarcelloDB;
using MarcelloDB.Serialization;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Storage;
using MarcelloDB.Records;
using MarcelloDB.Transactions;

namespace MarcelloDB.Collections
{
    public class CollectionFile : ITransactor
    {
        Session Session { get; set; }

        StorageEngine StorageEngine { get; set; }

        RecordManager RecordManager { get; set; }

        string Name { get; set; }

        Dictionary<string, Collection> Collections { get; set; }

        internal CollectionFileRoot Root { get; set; }

        Record RootRecord { get; set; }

        internal CollectionFile(Session session, string name)
        {
            this.Session = session;
            this.Name = name;
            Collections = new Dictionary<string, Collection>();
            this.StorageEngine = new StorageEngine(this.Session, this.Name);
            this.RecordManager = new RecordManager(
                this,
                new DoubleSizeAllocationStrategy(),
                this.StorageEngine
            );
            LoadCollectionFileRoot();
        }

        public Collection<T> Collection<T>(string collectionName)
        {
            if (collectionName == null)
            {
                collectionName = typeof(T).Name.ToLower();
            }
            if(!Collections.ContainsKey(collectionName)){
                Collections.Add (collectionName,
                    new Collection<T> (this.Session,
                        this,
                        collectionName,
                        new BsonSerializer<T>(),
                        new DoubleSizeAllocationStrategy(),
                        this.RecordManager)
                    );
            }

            var retVal = Collections[collectionName] as Collection<T>;
            if (retVal == null)
            {
                ThrowCollectionDefinedForOtherType<T>(collectionName);
            }
            return (Collection<T>)Collections[collectionName];

        }

        void ThrowCollectionDefinedForOtherType<T>(string collectionName)
        {
            throw new InvalidOperationException(
                string.Format("Collection with name \"{0}\" is allready defined as Collection<{1}>" +
                    " and cannot be used as a Collection<{2}>.",
                    collectionName,
                    Collections[collectionName].GetType().GenericTypeArguments[0].Name,
                    typeof(T).Name
                )
            );
        }

        void LoadCollectionFileRoot(){
            var rootAddress = ReadRootAddress();
            if (rootAddress > 0)
            {
                this.RootRecord = RecordManager.GetRecord(rootAddress);
                this.Root = CollectionFileRoot.Deserialize(this.RootRecord.Data);
            }
            else
            {
                this.Root = new CollectionFileRoot();
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

        #region ITransactor implementation

        public void SaveState()
        {
            if (this.Root.IsDirty)
            {
                byte[] data = this.Root.Serialize();
                if (this.RootRecord == null)
                {
                    this.RootRecord = this.RecordManager.AppendRecord(data, true);
                }
                else
                {
                    this.RootRecord = this.RecordManager.UpdateRecord(this.RootRecord, data, true);
                }

                WriteRootAddress(this.RootRecord.Header.Address);
            }
        }

        public void RollbackState()
        {
            LoadCollectionFileRoot();
        }

        #endregion
    }
}

