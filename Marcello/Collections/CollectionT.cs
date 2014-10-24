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

        IObjectSerializer<T> Serializer { get; set; }

        IAllocationStrategy AllocationStrategy { get; set;}

        RecordManager<T> RecordManager { get; set; }        		

        internal Collection (Marcello session, 
            IObjectSerializer<T> serializer,
            IAllocationStrategy allocationStrategy)
        {
            Session = session;
            AllocationStrategy = allocationStrategy; 
            Serializer = serializer;

            RecordManager = new RecordManager<T> (Session, 
                new StorageEngine<T>(Session.StreamProvider),
                serializer,
                new DoubleSizeAllocationStrategy()
            );

        }

        public IEnumerable<T> All{
            get{
                return new CollectionEnumerator<T>(RecordManager, Serializer);
            }
        }

        public void Persist(T obj)
        {
            var objectID = new ObjectProxy(obj).ID;
            //Try Load record with object ID
            Record record = null; 
            if (record != null) 
            {
                RecordManager.UpdateObject (record, obj);
            }
            else 
            {
                RecordManager.AppendObject (obj);
            }   
        }

        public void Destroy(T obj)
        {
        }            
    }
}

