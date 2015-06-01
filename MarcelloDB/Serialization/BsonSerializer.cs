using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Bson;
using MarcelloDB.Buffers;

namespace MarcelloDB.Serialization
{
    public class ObjectWrapper<T>
    {
        public T O { get; set; }
    }


    public class BsonSerializer<T> : IObjectSerializer<T>
    {
        public BsonSerializer ()
        {
        }

        #region IObjectSerializer implementation

        public byte[] Serialize(T o)
        {
            var serializer = GetSerializer();

            var memoryStream = new MemoryStream();
            var bsonWriter = new BsonWriter(memoryStream);

            serializer.Serialize(bsonWriter, new ObjectWrapper<T>{O=o});
            bsonWriter.Flush();

            return memoryStream.ToArray();
        }

        public T Deserialize (ByteBuffer buffer)
        {
            var serializer = new JsonSerializer();
            var memoryStream = new MemoryStream(buffer.Bytes, 0, buffer.Length);
            var reader = new BsonReader(memoryStream);

            return serializer.Deserialize<ObjectWrapper<T>>(reader).O;
        }
        #endregion

        JsonSerializer GetSerializer()
        {
            return new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
        }
    }
}

