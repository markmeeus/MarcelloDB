using System;

namespace MarcelloDB.Storage.StreamActors
{
    internal class StreamActor
    {
        protected Marcello Session { get; set; }

        protected string CollectionName {get;set;}

        internal StreamActor(Marcello session, string collectionName)
        {
            this.Session = session;
            this.CollectionName = collectionName;
        }     
            
        protected IStorageStream GetStream()
        {
            return this.Session.StreamProvider.GetStream(this.CollectionName);
        }
    }
}

