using System;
using MarcelloDB.Storage;

namespace MarcelloDB.Platform
{
    public interface IPlatform : IDisposable
    {
        IStorageStreamProvider GetStorageStreamProvider(string rootPath);
    }
}

