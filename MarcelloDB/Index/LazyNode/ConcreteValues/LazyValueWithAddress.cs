using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;

namespace MarcelloDB.Index.LazyNode.ConcreteValues
{
    internal class LazyValueWithAddress<TValue> : LazyValue<ValueWithAddressIndexKey<TValue>>
    {
        internal LazyValueWithAddress(ValueWithAddressIndexKey<TValue> value) : base(value)
        {
        }

        internal LazyValueWithAddress(byte[] bytes, int firstByteIndex) : base(bytes, firstByteIndex)
        {
        }

        protected override ValueWithAddressIndexKey<TValue> LoadValue(int startIndex)
        {
            BufferReader reader = new BufferReader(this.Bytes);
            reader.MoveTo(startIndex);
            var address = reader.ReadInt64();
            var value = LazyValue<TValue>
                .ConstructValue(this.Bytes, startIndex + sizeof(Int64));

            var valueWithAddress = new ValueWithAddressIndexKey<TValue>();
            valueWithAddress.A = address;
            valueWithAddress.V = value.Value;
            return valueWithAddress;
        }

        internal override byte[] GetValueAsBytes()
        {
            var value = LazyValue<TValue>.ConstructValue(this.Value.V);
            var valueBytes = value.ToBytes();
            return new BufferWriter(new byte[0])
                .WriteInt64(this.Value.A)
                .WriteBytes(valueBytes)
                .GetTrimmedBuffer();
        }

        internal override byte GetTypeID()
        {
            return TYPEID_INT64;
        }

        #region implemented abstract members of LazyValue

        protected override int GetValueByteSize(int startIndex)
        {
            LazyValue<TValue> value;
            if (this.Bytes != null)
            {
                value = LazyValue<TValue>
                    .ConstructValue(this.Bytes, startIndex + sizeof(Int64));
            }
            else
            {
                value = LazyValue<TValue>.ConstructValue(this.Value.V);
            }
            return sizeof(Int64) + value.ByteSize;
        }

        #endregion
    }
}

