using System;

namespace Marcello.Serialization
{
    internal class BufferWriter
    {
        internal byte[] Buffer { get; set; }

        internal bool IsLittleEndian { get; set; }

        internal int Position { get; set; }

        public BufferWriter(byte[] buffer, bool isLittleEndian)
        {
            this.Buffer = buffer;
            this.IsLittleEndian = isLittleEndian;
            this.Position = 0;
        }

        public void Write(Int32 value)
        {
            WriteBytes(LittleEndian(BitConverter.GetBytes(value)));
        }

        public void Write(Int64 value)
        {
            WriteBytes(LittleEndian(BitConverter.GetBytes(value)));
        }


        public byte[] GetTrimmedBuffer()
        {
            if (this.Buffer.Length == this.Position)
            {
                return this.Buffer;
            }
            else
            {
                var trimmed = new byte[this.Position];
                Array.Copy(this.Buffer, trimmed, this.Position - 1);
                return trimmed;
            }
        }

        #region private methods
        private byte[] LittleEndian(byte[] bytes)
        {
            if (!this.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        private void WriteBytes(byte[] bytes)
        {
            if (this.Position + bytes.Length > Buffer.Length)
            {
                var newBuffer = new byte[this.Position + bytes.Length];
                this.Buffer.CopyTo(newBuffer, 0);
                this.Buffer = newBuffer;
            }
            bytes.CopyTo(Buffer, this.Position);
            this.Position += bytes.Length;
        }            
        #endregion private methods
    }
}


