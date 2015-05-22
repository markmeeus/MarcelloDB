using System;

namespace MarcelloDB.Storage
{
    public interface IStorageStreamProvider: IDisposable
    {
        IStorageStream GetStream(string streamName);
    }
}

