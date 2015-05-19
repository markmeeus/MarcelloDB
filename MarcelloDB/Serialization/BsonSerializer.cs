using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Bson;

namespace MarcelloDB.Serialization
{
    public class BsonSerializer 
    {
        public byte[] Serialize(object o)
        {
            var serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.All;
            var memoryStream = new MemoryStream();
            var bsonWriter = new BsonWriter(memoryStream);

            serializer.Serialize(bsonWriter, o);
            bsonWriter.Flush();
            return memoryStream.ToArray();
        }    

        public object Deserialize(byte[] bytes)
        {
            var serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.All;
            var memoryStream = new MemoryStream(bytes);
            var reader = new BsonReader(memoryStream);

            return serializer.Deserialize(reader);
        }
    }

    public class BsonSerializer<T> : BsonSerializer, IObjectSerializer<T>
    {
        public BsonSerializer ()
        {
        }

        #region IObjectSerializer implementation

        public byte[] Serialize(T obj)
        {
            return base.Serialize(obj);           
        }
       
        public new T Deserialize (byte[] bytes)
        {
            return (T)base.Deserialize(bytes);
        }
        #endregion
    }
}

