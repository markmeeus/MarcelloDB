using System;
using MarcelloDB.Platform;
using System.Collections.Generic;
using MarcelloDB.Storage;

namespace MarcelloDB.netfx
{
    public class Platform : IPlatform
    {
        public IStorageStreamProvider CreateStorageStreamProvider(string rootPath)
        {
            return new FileStorageStreamProvider(rootPath);
        }
    }
}

