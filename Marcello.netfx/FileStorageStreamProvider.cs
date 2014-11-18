﻿using System;
using Marcello.Storage;
using System.Collections.Generic;
using System.IO;

namespace Marcello
{
    public class FileStorageStreamProvider : IStorageStreamProvider
    {
        string RootPath { get; set;}

        Dictionary<string, IStorageStream> Streams { get; set; }

        public FileStorageStreamProvider(string rootPath)
        {
            this.RootPath = rootPath;
            this.Streams = new Dictionary<string, IStorageStream>();
        }

        #region IStorageStreamProvider implementation

        public IStorageStream GetStream (string streamName)
        {
            if (!this.Streams.ContainsKey (streamName)) 
            {
                this.Streams.Add(
                    streamName,
                    new FileStorageStream(System.IO.Path.Combine(this.RootPath, streamName)));
            }
            return this.Streams[streamName];
        }
        #endregion
    }

    internal class FileStorageStream : IStorageStream
    {
        FileStream _backingStream;

        internal FileStorageStream(string filePath)
        {
            _backingStream = new FileStream (
                filePath,
                FileMode.OpenOrCreate, 
                FileAccess.ReadWrite);
        }

        #region IStorageStream implementation
        public byte[] Read (long address, int length)
        {
            byte[] result = new byte[length];
            _backingStream.Seek(address, SeekOrigin.Begin);
            _backingStream.Read(result, 0, length);
            return result;
        }

        public void Write (long address, byte[] bytes)
        {
            _backingStream.Seek(address, SeekOrigin.Begin);
            _backingStream.Write(bytes, 0, (int)bytes.Length);
        }
        #endregion
    }
}

