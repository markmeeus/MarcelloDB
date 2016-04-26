using System;
using System.Text;
using MarcelloDB.Serialization;

namespace MarcelloDB
{
    internal class LazyStringValue : LazyValue<string>
    {
        internal LazyStringValue(string value) : base (value)
        {
        }

        internal LazyStringValue(byte[] bytes, int firstByteIndex) : base(bytes, firstByteIndex)
        {
        }

        #region implemented abstract members of LazyValue

        internal override byte[] GetValueAsBytes()
        {
            var valueBytes = Encoding.UTF8.GetBytes(this.Value);
            return new BufferWriter(new byte[GetValueByteSize(0)])
                .WriteInt32(valueBytes.Length)
                .WriteBytes(valueBytes)
                .GetTrimmedBuffer();
        }

        internal override byte GetTypeID()
        {
            return LazyValue<string>.TYPEID_STRING;
        }

        protected override int GetValueByteSize(int startIndex)
        {
            return sizeof(Int32) + Encoding.UTF8.GetByteCount(this.Value);
        }

        protected override string LoadValue(int startIndex)
        {
            var reader = new BufferReader(this.Bytes);
            reader.MoveTo(startIndex);
            var nrOfBytes = reader.ReadInt32();

            return Encoding.UTF8.GetString(this.Bytes, startIndex + sizeof(Int32), nrOfBytes);
        }

        #endregion
    }
}

