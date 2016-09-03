using System;

namespace MarcelloDB.Serialization.ValueSerializers
{
    class StringSerializer : ValueSerializer<String>
    {
        internal override String ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadString();
        }
        internal override void WriteValue(BinaryFormatter formatter, String value)
        {
            formatter.WriteString(value);
        }
    }
}

