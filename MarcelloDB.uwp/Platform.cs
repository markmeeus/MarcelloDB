using MarcelloDB.Collections;
using MarcelloDB.Platform;
using MarcelloDB.Storage;
using MarcelloDB.uwp.Storage;
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
        public IStorageStreamProvider CreateStorageStreamProvider(string rootPath)
        {
            return new FileStorageStreamProvider(GetFolderForPath(rootPath));        
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
