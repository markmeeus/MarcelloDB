using System;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using System.Linq;
using System.Threading.Tasks;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Records;
using MarcelloDB.Storage;
using MarcelloDB.Index;
using MarcelloDB.Transactions.__;

namespace MarcelloDB.Collections
{
    public class Collection{}

    public class Collection<T> : Collection
    {
        Marcello Session { get; set; }

        IObjectSerializer<T> Serializer { get; set; }

        IAllocationStrategy AllocationStrategy { get; set;}

        StorageEngine StorageEngine {get;set;}

        RecordManager RecordManager { get; set; }        		

        internal Collection (Marcello session, 
            IObjectSerializer<T> serializer,
            IAllocationStrategy allocationStrategy,
            StorageEngine storageEngine)
        {
            Session = session;
            AllocationStrategy = allocationStrategy; 
            Serializer = serializer;
            StorageEngine = storageEngine;

            RecordManager = new RecordManager(
                new DoubleSizeAllocationStrategy(),
                StorageEngine
            );                        
        }

        public IEnumerable<T> All
        {
            get{
                return new CollectionEnumerator<T>(
                    this.Session, RecordManager, Serializer);
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
                PersistInternal(obj);                
            });               
        }            

        public void Destroy(T obj)
        {
            Transacted(() => {
                DestroyInternal(obj);
            });
        }

        internal void DestroyAll()
        {
            var toDestroy = All.ToList();
            foreach(var o in toDestroy)
            {
                DestroyInternal(o);
            }        
        }            
            
        void Transacted(Action action)
        {
            this.Session.Transaction(() =>
                {
                    this.Session.CurrentTransaction.AddTransactor(this.RecordManager);
                    action();
                });
        }

        Record GetRecordForObjectID(object objectID)
        {                
            var index = RecordIndex.Create(this.RecordManager, 
                RecordIndex.ID_INDEX_NAME);
            var address = index.Search(objectID);
            if (address > 0)
            {
                return RecordManager.GetRecord(address);
            }
            return null;
        }
            
        void PersistInternal(T obj)
        {
            var objectID = new ObjectProxy(obj).ID;
            //Try to load the record with object ID
            Record record = GetRecordForObjectID(objectID);
            if (record != null) {
                record = UpdateObject(record, obj);
            }
            else {
                record = AppendObject(obj);
            }
                
            var index = RecordIndex.Create(
                this.RecordManager, RecordIndex.ID_INDEX_NAME);
            index.Register(objectID, record.Header.Address);
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
            var objectID = new ObjectProxy(obj).ID;
            //Try to load the record with object ID
            Record record = GetRecordForObjectID(objectID);
            if (record != null)
            {
                var index = RecordIndex.Create(
                    this.RecordManager, RecordIndex.ID_INDEX_NAME);
                index.UnRegister(objectID);

                this.RecordManager.Recycle(record.Header.Address);
            }
        }
    }
}