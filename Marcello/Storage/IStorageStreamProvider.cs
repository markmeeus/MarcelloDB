using System;

namespace Marcello.Storage
{
    public interface IStorageStreamProvider
    {
        IStorageStream GetStream(string streamName);
    }
}

