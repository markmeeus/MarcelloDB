using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Bson;

namespace Marcello
{
    public class BsonSerializer<T> : IObjectSerializer<T>
    {
        public BsonSerializer ()
        {
        }

        #region IObjectSerializer implementation

        public byte[] Serialize (T obj)
        {
            var serializer = new JsonSerializer();
            var memoryStream = new MemoryStream();
            var bsonWriter = new BsonWriter(memoryStream);

            serializer.Serialize(bsonWriter, obj);
            bsonWriter.Flush();
            return memoryStream.ToArray();
        }

        public T Deserialize (byte[] bytes)
        {
            var serializer = new JsonSerializer();
            var memoryStream = new MemoryStream(bytes);
            var reader = new BsonReader(memoryStream);

            return serializer.Deserialize<T>(reader);
        }

        #endregion
    }
}

