using System;
using MarcelloDB.Serialization;

namespace MarcelloDB.Index.LazyNode.ConcreteValues
{
    internal class LazyInt32Value : LazyValue<Int32>
    {
        internal LazyInt32Value(Int32 value) : base(value)
        {
        }

        internal LazyInt32Value(byte[] bytes, int firstByteIndex) : base(bytes, firstByteIndex)
        {
        }

        protected override Int32 LoadValue(int valueIndex)
        {
            BufferReader reader = new BufferReader(this.Bytes);
            return reader.ReadInt32At(valueIndex);
        }

        internal override byte GetTypeID()
        {
            return TYPEID_INT32;
        }

        internal override byte[] GetValueAsBytes()
        {
            var writer = new BufferWriter(new byte[sizeof(Int32)]);
            return writer
                .WriteInt32(this.Value)
                .GetTrimmedBuffer();
        }
    }
}

