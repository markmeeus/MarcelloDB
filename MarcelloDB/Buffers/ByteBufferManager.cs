using System;
using System.Linq;
using System.Collections.Generic;
using MarcelloDB.Transactions;

namespace MarcelloDB.Buffers
{
    internal class ByteBufferManager : ITransactor
    {
        List<byte[]> RecycledBytes { get; set;}
        List<byte[]> CreatedBytes { get; set; }

        internal ByteBufferManager()
        {
            RecycledBytes = new List<byte[]>();
            CreatedBytes = new List<byte[]>();
        }

        internal ByteBuffer Create(int length)
        {
            var bytes = GetRecycledBytes(length) ?? CreateBytes(length);
            CreatedBytes.Add(bytes);
            return new ByteBuffer(bytes, length);
        }

        internal void Recycle(ByteBuffer buffer)
        {
            Recycle(buffer.Bytes);
        }

        internal void Recycle(byte[] bytes)
        {
            RecycledBytes.Add(bytes);
            RecycledBytes = RecycledBytes.OrderBy(b => b.Length).ToList();
        }

        internal void RecycleAll()
        {
            foreach (var bytes in CreatedBytes)
            {
                Recycle(bytes);
            }
        }
        #region ITransactor implementation

        public void SaveState()
        {
         
        }

        public void RollbackState()
        {

        }

        #endregion

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

