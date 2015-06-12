using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class JournalledWriter : Writer
    {
        internal JournalledWriter(Session session, string streamName)
            : base(session, streamName)
        {
        }

        internal override void Write (long address, byte[] bytes)
        {
            Session.Journal.Write(this.StreamName, address, bytes);
        }
    }
}

