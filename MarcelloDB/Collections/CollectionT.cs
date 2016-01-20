using System;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Records;
using MarcelloDB.Storage;
using MarcelloDB.Index;
using MarcelloDB.Transactions;
using MarcelloDB.Exceptions;

namespace MarcelloDB.Collections
{
    public class Collection : SessionBoundObject
    {
        internal  Collection(Session session) : base(session){}
    }

    public class Collection<T, TIndexDef> : Collection<T> where TIndexDef : new(){
        internal Collection (Session session,
            CollectionFile collectionFile,
            string name,
            IObjectSerializer<T> serializer,
            RecordManager recordManager) :
        base(session, collectionFile, name, serializer, recordManager){

        }

        public IndexAccessor<T, TIndexKey> Index<TIndexKey>(Expression<Func<TIndexDef, TIndexKey>> keyMap)
        {
            var memberExpression = keyMap.Body as MemberExpression;
            if (memberExpression != null)
            {
                return new IndexAccessor<T, TIndexKey>(
                    this, this.Session, this.RecordManager, this.Serializer, memberExpression.Member.Name);
            }
            return null;
        }

        internal override List<IndexedFieldDescriptor> GetIndexedFieldDescriptors()
        {
            return IndexDefinition.GetIndexedFieldDescriptors<TIndexDef, T>();
        }
    }

    public class Collection<T> : Collection
    {
        public string Name { get; set; }

        internal bool BlockModification { get; set; }

        CollectionFile CollectionFile { get; set; }

        internal IObjectSerializer<T> Serializer { get; set; }

        internal RecordManager RecordManager { get; set; }

        internal Collection (Session session,
            CollectionFile collectionFile,
            string name,
            IObjectSerializer<T> serializer,
            RecordManager recordManager) : base(session)
        {
            this.CollectionFile = collectionFile;
            this.Name = name;
            this.Serializer = serializer;
            this.RecordManager = recordManager;
        }

        public IEnumerable<T> All
        {
            get{
                return new CollectionEnumerator<T>(
                    this, Session, RecordManager, Serializer, this.GetIDIndexedFieldDescriptor().Name);
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

        public void Destroy(object objectID)
        {
            EnsureModificationIsAllowed();
            Transacted(() => {
                DestroyInternal(objectID);
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
            var index = GetIDIndex();
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

            Record record = GetRecordForObjectID(objectID);
            if (record != null)
            {
                var originalAddress = record.Header.Address;
                record = UpdateObject(record, obj);
                if (record.Header.Address != originalAddress)
                {
                    //object moved, register it's adress in the index
                    RegisterInIndexes(obj, record, true);
                }
            }
            else
            {
                record = AppendObject(obj);
                RegisterInIndexes(obj, record);
            }
        }

        Record AppendObject(T obj)
        {
            var data = Serializer.Serialize(obj);

            return RecordManager.AppendRecord(data, this.Session.AllocationStrategyResolver.StrategyFor(obj));
        }

        Record UpdateObject(Record record, T obj)
        {
            var bytes = Serializer.Serialize(obj);
            return RecordManager.UpdateRecord(record, bytes, this.Session.AllocationStrategyResolver.StrategyFor(obj));
        }

        void DestroyInternal (object objectID)
        {
            //Try to load the record with object ID
            Record record = GetRecordForObjectID(objectID);
            if (record != null)
            {
                var o = Serializer.Deserialize(record.Data);
                UnRegisterInIndexes(o, record);
                this.RecordManager.Recycle(record.Header.Address);
            }
            else
            {
                throw new ObjectNotFoundException("Object not found: " + objectID.ToString());
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

        IndexedFieldDescriptor GetIDIndexedFieldDescriptor(){
            return GetIndexedFieldDescriptors().First(d => d.IsID);
        }

        RecordIndex<object> GetIndex(string indexName)
        {
            return new RecordIndex<object>(
                this.Session,
                this.RecordManager,
                RecordIndex.GetIndexName<T>(this.Name, indexName),
                this.Session.SerializerResolver.SerializerFor<Node<object, Int64>>());
        }

        RecordIndex<object> GetIDIndex()
        {
            return GetIndex(GetIDIndexedFieldDescriptor().Name);
        }

        void RegisterInIndexes(T o, Record record, bool unregisterFirst = false)
        {
            foreach (var indexedFieldDescriptor in this.GetIndexedFieldDescriptors())
            {
                var index = GetIndex(indexedFieldDescriptor.Name);
                var value = indexedFieldDescriptor.ValueFunc(o);
                if (unregisterFirst)
                {
                    index.UnRegister(value);
                }
                index.Register(value, record.Header.Address);
            }
        }

        void UnRegisterInIndexes(T o, Record record)
        {
            foreach (var indexedFieldDescriptor in this.GetIndexedFieldDescriptors())
            {
                var index = GetIndex(indexedFieldDescriptor.Name);
                var value = indexedFieldDescriptor.ValueFunc(o);
                index.UnRegister(value);
            }
        }

        internal virtual List<IndexedFieldDescriptor> GetIndexedFieldDescriptors()
        {
            return IndexDefinition.GetIndexedFieldDescriptors<EmptyIndexDef, T>();
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