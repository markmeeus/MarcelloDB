using System;
using System.Collections.Generic;
using Marcello.Serialization;
using System.Linq;
using System.Threading.Tasks;
using Marcello.AllocationStrategies;
using Marcello.Records;
using Marcello.Storage;

namespace Marcello.Collections
{
    public class Collection{}

    public class Collection<T> : Collection
    {
        internal StorageEngine<T> StorageEngine {get;set;}

        internal RecordManager<T> RecordManager { get; set; }                

        Marcello Session { get; set; }

        IObjectSerializer<T> Serializer { get; set; }

        IAllocationStrategy AllocationStrategy { get; set;}

        internal Collection (Marcello session, 
            IObjectSerializer<T> serializer,
            IAllocationStrategy allocationStrategy,
            StorageEngine<T> storageEngine)
        {
            Session = session;
            AllocationStrategy = allocationStrategy; 
            Serializer = serializer;
            StorageEngine = storageEngine;

            RecordManager = new RecordManager<T>(
                Session,
                serializer,
                new DoubleSizeAllocationStrategy(),
                StorageEngine
            );

        }

        public IEnumerable<T> All
        {
            get{
                return new CollectionEnumerator<T>(this.Session, RecordManager, Serializer);
            }
        }            

        public void Persist(T obj)
        {
            this.Session.Transaction(() => {
                PersistInternal(obj);                
            });               
        }            

        public void Destroy(T obj)
        {
            this.Session.Transaction(() => {
                DestroyInternal(obj);
            });
        }

        #region internal methods
        internal void DestroyAll()
        {
            var toDestroy = All.ToList();
            foreach(var o in toDestroy)
            {
                Destroy(o);
            }        
        }

        internal void DisableJournal()
        {
            this.RecordManager.DisableJournal();
        }
        #endregion

        #region private methods
        Record GetRecordForObjectID(object objectID)
        {        
            //temp implementation untill ID is indexed
            var record = RecordManager.GetFirstRecord();

            while (record != null) {
                var obj = Serializer.Deserialize(record.Data);
                var objProxy = new ObjectProxy(obj);
                if (objProxy.ID.Equals(objectID)){
                    return record;
                }
                record = RecordManager.GetNextRecord(record);
            }
            return null;
        }

        void PersistInternal(T obj)
        {
            var objectID = new ObjectProxy(obj).ID;
            //Try Load record with object ID
            Record record = GetRecordForObjectID(objectID);
            if (record != null) {
                RecordManager.UpdateObject(record, obj);
            }
            else {
                RecordManager.AppendObject(obj);
            }
        }

        void DestroyInternal (T obj)
        {
            var objectID = new ObjectProxy(obj).ID;
            //Try Load record with object ID
            Record record = GetRecordForObjectID(objectID);
            if (record != null)
            {
                //release the record if present
                RecordManager.ReleaseRecord(record);
            }

        }
        #endregion
    }
}