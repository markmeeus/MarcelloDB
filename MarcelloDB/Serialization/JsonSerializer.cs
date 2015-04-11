using System;
using Newtonsoft.Json;
using MarcelloDB.Buffers;

namespace MarcelloDB.Serialization
{
    public class JsonSerializer<T> : IObjectSerializer<T>
    {
        public JsonSerializer ()
        {
        }

        #region IObjectSerializer implementation

        public byte[] Serialize (T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return System.Text.Encoding.UTF8.GetBytes(json.ToCharArray());
        }

        public T Deserialize (ByteBuffer buffer)
        {
            string json = "";
            json = System.Text.Encoding.UTF8.GetString(buffer.Bytes, 0, buffer.Length);
            return JsonConvert.DeserializeObject<T>(json);            
        }

        #endregion
    }
}

