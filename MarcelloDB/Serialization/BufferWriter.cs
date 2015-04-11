using System;
using MarcelloDB.Buffers;

namespace MarcelloDB.Serialization
{
    internal class BufferWriter
    {
        internal Marcello Session { get; set; }

        internal ByteBuffer Buffer { get; set; }

        internal bool IsLittleEndian { get; set; }

        internal int Position { get; set; }

        public BufferWriter(Marcello session, ByteBuffer buffer, bool isLittleEndian)
        {
            this.Session = session;
            this.Buffer = buffer;
            this.IsLittleEndian = isLittleEndian;
            this.Position = 0;
        }

        public BufferWriter WriteInt32(Int32 value)
        {
            WriteBytes(LittleEndian(BitConverter.GetBytes(value)));
            return this;
        }

        public BufferWriter WriteInt64(Int64 value)
        {
            WriteBytes(LittleEndian(BitConverter.GetBytes(value)));
            return this;
        }
            
        public ByteBuffer GetTrimmedBuffer()
        {
            if (this.Buffer.Length == this.Position)
            {
                return this.Buffer;
            }
            else
            {
                var trimmed = new byte[this.Position];
                Array.Copy(this.Buffer.Bytes, trimmed, this.Position - 1);
                return this.Session.ByteBufferManager.FromBytes(trimmed);
            }
        }
            
        byte[] LittleEndian(byte[] bytes)
        {
            if (!this.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        void WriteBytes(byte[] bytes)
        {
            if (this.Position + bytes.Length > Buffer.Length)
            {
                var newBuffer = this.Session.ByteBufferManager.Create(this.Position + bytes.Length);
                this.Buffer.Bytes.CopyTo(newBuffer.Bytes, 0);
                this.Buffer = newBuffer;
            }
            bytes.CopyTo(Buffer.Bytes, this.Position);
            this.Position += bytes.Length;
        }
    }
}


