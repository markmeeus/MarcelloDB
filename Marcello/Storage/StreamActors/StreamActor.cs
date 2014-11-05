using System;

namespace Marcello.Storage.StreamActors
{
    internal class StreamActor<T>
    {
        protected Marcello Session { get; set; }

        internal StreamActor(Marcello session)
        {
            this.Session = session;
        }     

        #region protected methods
        protected IStorageStream GetStream()
        {
            return this.Session.StreamProvider.GetStream(typeof(T).Name);
        }
        #endregion
    }
}

