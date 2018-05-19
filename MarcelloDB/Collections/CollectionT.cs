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
        public string Name { get; private set; }

        internal  Collection(Session session, string name) : base(session)
        {
            this.Name = name;
        }

        internal bool BlockModification { get; set; }

        internal void EnsureModificationIsAllowed()
        {
            if (this.BlockModification)
            {
                throw new InvalidOperationException("Cannot modify a collection while it is being enumerated");
            }
        }
    }

    internal class EmptyIndexDefinition<T>: IndexDefinition<T>
    {
    }

    public class Collection<T, TID, TIndexDef> : Collection<T, TID> where TIndexDef : IndexDefinition<T>, new()
    {
        public TIndexDef Indexes { get; internal set; }

        internal Collection (Session session,
            CollectionFile collectionFile,
            string name,
            IObjectSerializer<T> serializer,
            RecordManager recordManager,
            Func<T, TID> idFunc) :
        base(session, collectionFile, name, serializer, recordManager, idFunc)
        {
            IndexDefinitionValidator.Validate<T, TIndexDef>();

            this.Indexes = new TIndexDef();
            this.Indexes.Initialize();
            this.Indexes.IndexedValues.Add(
                new IndexedIDValue<T, TID>
                {
                    IDValueFunction = idFunc
                });

            this.Indexes.SetContext(this, session, recordManager, serializer);
        }

        internal override IndexDefinition<T> GetIndexDefinition()
        {
            return this.Indexes;
        }
    }

    public class Collection<T, TID> : Collection
    {
        CollectionFile CollectionFile { get; set; }

        internal IObjectSerializer<T> Serializer { get; set; }

        internal RecordManager RecordManager { get; set; }

        internal EmptyIndexDefinition<T> EmptyIndexDefinition { get; set; }

        internal Func<T, TID> IDFunc { get; set; }

        internal Collection (Session session,
            CollectionFile collectionFile,
            string name,
            IObjectSerializer<T> serializer,
            RecordManager recordManager,
            Func<T, TID> idFunc) : base(session, name)
        {
            this.CollectionFile = collectionFile;
            this.Serializer = serializer;
            this.RecordManager = recordManager;

            this.EmptyIndexDefinition = new EmptyIndexDefinition<T>();
            this.EmptyIndexDefinition.Initialize();
            this.EmptyIndexDefinition.IndexedValues.Add(
                new IndexedIDValue<T, TID>
                {
                    IDValueFunction = idFunc
                });

            this.EmptyIndexDefinition.SetContext(this, session, recordManager, serializer);

            this.IDFunc = idFunc;
        }

        public IEnumerable<T> All
        {
            get
            {
                return new CollectionEnumerator<T, TID>
                    (this, Session, RecordManager, Serializer, GetIDIndexName());
            }
        }

        public T Find(TID id)
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

        public IEnumerable<T> Find(IEnumerable<TID> ids)
        {
            return new ObjectByIDEnumerator<T, TID>(this, ids.Distinct());
        }

        public void Persist(T obj)
        {
            Transacted(() => {
                EnsureModificationIsAllowed();
                PersistInternal(obj);
            });
        }

        public void Destroy(TID objectID)
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

        Record GetRecordForObjectID(TID objectID)
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

        void DestroyInternal (TID objectID)
        {
            //Try to load the record with object ID
            Record record = GetRecordForObjectID(objectID);
            if (record != null)
            {
                var o = Serializer.Deserialize(record.Data);
                UnRegisterInIndexes(o, record);
                this.RecordManager.RegisterEmpty(record.Header.Address);
            }
            else
            {
                throw new ObjectNotFoundException("Object not found: " + objectID.ToString());
            }
        }

        TID GetObjectIDOrThrow(T obj)
        {
            return this.IDFunc(obj);
        }

        IndexedIDValue<T, TID> GetIDIndexedValue()
        {
            return (IndexedIDValue<T, TID>) GetIndexDefinition().IndexedValues.First(v => v is IndexedIDValue<T, TID>);
        }

        string GetIDIndexName()
        {
            return RecordIndex.GetIndexName<T>(
                this.Name, this.GetIDIndexedValue().PropertyName, typeof(TID));
        }

        RecordIndex<TID> GetIDIndex()
        {
            return new RecordIndex<TID>(
                this.Session,
                this.RecordManager,
                GetIDIndexName(),
                this.Session.SerializerResolver.SerializerFor<Node<TID>>());
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

        void AddTransactors()
        {
            this.Session.CurrentTransaction.AddTransactor(this.RecordManager);
        }

        internal void Initialize ()
        {
            foreach(var indexedValue in this.GetIndexDefinition().IndexedValues){
                indexedValue.EnsureIndex();
            }
        }
    }
}