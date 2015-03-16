using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class Reader<T> : StreamActor<T>
    {
        internal Reader(Marcello session) : base(session)
        {
        }
            
        internal virtual byte[] Read(long address, int length)
        {
            return this.GetStream().Read(address, length);
        }            
    }
}

