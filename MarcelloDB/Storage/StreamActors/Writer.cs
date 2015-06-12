using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class Writer : StreamActor
    {
        internal Writer(Session session, string streamName)
            : base(session, streamName)
        {
        }

        internal virtual void Write(long address, byte[] bytes)
        {
            GetStream().Write (address, bytes);
        }
    }
}

