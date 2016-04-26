using System;
using MarcelloDB.Serialization;

namespace MarcelloDB.Index.LazyNode.ConcreteValues
{
    internal class LazyInt16Value : LazyValue<Int16>
    {
        internal LazyInt16Value(Int16 value) : base(value)
        {
        }

        internal LazyInt16Value(byte[] bytes, int firstByteIndex) : base(bytes, firstByteIndex)
        {
        }

        protected override Int16 LoadValue(int valueIndex)
        {
            BufferReader reader = new BufferReader(this.Bytes);
            return reader.ReadInt16At(valueIndex);
        }

        internal override byte GetTypeID()
        {
            return TYPEID_INT32;
        }

        internal override byte[] GetValueAsBytes()
        {
            var writer = new BufferWriter(new byte[sizeof(Int16)]);
            return writer
                .WriteInt16(this.Value)
                .GetTrimmedBuffer();
        }

        #region implemented abstract members of LazyValue

        protected override int GetValueByteSize(int startIndex)
        {
            return sizeof(Int16);
        }

        #endregion
    }
}

