using System;
using System.Collections.Generic;

namespace Marcello
{
    internal class StorageStreamCollection
    {
        IStorageStreamProvider StreamProvider { get; set; }
        Dictionary<string, IStorageStream> Streams { get; set; }

        internal StorageStreamCollection (IStorageStreamProvider streamProvider)
        {
            StreamProvider = streamProvider;
            Streams = new Dictionary<string, IStorageStream> ();
        }

        internal IStorageStream GetStream(string streamName)
        {
            if(!Streams.ContainsKey(streamName)){
                Streams.Add(streamName, StreamProvider.GetStream(streamName));
            }
            return Streams[streamName];
        }
    }
}

