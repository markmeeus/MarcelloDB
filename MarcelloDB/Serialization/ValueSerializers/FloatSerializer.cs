using System;

namespace MarcelloDB.Serialization.ValueSerializers
{
    class DecimalSerializer : ValueSerializer<Decimal>
    {
        internal override Decimal ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadDecimal();
        }
        internal override void WriteValue(BinaryFormatter formatter, Decimal value)
        {
            formatter.WriteDecimal(value);
        }
    }

    class NullableDecimalSerializer : ValueSerializer<Decimal?>
    {
        internal override Decimal? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableDecimal();
        }
        internal override void WriteValue(BinaryFormatter formatter, Decimal? value)
        {
            formatter.WriteNullableDecimal(value);
        }
    }

    class SingleSerializer : ValueSerializer<Single>
    {
        internal override Single ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadSingle();
        }
        internal override void WriteValue(BinaryFormatter formatter, Single value)
        {
            formatter.WriteSingle(value);
        }
    }

    class NullableSingleSerializer : ValueSerializer<Single?>
    {
        internal override Single? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableSingle();
        }
        internal override void WriteValue(BinaryFormatter formatter, Single? value)
        {
            formatter.WriteNullableSingle(value);
        }
    }

    class DoubleSerializer : ValueSerializer<Double>
    {
        internal override Double ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadDouble();
        }
        internal override void WriteValue(BinaryFormatter formatter, Double value)
        {
            formatter.WriteDouble(value);
        }
    }

    class NullableDoubleSerializer : ValueSerializer<Double?>
    {
        internal override Double? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableDouble();
        }
        internal override void WriteValue(BinaryFormatter formatter, Double? value)
        {
            formatter.WriteNullableDouble(value);
        }
    }

}

