using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class Reader : StreamActor
    {
        internal Reader(Session session, string collectionName) 
            :base(session, collectionName)
        {
        }
            
        internal virtual byte[] Read(long address, int length)
        {
            return this.GetStream().Read(address, length);
        }            
    }
}

