using System;
using MarcelloDB;
using MarcelloDB.Storage.StreamActors;
using MarcelloDB.Transactions.__;

namespace MarcelloDB.Storage
{
    internal class StorageEngine<T>
    {
        internal Marcello Session { get; set; }

        public StorageEngine(Marcello session)
        {
            Session = session;
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
            return new JournalledWriter(this.Session, typeof(T).Name);
        }

        Reader Reader()
        {            
            return new JournalledReader(this.Session, typeof(T).Name);
        }
        #endregion 
    }
}
    