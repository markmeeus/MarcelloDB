using System;
using System.Linq;
using System.Collections.Generic;

namespace MarcelloDB.Buffers
{
    internal class ByteBufferManager
    {
        List<byte[]> RecycledBytes { get; set;}

        internal ByteBufferManager()
        {
            RecycledBytes = new List<byte[]>();
        }

        internal ByteBuffer Create(int length)
        {
            var bytes = GetRecycledBytes(length) ?? CreateBytes(length);

            return new ByteBuffer(bytes, length);
        }

        internal void Recycle(ByteBuffer buffer)
        {
            RecycledBytes.Add(buffer.Bytes);
            RecycledBytes = RecycledBytes.OrderBy(b => b.Length).ToList();
        }

        protected virtual byte[] CreateBytes(int minimumLength)
        {
            return new byte[minimumLength];
        }

        byte[] GetRecycledBytes(int minimumLength)
        {
            byte[] result = this.RecycledBytes.FirstOrDefault(b => b.Length >= minimumLength);
            if (result != null)
            {
                this.RecycledBytes.Remove(result);
                return result;
            }
            return null;
        }
    }
}

