using System;
using MarcelloDB.Storage;

namespace MarcelloDB.Platform
{
    public interface IPlatform
    {
        IStorageStreamProvider CreateStorageStreamProvider(string rootPath);
    }
}

