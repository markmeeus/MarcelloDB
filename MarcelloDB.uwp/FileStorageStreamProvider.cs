using MarcelloDB.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MarcelloDB.uwp
{
    public class FileStorageStreamProvider : IStorageStreamProvider
    {
        StorageFolder RootFolder { get; set; }

        Dictionary<string, IStorageStream> Streams { get; set; }

        public FileStorageStreamProvider(StorageFolder rootFolder)
        {
            this.RootFolder = rootFolder;
            this.Streams = new Dictionary<string, IStorageStream>();
        }

        #region IStorageStreamProvider implementation
        public IStorageStream GetStream(string streamName)
        {
            if (!this.Streams.ContainsKey(streamName))
            {
                this.Streams.Add(
                    streamName,
                    new FileStorageStream(this.RootFolder, streamName));
            }
            return this.Streams[streamName];
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var stream in this.Streams.Values)
                {
                    ((FileStorageStream)stream).Dispose();
                }
            }
            GC.SuppressFinalize(this);
        }

        ~FileStorageStreamProvider()
        {
            Dispose(false);
        }
    }

    internal class FileStorageStream : IStorageStream, IDisposable
    {
        Stream _backingStream;

        internal FileStorageStream(StorageFolder rootFolder, string filePath)
        {
            var task = rootFolder.OpenStreamForWriteAsync(filePath, CreationCollisionOption.OpenIfExists);
            task.ConfigureAwait(false);
            task.Wait();
            _backingStream = task.Result;
        }

        #region IStorageStream implementation
        public byte[] Read(long address, int length)
        {
            byte[] result = new byte[length];
            _backingStream.Seek(address, SeekOrigin.Begin);
            _backingStream.Read(result, 0, length);
            return result;
        }

        public void Write(long address, byte[] bytes)
        {
            _backingStream.Seek(address, SeekOrigin.Begin);
            _backingStream.Write(bytes, 0, (int)bytes.Length);
            _backingStream.Flush();
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                _backingStream.Flush();
                _backingStream.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        ~FileStorageStream()
        {
            Dispose(false);
        }
    }
}
