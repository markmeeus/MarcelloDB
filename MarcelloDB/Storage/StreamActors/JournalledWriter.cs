using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class JournalledWriter : Writer
    {
        internal JournalledWriter(Session session, string collectionName) 
            : base(session, collectionName)
        {
        }

        internal override void Write (long address, byte[] bytes)
        {
            Session.Journal.Write(this.CollectionName, address, bytes);
        }
    }
}

