using System;
using Marcello.AllocationStrategies;
using Marcello.Collections;
using Marcello.Serialization;
using Marcello.Transactions;
using Marcello.Storage;
using System.Collections.Generic;
using System.Threading;

namespace Marcello
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

        #region private methods
        void EnsureTransaction()
        {
            if (CurrentTransaction == null) 
            {
                CurrentTransaction = new Transaction(this);
            }
        }
        #endregion
    }
}

