using System;
using MarcelloDB.Buffers;

namespace MarcelloDB.Storage
{
    public interface IStorageStream
    {
        byte[] Read(long address, int length);

        void Write(long address, ByteBuffer buffer);
    }
}

