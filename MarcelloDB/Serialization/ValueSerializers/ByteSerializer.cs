using System;

namespace MarcelloDB.Serialization.ValueSerializers
{
    class ByteSerializer : ValueSerializer<Byte>
    {
        internal override Byte ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadByte();
        }
        internal override void WriteValue(BinaryFormatter formatter, Byte value)
        {
            formatter.WriteByte(value);
        }
    }

    class NullableByteSerializer : ValueSerializer<Byte?>
    {
        internal override Byte? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableByte();
        }
        internal override void WriteValue(BinaryFormatter formatter, Byte? value)
        {
            formatter.WriteNullableByte(value);
        }
    }
}

