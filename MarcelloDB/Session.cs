using System;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Collections;
using MarcelloDB.Serialization;
using MarcelloDB.Transactions;
using MarcelloDB.Storage;
using System.Collections.Generic;
using System.Threading;
using MarcelloDB.Platform;

namespace MarcelloDB
{
    internal class SessionBasedObject
    {
        internal Session Session { get; }

        internal SessionBasedObject(Session session){this.Session = session;}
    }

    public class Session : IDisposable
    {
        Dictionary<string, CollectionFile> CollectionFiles { get; set; }

        internal IStorageStreamProvider StreamProvider { get; set; }

        internal Transaction CurrentTransaction { get; set; }

        internal AllocationStrategyResolver AllocationStrategyResolver { get; set; }

        internal Journal Journal { get; set; }

        internal object SyncLock { get; set; }

        public Session (IPlatform platform, string rootPath)
        {
            this.CollectionFiles = new Dictionary<string, CollectionFile>();
            this.StreamProvider = platform.CreateStorageStreamProvider(rootPath);
            this.Journal = new Journal(this);
            this.AllocationStrategyResolver = new AllocationStrategyResolver();
            SyncLock = new object();
        }

        public CollectionFile this[string fileName]
        {
            get
            {
                if (!IsValidCollectionFileName(fileName))
                {
                    throw new ArgumentException(string.Format("{0} cannot be used as collection file.", fileName));
                }
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

        public void Dispose()
        {
            this.StreamProvider.Dispose();
        }

        void EnsureTransaction()
        {
            if (CurrentTransaction == null)
            {
                CurrentTransaction = new Transaction(this);
            }
        }

        private bool IsValidCollectionFileName(string fileName)
        {
            return fileName.ToLower() != Journal.JOURNAL_COLLECTION_NAME.ToLower();
        }
    }
}

