using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class JournalledWriter<T> : Writer<T>
    {
        internal JournalledWriter(Marcello session, string collectionName) 
            : base(session, collectionName)
        {
        }

        internal override void Write (long address, byte[] bytes)
        {
            Session.Journal.Write(typeof(T), address, bytes);
        }
    }
}

