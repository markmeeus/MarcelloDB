using System;
using System.Linq;
using System.Collections.Generic;
using MarcelloDB.Transactions;

namespace MarcelloDB.Buffers
{
    internal class ByteBufferManager : ITransactor
    {
        Dictionary<byte[], byte[]> RecycledBytesHash { get; set;}

        List<byte[]> RecycledBytes { get; set;}

        List<byte[]> TrackedBytes { get; set; }

        internal ByteBufferManager()
        {
            RecycledBytesHash = new Dictionary<byte[], byte[]>();
            RecycledBytes = new List<byte[]>();
            TrackedBytes = new List<byte[]>();
        }

        internal ByteBuffer Create(int length)
        {
            var bytes = GetRecycledBytes(length) ?? CreateBytes(length);
            return new ByteBuffer(bytes, length);
        }

        internal ByteBuffer FromBytes(byte[] bytes)
        {
            return new ByteBuffer(bytes, bytes.Length);
        }

        internal void Recycle(ByteBuffer buffer)
        {
            Recycle(buffer.Bytes);
        }

        internal void Recycle(byte[] bytes, bool sortImmediately = true)
        {
            if (RecycledBytesHash.ContainsKey(bytes))
            {
                return; //bytes allready recycled
            }

            //RecycledBytes.Add(bytes);
            RecycledBytesHash.Add(bytes, bytes);

            if (sortImmediately)
            {
                RecycledBytes = RecycledBytes.OrderBy(b => b.Length).ToList();
            }
        }

        internal void RecycleAll()
        {
            foreach (var bytes in TrackedBytes)
            {
                Recycle(bytes, false);
                
            }
            RecycledBytes = RecycledBytes.OrderBy(b => b.Length).ToList();
        }
        #region ITransactor implementation

        public void SaveState()
        {
         
        }

        public void RollbackState()
        {

        }

        public void CleanUp()
        {
            RecycleAll();
        }

        #endregion

        protected virtual byte[] CreateBytes(int minimumLength)
        {
            var bytes = new byte[minimumLength];
            TrackedBytes.Add(bytes);
            return bytes;
        }

        byte[] GetRecycledBytes(int minimumLength)
        {
            byte[] result = this.RecycledBytesHash.Values.FirstOrDefault(b => b.Length >= minimumLength);
            if (result != null)
            {
                //this.RecycledBytes.Remove(result);
                this.RecycledBytesHash.Remove(result);
                return result;
            }
            return null;
        }
    }
}

