using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class StreamActor
    {
        protected Session Session { get; set; }

        protected string StreamName {get;set;}

        internal StreamActor(Session session, string streamName)
        {
            this.Session = session;
            this.StreamName = streamName;
        }

        protected IStorageStream GetStream()
        {
            return this.Session.StreamProvider.GetStream(this.StreamName);
        }
    }
}

