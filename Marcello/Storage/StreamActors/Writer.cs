using System;

namespace Marcello.Storage.StreamActors
{
    internal class Writer<T> : StreamActor<T>
    {
        internal Writer(Marcello session) : base(session)
        {
        }

        internal virtual void Write(long address, byte[] bytes)
        {
            GetStream ().Write (address, bytes);
        }            
    }
}

