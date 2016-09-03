using System;
using MarcelloDB.Records;

namespace MarcelloDB.Serialization.ValueSerializers
{
    class ValueWithAddressSerializer<TValue> : ValueSerializer<ValueWithAddressIndexKey<TValue>>
    {
        ValueSerializer<TValue> ValueSerializer { get; set; }

        internal ValueWithAddressSerializer(ValueSerializer<TValue> valueSerializer){
            this.ValueSerializer = valueSerializer;
        }

        internal override void WriteValue(BinaryFormatter formatter, ValueWithAddressIndexKey<TValue> value)
        {
            formatter.WriteInt64(value.A);
            this.ValueSerializer.WriteValue(formatter, value.V);
        }

        internal override ValueWithAddressIndexKey<TValue> ReadValue(BinaryFormatter formatter)
        {
            var result = new ValueWithAddressIndexKey<TValue>();
            result.A = formatter.ReadInt64();
            result.V = this.ValueSerializer.ReadValue(formatter);
            return result;
        }
    }
}

