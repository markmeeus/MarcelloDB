using System;

namespace Marcello.Storage.StreamActors
{
    internal class JournalledWriter<T> : Writer<T>
    {
        internal JournalledWriter(Marcello session) : base(session)
        {
        }

        internal override void Write (long address, byte[] bytes)
        {
            Session.Journal.Write(typeof(T), address, bytes);
        }
    }
}

