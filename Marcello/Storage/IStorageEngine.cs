using System;

namespace Marcello
{
    internal interface IStorageEngine
    {
        byte[] Read(long address, long length);

        long Append(byte[] bytes);

        void Write(long address, byte[] bytes);
    }
}

