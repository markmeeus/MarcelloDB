using System;

namespace MarcelloDB.Serialization
{
    public class BufferReader
    {
        byte[] Buffer { get; set; }

        int Position {get;set;}

        bool IsLittleEndian {get;set;}

        internal BufferReader(byte[] buffer)
        {
            Initialize(buffer, BitConverter.IsLittleEndian);
        }

        internal BufferReader(byte[] buffer, bool isLittleEndian)
        {
            Initialize(buffer, isLittleEndian);
        }

        internal byte ReadByte()
        {
            var value = this.Buffer[this.Position];
            this.Position++;
            return value;
        }

        internal Int32 ReadInt32()
        {
            var bytes = GetBytesInLittleEndianOrder(sizeof(Int32));

            var value = BitConverter.ToInt32(bytes, 0);
            this.Position += sizeof(Int32);
            return value;
        }

        internal Int64 ReadInt64()
        {
            var bytes = GetBytesInLittleEndianOrder(sizeof(Int64));

            var value = BitConverter.ToInt64(bytes, 0);
            this.Position += sizeof(Int64);
            return value;
        }

        void Initialize(byte[] buffer, bool isLittleEndian){
            this.Buffer = buffer;
            this.Position = 0;
            this.IsLittleEndian = isLittleEndian;
        }

        byte[] GetBytesInLittleEndianOrder(int size)
        {
            var bytes = new byte[size];
            Array.Copy(this.Buffer, this.Position, bytes, 0, bytes.Length);
            if (!this.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
    }
}