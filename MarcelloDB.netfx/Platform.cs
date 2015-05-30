using System;
using MarcelloDB.Platform;
using System.Collections.Generic;

namespace MarcelloDB.netfx
{
    public class Platform : IPlatform
    {
        Dictionary<string, FileStorageStreamProvider> _streamProviders;

        public Platform()
        {
            _streamProviders = new Dictionary<string, FileStorageStreamProvider>();
        }

        #region IPlatform implementation

        public MarcelloDB.Storage.IStorageStreamProvider GetStorageStreamProvider(string rootPath)
        {
            if (!_streamProviders.ContainsKey(rootPath))
            {
                _streamProviders[rootPath] = new FileStorageStreamProvider(rootPath);
            }
            return _streamProviders[rootPath];
        }

        #endregion

        public void Dispose()
        {
            foreach (var provider in _streamProviders.Values)
            {
                provider.Dispose();
            }
            _streamProviders = new Dictionary<string, FileStorageStreamProvider>();
        }
    }
}

