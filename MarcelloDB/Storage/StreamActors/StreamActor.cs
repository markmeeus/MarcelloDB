using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class StreamActor : SessionBoundObject
    {
        protected string StreamName {get;set;}

        internal StreamActor(Session session, string streamName) : base(session)
        {
            this.StreamName = streamName;
        }

        protected IStorageStream GetStream()
        {
            return this.Session.StreamProvider.GetStream(this.StreamName);
        }
    }
}

