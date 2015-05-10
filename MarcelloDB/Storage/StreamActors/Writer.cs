using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class Writer<T> : StreamActor
    {
        internal Writer(Marcello session, string collectionName) 
            : base(session, collectionName)
        {
        }

        internal virtual void Write(long address, byte[] bytes)
        {
            GetStream ().Write (address, bytes);
        }            
    }
}

