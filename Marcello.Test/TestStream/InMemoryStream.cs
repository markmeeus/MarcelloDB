using System;
using System.Collections.Generic;
using System.IO;

namespace Marcello.Test
{
    public class InMemoryStream : IStorageStream
    {
        MemoryStream BackingStream { get; set; }

        public InMemoryStream()
        {
            BackingStream = new MemoryStream ();
        }

        #region IStorageStream implementation

        public byte[] Read (long address, int length)
        {
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
                Streams.Add (streamName, new InMemoryStream ());
            }
            return Streams [streamName];
        }

        #endregion
    }
}

