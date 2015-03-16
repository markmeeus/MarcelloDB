using System;

namespace MarcelloDB.Storage
{
    public interface IStorageStreamProvider
    {
        IStorageStream GetStream(string streamName);
    }
}

