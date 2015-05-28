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
    public class Session
    {
        Dictionary<string, CollectionFile> CollectionFiles { get; set; }

        internal IStorageStreamProvider StreamProvider { get; set; }

        internal Transaction CurrentTransaction { get; set; }

        internal Journal Journal { get; set; }

        internal object SyncLock { get; set; }
            
        public Session (IStorageStreamProvider streamProvider)
        {
            this.CollectionFiles = new Dictionary<string, CollectionFile>();
            this.StreamProvider = streamProvider;
            this.Journal = new Journal(this);
            SyncLock = new object();
        }

        public CollectionFile this[string fileName]
        {
            get
            {
                if(!this.CollectionFiles.ContainsKey(fileName))
                {
                    this.CollectionFiles[fileName] = new CollectionFile(this, fileName);
                }
                return this.CollectionFiles[fileName];
            }
        }
        public void Transaction(Action action)
        {
            lock (this.SyncLock) {
                EnsureTransaction ();
                try {
                    CurrentTransaction.Enlist ();
                    action ();
                    CurrentTransaction.Leave ();            
                } catch (Exception) {
                    CurrentTransaction.Rollback ();
                    throw;
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

