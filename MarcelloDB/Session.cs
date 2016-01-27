using System;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Collections;
using MarcelloDB.Serialization;
using MarcelloDB.Transactions;
using MarcelloDB.Storage;
using System.Collections.Generic;
using System.Threading;
using MarcelloDB.Platform;
using MarcelloDB.Records;
using MarcelloDB.Index;

namespace MarcelloDB
{
    public class SessionBoundObject
    {
        internal Session Session { get; set;}

        internal SessionBoundObject(Session session){
            this.Session = session;
        }
    }

    public class Session : IDisposable
    {
        Dictionary<string, CollectionFile> CollectionFiles { get; set; }

        internal IStorageStreamProvider StreamProvider { get; set; }

        internal Transaction CurrentTransaction { get; set; }

        internal AllocationStrategyResolver AllocationStrategyResolver { get; set; }

        internal SerializerResolver SerializerResolver { get; set; }

        internal Journal Journal { get; set; }

        internal object SyncLock { get; set; }

        bool Disposed { get; set; }

        public Session (IPlatform platform, string rootPath)
        {
            this.CollectionFiles = new Dictionary<string, CollectionFile>();
            this.StreamProvider = platform.CreateStorageStreamProvider(rootPath);
            this.Journal = new Journal(this);
            this.AllocationStrategyResolver = new AllocationStrategyResolver();
            this.SerializerResolver = new SerializerResolver();
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
                this.AssertValid();
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
            Dispose(true);

        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.StreamProvider.Dispose();
                this.Disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~Session()
        {
            this.Dispose(false);
        }

        internal void AssertValid()
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException("This session was disposed and cannot be used anymore");
            }
        }

        void EnsureTransaction()
        {
            if (CurrentTransaction == null)
            {
                //Make sure the journal is  applied before a new transaction is started
                this.Journal.Apply();
                CurrentTransaction = new Transaction(this);

            }
        }

        private bool IsValidCollectionFileName(string fileName)
        {
            return fileName.ToLower() != Journal.JOURNAL_COLLECTION_NAME.ToLower();
        }
    }
}

