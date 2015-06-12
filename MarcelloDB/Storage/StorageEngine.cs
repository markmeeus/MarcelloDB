using System;
using MarcelloDB;
using MarcelloDB.Storage.StreamActors;
using MarcelloDB.Transactions;

namespace MarcelloDB.Storage
{
    internal class StorageEngine
    {
        internal Session Session { get; set; }

        string StreamName { get; set; }

        public StorageEngine(Session session,string streamName)
        {
            this.Session = session;
            this.StreamName = streamName;
        }

        internal byte[] Read(long address, int length)
        {
            return Reader().Read(address, length);
        }

        internal void Write(long address, byte[] bytes)
        {
            Writer().Write(address, bytes);
        }

        #region reader/writer factories
        Writer Writer()
        {
            return new JournalledWriter(this.Session, this.StreamName);
        }

        Reader Reader()
        {
            return new JournalledReader(this.Session, this.StreamName);
        }
        #endregion
    }
}
