using System;
using MarcelloDB.Buffers;

namespace MarcelloDB.Storage.StreamActors
{
    internal class JournalledWriter<T> : Writer<T>
    {
        internal JournalledWriter(Marcello session) : base(session)
        {
        }

        internal override void Write (long address, ByteBuffer buffer)
        {
            Session.Journal.Write(typeof(T), address, buffer);
        }
    }
}

