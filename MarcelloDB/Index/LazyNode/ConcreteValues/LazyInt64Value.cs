using System;
using MarcelloDB.Serialization;

namespace MarcelloDB.Index.LazyNode.ConcreteValues
{
    internal class LazyInt64Value : LazyValue<Int64>
    {
        internal LazyInt64Value(Int64 value) : base(value)
        {
        }

        internal LazyInt64Value(byte[] bytes, int firstByteIndex) : base(bytes, firstByteIndex)
        {
        }

        protected override Int64 LoadValue(int startIndex)
        {
            BufferReader reader = new BufferReader(this.Bytes);
            return reader.ReadInt64At(startIndex);
        }

        internal override byte[] GetValueAsBytes()
        {
            var bytes = new byte[sizeof(Int64)];
            return new BufferWriter(bytes).WriteInt64(this.Value).GetTrimmedBuffer();
        }

        internal override byte GetTypeID()
        {
            return TYPEID_INT64;
        }

        #region implemented abstract members of LazyValue

        protected override int GetValueByteSize()
        {
            return sizeof(Int64);
        }

        #endregion
    }
}

