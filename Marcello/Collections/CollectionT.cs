using System;
using System.Collections.Generic;
using Marcello.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace Marcello
{
    public class Collection<T>
    {
        Marcello Session { get; set; }

        IStorageEngine StorageEngine{ get; set;}

        IObjectSerializer<T> Serializer { get; set; }

        CollectionMetaData<T> CollectionMetaData { get: set; }

        RecordManager _recordManager;
        RecordManager RecordManager 
        { 
            get
            {
                if (_recordManager == null) {
                    _recordManager = new RecordManager (Session);
                }
                return _recordManager;
            }  
        }			

        internal Collection (Marcello session, IStorageEngine storageEngine, IObjectSerializer<T> serializer)
        {
            Session = session;
            StorageEngine = storageEngine;
            Serializer = serializer;
        }

        public IEnumerable<T> All{
            get{
                return new CollectionEnumerator<T>(RecordManager);
            }
        }

        public void Persist(T obj)
        {
            var objectID = new ObjectProxy(obj).ID;
            var record = All.Where (o => new ObjectProxy (o).ID == objectID).FirstOrDefault ();
            if (record != null) 
            {
                UpdateObject (record, obj);
            }
            else 
            {
                AppendObject (obj);
            }   
        }

        public void Destroy(T obj)
        {
        }
            
        #region private methods
        void AppendObject(object obj)
        {
            var bytes = Serializer.Serialize(obj);
            var record = new Record ();

            var lastRecord = RecordManager.GetLastRecord ();
            if (lastRecord != null) 
            {
                record.Header.Address = lastRecord.Header.Address + lastRecord.Header.AllocatedSize;
                lastRecord.Header.Next = record.Header.Address;			
                WriteHeader (lastRecord);
            } 
	
            StorageEngine.Write(record.Header.Address, record.AsBytes());
            SetLastRecord (record);
        }
            
        void UpdateObject(Record record, T obj)
        {
            var bytes = Serializer.Serialize (obj);
            if (bytes.Length > record.Header.AllocatedSize) 
            {
                ReleaseRecord (record);
                AppendObject (obj); 
            }
        }

        void ReleaseRecord(Record record)
        {
            var previousRecord = RecordManager.GetPreviousRecord(record);
            var nextRecord = RecordManager.GetNextRecord (record);

            if (previousRecord != null && nextRecord != null) 
            {
                //record in the middle, link previous to next and vice versa
                nextRecord.Header.Previous = previousRecord.Header.Address;
                previousRecord.Header.Next = nextRecord.Header.Address;
            }

            if (previousRecord != null && nextRecord == null) 
            {
                //no next record, so this was the last one
                previousRecord.Header.Next = 0;
                SetLastRecord (previousRecord);
            }

            if (previousRecord == null && nextRecord != null) 
            {
                //first record
                nextRecord.Header.Previous = 0;
                SetFirstRecord (nextRecord);
            }
				
            if (previousRecord == null && nextRecord == null) 
            {
                //this was the last one
                SetEmpty ();
            }

            if (nextRecord != null) 
            {
                WriteHeader (nextRecord);
            }
				
            if (previousRecord != null) 
            {
                WriteHeader (previousRecord);
            }
        }
			
        void WriteHeader(Record record)
        {
            StorageEngine.Write (record.Header.Address, record.Header.AsBytes);
        }

        void SetFirstRecord(Record record)
        {
            UpdateMetaData (r => r.FirstRecordAddress = record.Header.Address);
        }

        void SetLastRecord(Record record)
        {
            UpdateMetaData (r => r.LastRecordAddress = record.Header.Address);
        }

        void SetEmpty()
           {
        	UpdateMetaData ((r) => {
        		r.LastRecordAddress = 0;
        		r.FirstRecordAddress = 0; 
        	});
        }
            
        void UpdateMetaData(Action<CollectionMetaDataRecord> updateAction)
        {
            var metaData = CollectionMetaData.GetRecord ();
            updateAction (metaData);
            CollectionMetaData.Update (metaData);
        }
    #endregion
    }
}

