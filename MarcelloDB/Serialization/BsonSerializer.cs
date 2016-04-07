using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Bson;

namespace MarcelloDB.Serialization
{
    internal class ObjectWrapper<T>
    {
        public T O { get; set; }
    }


    internal class BsonSerializer<T> : IObjectSerializer<T>
    {
        JsonSerializer JsonSerializer { get; set; }

        public BsonSerializer ()
        {
            this.JsonSerializer = GetSerializer();
        }

        #region IObjectSerializer implementation

        public byte[] Serialize(T o)
        {

            var memoryStream = new MemoryStream();
            var bsonWriter = new BsonWriter(memoryStream);

            this.JsonSerializer.Serialize(bsonWriter, new ObjectWrapper<T>{O=o});
            bsonWriter.Flush();

            return memoryStream.ToArray();
        }

        public T Deserialize (byte[] bytes)
        {
            var reader = new BsonReader(
                new MemoryStream(bytes)
            );

            return this.JsonSerializer.Deserialize<ObjectWrapper<T>>(reader).O;
        }
        #endregion

        JsonSerializer GetSerializer(){
            return new JsonSerializer {TypeNameHandling = TypeNameHandling.Auto};
        }
    }
}

