using MarcelloDB.Collections;
using MarcelloDB.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MarcelloDB.uwp
{
    public class Platform : IPlatform
    {
        Dictionary<string, FileStorageStreamProvider> _fileStorageStreamProviders;

        public Platform()
        {
            _fileStorageStreamProviders = new Dictionary<string, FileStorageStreamProvider>();
        }
        public Storage.IStorageStreamProvider GetStorageStreamProvider(string rootPath)
        {
            if (!_fileStorageStreamProviders.ContainsKey(rootPath))
            {
                StorageFolder folder = GetFolderForPath(rootPath);
                _fileStorageStreamProviders[rootPath] = new FileStorageStreamProvider(folder);
            }
            return _fileStorageStreamProviders[rootPath];
        }

        public void Dispose()
        {
            foreach (var streamProvider in _fileStorageStreamProviders.Values)
            {
                streamProvider.Dispose();
            }
        }

        StorageFolder GetFolderForPath(string path)
        {
            var getFolderTask = StorageFolder.GetFolderFromPathAsync(path).AsTask();
            getFolderTask.ConfigureAwait(false);
            getFolderTask.Wait();
            return getFolderTask.Result;
        }
    }
}
