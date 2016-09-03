using System;

namespace MarcelloDB.Serialization.ValueSerializers
{
    class BooleanSerializer : ValueSerializer<Boolean>
    {
        internal override Boolean ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadBool();
        }
        internal override void WriteValue(BinaryFormatter formatter, Boolean value)
        {
            formatter.WriteBool(value);
        }
    }

    class NullableBooleanSerializer : ValueSerializer<Boolean?>
    {
        internal override Boolean? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableBool();
        }
        internal override void WriteValue(BinaryFormatter formatter, Boolean? value)
        {
            formatter.WriteNullableBool(value);
        }
    }
}

