using System;
using MarcelloDB.Platform;

namespace MarcelloDB.Test
{
    public class TestPlatform : IPlatform
    {
        public MarcelloDB.Storage.IStorageStreamProvider CreateStorageStreamProvider(string rootPath)
        {
            return new InMemoryStreamProvider();
        }
    }
}

