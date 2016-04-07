using System;

namespace MarcelloDB.Serialization
{
    internal interface IObjectSerializer<T>
    {
        byte[] Serialize(T obj);

        T Deserialize(byte[] bytes);
    }
}

