using System;
using MarcelloDB.Buffers;

namespace MarcelloDB.Storage.StreamActors
{
    internal class Writer<T> : StreamActor<T>
    {
        internal Writer(Marcello session) : base(session)
        {
        }

        internal virtual void Write(long address, ByteBuffer buffer)
        {
            GetStream().Write(address, buffer);
        }            
    }
}

