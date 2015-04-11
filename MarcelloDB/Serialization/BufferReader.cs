using System;
using MarcelloDB.Buffers;

namespace MarcelloDB.Serialization
{
    public class BufferReader
    {
        Marcello Session { get; set; }

        ByteBuffer Buffer { get; set; }

        int Position {get;set;}

        bool IsLittleEndian {get;set;}

        public BufferReader(Marcello session, ByteBuffer buffer, bool isLittleEndian)
        {
            this.Session = session;
            this.Buffer = buffer;
            this.Position = 0;
            this.IsLittleEndian = isLittleEndian;
        }

        public Int32 ReadInt32()
        {
            var bytes = GetBytesInLittleEndianOrder(sizeof(Int32));
                
            var value = BitConverter.ToInt32(bytes, 0);
            this.Position += sizeof(Int32);
            return value;
        }

        public Int64 ReadInt64()
        {
            var bytes = GetBytesInLittleEndianOrder(sizeof(Int64));

            var value = BitConverter.ToInt64(bytes, 0);
            this.Position += sizeof(Int64);
            return value;
        }            

        byte[] GetBytesInLittleEndianOrder(int size)
        {
            var bytes =  new byte[size];
            Array.Copy(this.Buffer.Bytes, this.Position, bytes, 0, bytes.Length);
            if (!this.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
    }
}

