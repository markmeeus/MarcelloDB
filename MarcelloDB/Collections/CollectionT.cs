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
using System.Reflection;

namespace MarcelloDB.Collections
{
    public class Collection : SessionBoundObject
    {
        internal  Collection(Session session) : base(session){}
    }

    internal class EmptyIndexDefinition<T>: IndexDefinition<T>
    {
    }

    public class Collection<T, TIndexDef> : Collection<T> where TIndexDef : IndexDefinition<T>, new()
    {
        public TIndexDef Indexes { get; private set; }

        internal Collection (Session session,
            CollectionFile collectionFile,
            string name,
            IObjectSerializer<T> serializer,
            RecordManager recordManager) :
        base(session, collectionFile, name, serializer, recordManager)
        {
            IndexDefinitionValidator.Validate<T, TIndexDef>();

            this.Indexes = new TIndexDef();
            this.Indexes.Initialize();
            this.Indexes.SetContext(this, session, recordManager, serializer);
        }

        internal override IndexDefinition<T> GetIndexDefinition()
        {
            return this.Indexes;
        }
    }

    public class Collection<T> : Collection
    {
        public string Name { get; set; }

        internal bool BlockModification { get; set; }

        CollectionFile CollectionFile { get; set; }

        internal IObjectSerializer<T> Serializer { get; set; }

        internal RecordManager RecordManager { get; set; }

        internal EmptyIndexDefinition<T> EmptyIndexDefinition { get; set; }

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

            this.EmptyIndexDefinition = new EmptyIndexDefinition<T>();
            this.EmptyIndexDefinition.Initialize();
            this.EmptyIndexDefinition.SetContext(this, session, recordManager, serializer);
        }

        public IEnumerable<T> All
        {
            get
            {
                var indexName =
                    RecordIndex.GetIndexName<T>(this.Name, this.GetIDIndexedValue().PropertyName);

                return new CollectionEnumerator<T, object>
                    (this, Session, RecordManager, Serializer, indexName);
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
            Transacted(() => {
                EnsureModificationIsAllowed();
                PersistInternal(obj);
            });
        }

        public void Destroy(object objectID)
        {
            Transacted(() => {
                EnsureModificationIsAllowed();
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
                var originalObject = Serializer.Deserialize(record.Data);

                record = UpdateObject(record, obj);

                //update all indexes
                RegisterInIndexes(obj, record, originalAddress, originalObject);
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

        object GetObjectIDOrThrow(T obj)
        {
            var objectID = new ObjectProxy<T>(obj).ID;
            if(objectID == null){
                throw new IDMissingException(obj.GetType().Name +
                    " either has no ID property, or the property returned null");
            }
            return objectID;
        }

        IndexedIDValue<T> GetIDIndexedValue()
        {
            return (IndexedIDValue<T>) GetIndexDefinition().IndexedValues.First(v => v is IndexedIDValue<T>);
        }

        RecordIndex<object> GetIndex(string indexName)
        {
            return new RecordIndex<object>(
                this.Session,
                this.RecordManager,
                RecordIndex.GetIndexName<T>(this.Name, indexName),
                this.Session.SerializerResolver.SerializerFor<Node<object>>());
        }

        RecordIndex<object> GetIDIndex()
        {
            return GetIndex(GetIDIndexedValue().PropertyName);
        }

        void RegisterInIndexes(T o, Record record, Int64 originalAddress = 0, T originalObject = default(T))
        {
            foreach (var indexedValue in this.GetIndexDefinition().IndexedValues)
            {
                if (originalAddress > 0)
                {
                    indexedValue.UnRegister(originalObject, originalAddress);
                }
                indexedValue.Register(o, record.Header.Address);
            }
        }

        void UnRegisterInIndexes(T o, Record record)
        {
            foreach (var indexedValue in this.GetIndexDefinition().IndexedValues)
            {
                indexedValue.UnRegister(o, record.Header.Address);
            }
        }

        internal virtual IndexDefinition<T> GetIndexDefinition()
        {
            return this.EmptyIndexDefinition;
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