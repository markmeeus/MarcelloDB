using System;
using MarcelloDB.Platform;

namespace MarcelloDB.Test
{
    public class TestPlatform : IPlatform
    {
        #region IPlatform implementation

        public MarcelloDB.Storage.IStorageStreamProvider GetStorageStreamProvider(string rootPath)
        {
            return new InMemoryStreamProvider();
        }

        #endregion

        public void Dispose()
        {            
        }
    }
}

