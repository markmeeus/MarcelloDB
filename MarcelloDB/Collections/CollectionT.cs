using System;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using System.Linq;
using System.Threading.Tasks;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Records;
using MarcelloDB.Storage;
using MarcelloDB.Index;
using MarcelloDB.Transactions;
using MarcelloDB.Exceptions;

namespace MarcelloDB.Collections
{
    public class Collection{}

    public class Collection<T> : Collection
    {
        public string Name { get; set; }

        internal bool BlockModification { get; set; }

        Session Session { get; set; }

        CollectionFile File { get; set; }

        IObjectSerializer<T> Serializer { get; set; }

        IAllocationStrategy AllocationStrategy { get; set;}

        RecordManager RecordManager { get; set; }

        internal Collection (Session session,
            CollectionFile file,
            string name,
            IObjectSerializer<T> serializer,
            IAllocationStrategy allocationStrategy,
            RecordManager recordManager)
        {
            this.Session = session;
            this.File = file;
            this.Name = name;
            this.AllocationStrategy = allocationStrategy;
            this.Serializer = serializer;
            this.RecordManager = recordManager;
        }

        public IEnumerable<T> All
        {
            get{
                return new CollectionEnumerator<T>(
                    this, Session, RecordManager, Serializer);
            }
        }

        public T Find(object id)
        {
            T result = default(T);

            Transacted(() => {
                var record =  GetRecordForObjectID(id);
                if (record != null)
                {
                    result = Serializer.Deserialize(record.Data);
                }
            });
            return result;
        }

        public void Persist(T obj)
        {
            EnsureModificationIsAllowed();
            Transacted(() => {
                PersistInternal(obj);
            });
        }

        public void Destroy(T obj)
        {
            EnsureModificationIsAllowed();
            Transacted(() => {
                DestroyInternal(obj);
            });
        }

        void Transacted(Action action)
        {
            this.Session.Transaction(() =>
                {
                    AddTransactors();
                    action();
                });
        }

        Record GetRecordForObjectID(object objectID)
        {
            var index = RecordIndex.Create<object>(
                this.RecordManager,
                RecordIndex.GetIDIndexName<T>(this.Name),
                new BsonSerializer<Node<object, Int64>>());
            var address = index.Search(objectID);
            if (address > 0)
            {
                return RecordManager.GetRecord(address);
            }
            return null;
        }

        void PersistInternal(T obj)
        {
            var objectID = GetObjectIDOrThrow(obj);

            var index = RecordIndex.Create<object>(
                this.RecordManager, 
                RecordIndex.GetIDIndexName<T>(this.Name),
                new BsonSerializer<Node<object, Int64>>());

            Record record = GetRecordForObjectID(objectID);
            if (record != null)
            {
                var originalAddress = record.Header.Address;
                record = UpdateObject(record, obj);
                if (record.Header.Address != originalAddress)
                {
                    //object moved, register it's adress in the index
                    index.UnRegister(objectID);
                    index.Register(objectID, record.Header.Address);
                }
            }
            else
            {
                record = AppendObject(obj);
                index.Register(objectID, record.Header.Address);
            }
        }

        Record AppendObject(T obj)
        {
            var data = Serializer.Serialize(obj);

            return RecordManager.AppendRecord(data, true);
        }

        Record UpdateObject(Record record, T obj)
        {
            var bytes = Serializer.Serialize(obj);
            return RecordManager.UpdateRecord(record, bytes);
        }

        void DestroyInternal (T obj)
        {
            var objectID = GetObjectIDOrThrow(obj);
            //Try to load the record with object ID
            Record record = GetRecordForObjectID(objectID);
            if (record != null)
            {
                var index = RecordIndex.Create<object>(
                    this.RecordManager, 
                    RecordIndex.GetIDIndexName<T>(this.Name),
                    new BsonSerializer<Node<object, Int64>>());
                index.UnRegister(objectID);

                this.RecordManager.Recycle(record.Header.Address);
            }
        }

        object GetObjectIDOrThrow(T obj){
            var objectID = new ObjectProxy<T>(obj).ID;
            if(objectID == null){
                throw new IDMissingException(obj.GetType().Name +
                    " either has no ID property, or the property returned null");
            }
            return objectID;
        }

        void EnsureModificationIsAllowed()
        {
            if (this.BlockModification)
            {
                throw new InvalidOperationException("Cannot modify a collection while it is being enumerated");
            }
        }

        void AddTransactors()
        {
            this.Session.CurrentTransaction.AddTransactor(this.RecordManager);
        }
    }
}