using System;
using System.Collections;
using System.Collections.Generic;
using Marcello.Records;
using Marcello.Serialization;

namespace Marcello.Collections
{
	internal class CollectionEnumerator<T> : IEnumerable<T>
	{
        RecordManager<T> RecordManager  { get; set; }

        IObjectSerializer<T> Serializer { get; set; }

        Marcello Session { get; set; }

        public CollectionEnumerator(
            Marcello session,
            RecordManager<T> recordManager, 
            IObjectSerializer<T> serializer)
        {   
            Session = session;
            RecordManager = recordManager;
            Serializer = serializer;
        }
            
        public IEnumerator<T> GetEnumerator()
        {
            lock(this.Session.SyncLock){
                var record = RecordManager.GetFirstRecord();
                while (record != null) {
                    if (record.Header.HasObject)
                    {   
                        var obj = Serializer.Deserialize(record.Data);
                        yield return obj;
                    }
                    record = RecordManager.GetNextRecord(record);
                }
            }            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}