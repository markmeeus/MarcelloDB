using System;

namespace MarcelloDB.Serialization
{
    public interface IObjectSerializer<T>
    {
        byte[] Serialize(T obj);

        T Deserialize(byte[] bytes);
    }
}

