using System;

namespace Marcello
{
    public interface IObjectSerializer<T>
    {
        byte[] Serialize(T obj);

        T Deserialize(byte[] bytes);
    }
}

