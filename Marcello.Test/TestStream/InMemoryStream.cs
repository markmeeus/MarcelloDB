using System;
using System.Collections.Generic;
using System.IO;
using Marcello.Storage;
using System.Diagnostics;

namespace Marcello.Test
{
    public class InMemoryStream : IStorageStream
    {
        public MemoryStream BackingStream { get; set; }
        public string Name {get; set; }
        public InMemoryStream(string name)
        {
            BackingStream = new MemoryStream ();
            this.Name = name;
        }

        #region IStorageStream implementation

        public byte[] Read (long address, int length)
        {
            if (length == 0) {
            }
            byte[] result = new byte[length];
            BackingStream.Seek (address, SeekOrigin.Begin);
            BackingStream.Read (result, 0, length);
            return result;
        }

        public void Write (long address, byte[] bytes)
        {
            BackingStream.Seek (address, SeekOrigin.Begin);
            BackingStream.Write (bytes, 0, (int)bytes.Length);
        }

        #endregion
    }

    public class InMemoryStreamProvider : IStorageStreamProvider
    {
        Dictionary<string, IStorageStream> Streams { get; set;} 

        public InMemoryStreamProvider(){
            Streams = new Dictionary<string, IStorageStream> ();
        }

        #region IStorageStreamProvider implementation

        public IStorageStream GetStream (string streamName)
        {
            if(!Streams.ContainsKey(streamName))
            {
                Streams.Add (streamName, new InMemoryStream (streamName));
            }
            return Streams [streamName];
        }

        #endregion
    }
}

