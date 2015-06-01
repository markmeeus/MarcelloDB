using System;
using MarcelloDB.Buffers;

namespace MarcelloDB.Storage.StreamActors
{
    internal class Writer : StreamActor
    {
        internal Writer(Session session, string collectionName) 
            : base(session, collectionName)
        {
        }

        internal virtual void Write(long address, ByteBuffer buffer)
        {
            GetStream().Write(address, buffer);
        }            
    }
}

