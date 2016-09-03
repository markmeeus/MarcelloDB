using System;

namespace MarcelloDB.Serialization.ValueSerializers
{
    class Int16Serializer : ValueSerializer<Int16>
    {
        internal override Int16 ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadInt16();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int16 value)
        {
            formatter.WriteInt16(value);
        }
    }

    class NullableInt16Serializer : ValueSerializer<Int16?>
    {
        internal override Int16? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableInt16();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int16? value)
        {
            formatter.WriteNullableInt16(value);
        }
    }

    class Int32Serializer : ValueSerializer<Int32>
    {
        internal override Int32 ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadInt32();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int32 value)
        {
            formatter.WriteInt32(value);
        }
    }

    class NullableInt32Serializer : ValueSerializer<Int32?>
    {
        internal override Int32? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableInt32();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int32? value)
        {
            formatter.WriteNullableInt32(value);
        }
    }

    class Int64Serializer : ValueSerializer<Int64>
    {
        internal override Int64 ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadInt64();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int64 value)
        {
            formatter.WriteInt64(value);
        }
    }

    class NullableInt64Serializer : ValueSerializer<Int64?>
    {
        internal override Int64? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableInt64();
        }
        internal override void WriteValue(BinaryFormatter formatter, Int64? value)
        {
            formatter.WriteNullableInt64(value);
        }
    }

}

