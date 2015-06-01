using System;
using MarcelloDB;
using MarcelloDB.Storage.StreamActors;
using MarcelloDB.Transactions.__;
using MarcelloDB.Buffers;

namespace MarcelloDB.Storage
{
    internal class StorageEngine
    {
        internal Session Session { get; set; }

        string CollectionName { get; set; }

        public StorageEngine(Session session,string collectionName)
        {
            this.Session = session;
            this.CollectionName = collectionName;
        }

        internal byte[] Read(long address, int length)
        {
            return Reader().Read(address, length);
        }
                   
        internal void Write(long address, ByteBuffer buffer)
        {
            Writer().Write(address, buffer);
        }                   
            
        #region reader/writer factories
        Writer Writer()
        {            
            return new JournalledWriter(this.Session, this.CollectionName);
        }

        Reader Reader()
        {            
            return new JournalledReader(this.Session, this.CollectionName);
        }
        #endregion 
    }
}
    