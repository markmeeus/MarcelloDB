using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class Reader<T> : StreamActor
    {
        internal Reader(Marcello session, string collectionName) 
            :base(session, collectionName)
        {
        }
            
        internal virtual byte[] Read(long address, int length)
        {
            return this.GetStream().Read(address, length);
        }            
    }
}

