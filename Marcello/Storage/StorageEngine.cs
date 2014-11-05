using System;
using Marcello;
using Marcello.Storage.StreamActors;

namespace Marcello.Storage
{
    public abstract class StorageEngine
    {
        internal abstract byte[] Read (long address, int length);

        internal abstract void Write (long address, byte[] bytes);

        protected bool JournalEnabled { get; set; }

        #region internal methods
        internal void DisableJournal()
        {
            JournalEnabled = false;
        }
        #endregion 
    }

    public class StorageEngine<T> : StorageEngine
    {
        internal Marcello Session { get; set; }

        public StorageEngine(Marcello session)
        {
            Session = session;
            JournalEnabled = true;
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
    