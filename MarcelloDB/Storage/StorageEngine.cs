using System;
using MarcelloDB;
using MarcelloDB.Storage.StreamActors;
using MarcelloDB.Transactions.__;

namespace MarcelloDB.Storage
{
    public abstract class StorageEngine
    {
        internal abstract byte[] Read (long address, int length);

        internal abstract void Write (long address, byte[] bytes);

        protected bool JournalEnabled { get; set; }

        internal void DisableJournal()
        {
            JournalEnabled = false;
        }       
    }

    public class StorageEngine<T> : StorageEngine
    {
        internal Marcello Session { get; set; }

        public StorageEngine(Marcello session)
        {
            Session = session;
            JournalEnabled = typeof(T) != typeof(TransactionJournal);
        }

        internal override byte[] Read(long address, int length)
        {
            return Reader().Read(address, length);
        }
                   
        internal override void Write(long address, byte[] bytes)
        {
            Writer().Write(address, bytes);
        }                   
            
        #region reader/writer factories
        Writer<T> Writer()
        {
            if (JournalEnabled) 
            {    
                return new JournalledWriter<T>(this.Session);
            }
            else 
            {
                return new Writer<T>(this.Session);
            }
        }

        Reader<T> Reader()
        {
            if (JournalEnabled) 
            {    
                return new JournalledReader<T>(this.Session);
            }
            else 
            {
                return new Reader<T>(this.Session);
            }
        }
        #endregion 
    }
}
    