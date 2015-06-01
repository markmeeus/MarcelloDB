using System;
using MarcelloDB.Buffers;

namespace MarcelloDB.Storage.StreamActors
{
    internal class JournalledWriter : Writer
    {
        internal JournalledWriter(Session session, string collectionName) 
            : base(session, collectionName)
        {
        }

        internal override void Write (long address, ByteBuffer buffer)
        {
            Session.Journal.Write(this.CollectionName, address, buffer);
        }
    }
}

