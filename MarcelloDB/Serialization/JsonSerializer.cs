using System;
using Newtonsoft.Json;

namespace MarcelloDB.Serialization
{
    internal class JsonSerializer<T> : IObjectSerializer<T>
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

        public T Deserialize (byte[] bytes)
        {
            string json = "";
            json = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return JsonConvert.DeserializeObject<T>(json);            
        }

        #endregion
    }
}

