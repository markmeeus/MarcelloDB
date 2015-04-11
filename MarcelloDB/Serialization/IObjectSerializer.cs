using System;
using MarcelloDB.Buffers;

namespace MarcelloDB.Serialization
{
    public interface IObjectSerializer<T>
    {
        byte[] Serialize(T obj);

        T Deserialize(ByteBuffer buffer);
    }
}

