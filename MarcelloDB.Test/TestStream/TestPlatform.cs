using System;
using MarcelloDB.Platform;
using System.Collections.Generic;

namespace MarcelloDB.Test
{
    public class TestPlatform : IPlatform
    {
        Dictionary<string, InMemoryStreamProvider> _streams = new Dictionary<string, InMemoryStreamProvider>();
        public MarcelloDB.Storage.IStorageStreamProvider CreateStorageStreamProvider(string rootPath)
        {
            if (!_streams.ContainsKey(rootPath))
            {
                _streams.Add(rootPath, new InMemoryStreamProvider());
            }
            return _streams[rootPath];
        }
    }
}

