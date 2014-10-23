using System;

namespace Marcello
{
    internal class StorageEngine<T>
    {
        internal IStorageStreamProvider StreamProvider { get; set; }

        internal StorageEngine(IStorageStreamProvider streamProvider)
        {
            StreamProvider = streamProvider;
        }

        internal byte[] Read(long address, int length)
        {
            return GetStream().Read(address, length);
        }
                   
        internal void Write(long address, byte[] bytes)
        {
            GetStream().Write (address, bytes);
        }

        #region private methods
        private IStorageStream GetStream()
        {
            return StreamProvider.GetStream(typeof(T).Name);
        }
        #endregion
    }
}
    