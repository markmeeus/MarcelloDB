using System;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Collections;
using MarcelloDB.Serialization;
using MarcelloDB.Transactions;
using MarcelloDB.Storage;
using System.Collections.Generic;
using System.Threading;

namespace MarcelloDB
{
    public class Marcello
    {
        internal Dictionary<Type, Collection> Collections { get; set; }

        internal IStorageStreamProvider StreamProvider { get; set; }

        internal Transaction CurrentTransaction { get; set; }

        internal Journal Journal { get; set; }

        internal object SyncLock { get; set; }

        public Marcello (IStorageStreamProvider streamProvider)
        {
            Collections = new Dictionary<Type, Collection>();
            StreamProvider = streamProvider;
            Journal = new Journal(this);
            SyncLock = new object();
        }

        public Collection<T> Collection<T>()
        {
            if(!Collections.ContainsKey(typeof(T))){
                Collections.Add (typeof(T), 
                    new Collection<T> (this, 
                        new BsonSerializer<T> (), 
                        new DoubleSizeAllocationStrategy (),
                        new StorageEngine<T>(this))
                );
            }
            return (Collection<T>)Collections[typeof(T)];
        }

        public void Transaction(Action action)
        {
            lock (this.SyncLock) {
                EnsureTransaction ();
                try {
                    CurrentTransaction.Enlist ();
                    action ();
                    CurrentTransaction.Leave ();            
                } catch (Exception exc) {
                    CurrentTransaction.Rollback ();
                    throw exc;
                } finally {
                    if (!CurrentTransaction.Running) {
                        CurrentTransaction = null;
                    }
                }
            }
        }
            
        void EnsureTransaction()
        {
            if (CurrentTransaction == null) 
            {
                CurrentTransaction = new Transaction(this);
            }
        }
    }
}

