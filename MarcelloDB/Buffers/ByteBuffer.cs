using System;

namespace MarcelloDB.Buffers
{
    internal class ByteBuffer
    {
        byte[] _bytes;
        internal byte[] Bytes { get { return _bytes; } }

        int _length;
        internal int Length { get { return _length; } }

        internal ByteBuffer(byte[] bytes, int length)
        {
            _bytes = bytes;
            _length = length;
        }
    }
}

