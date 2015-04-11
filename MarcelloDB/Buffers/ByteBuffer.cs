using System;

namespace MarcelloDB.Buffers
{
    public class ByteBuffer
    {
        byte[] _bytes;
        public byte[] Bytes { get { return _bytes; } }

        int _length;
        public int Length { get { return _length; } }

        internal ByteBuffer(byte[] bytes, int length)
        {
            _bytes = bytes;
            _length = length;
        }
    }
}

