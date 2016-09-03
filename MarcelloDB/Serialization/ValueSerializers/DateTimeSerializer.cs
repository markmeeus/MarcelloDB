using System;

namespace MarcelloDB.Serialization.ValueSerializers
{
    class DateTimeSerializer : ValueSerializer<DateTime>
    {
        internal override DateTime ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadDateTime();
        }
        internal override void WriteValue(BinaryFormatter formatter, DateTime value)
        {
            formatter.WriteDateTime(value);
        }
    }

    class NullableDateTimeSerializer : ValueSerializer<DateTime?>
    {
        internal override DateTime? ReadValue(BinaryFormatter formatter)
        {
            return formatter.ReadNullableDateTime();
        }
        internal override void WriteValue(BinaryFormatter formatter, DateTime? value)
        {
            formatter.WriteNullableDateTime(value);
        }
    }
}

