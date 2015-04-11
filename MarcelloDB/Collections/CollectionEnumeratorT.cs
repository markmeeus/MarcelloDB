using System;
using System.Collections;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;

namespace MarcelloDB.Collections
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
                var index = RecordIndex.Create(
                    this.Session, 
                    this.RecordManager, 
                    RecordIndex.ID_INDEX_NAME);

                var walker = index.GetWalker();
                var node = walker.Next();
                while (node != null)
                {
                    var record = RecordManager.GetRecord(node.Pointer);
                    var obj = Serializer.Deserialize(record.Data);
                    yield return obj;
                    node = walker.Next();
                }
            }            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}