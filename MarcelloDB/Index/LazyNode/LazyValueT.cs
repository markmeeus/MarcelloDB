using System;
using MarcelloDB.Serialization;

namespace MarcelloDB
{
    internal abstract class LazyValue<T>
    {
        internal const byte TYPEID_INT32 = 0;
        internal const byte TYPEID_INT64 = 1;
        internal const byte TYPEID_INTSTRING = 2;
        internal const byte TYPEID_GUID = 3;
        internal const byte TYPEID_ANY = 4;

        T _value;

        bool _valueLoaded = false;

        protected byte[] Bytes { get; set; }

        protected int FirstByteIndex { get; set; }

        internal LazyValue(T value)
        {
            _value = value;
            _valueLoaded = true;
        }

        internal LazyValue(byte[] bytes, int firstByteIndex)
        {
            this.Bytes = bytes;
            this.FirstByteIndex = firstByteIndex;
        }


        internal T Value
        {
            get
            {
                if (!_valueLoaded)
                {
                    //value starts after the valueID byte
                    _value = LoadValue(this.FirstByteIndex + sizeof(byte));
                    _valueLoaded = true;
                }
                return _value;
            }
        }

        internal byte[] ToBytes()
        {
            var valueAsBytes = this.GetValueAsBytes();
            var typeID = this.GetTypeID();
            var bytes = new byte[ sizeof(byte) + valueAsBytes.Length];

            return new BufferWriter(bytes)
                .WriteByte(typeID)
                .WriteBytes(valueAsBytes)
                .GetTrimmedBuffer();
        }

        internal abstract byte[] GetValueAsBytes();

        internal abstract byte GetTypeID();

        internal virtual int ByteSize
        {
            get
            {
                return sizeof(byte) + this.GetValueByteSize();
            }
        }
        protected abstract int GetValueByteSize();

        protected abstract T LoadValue(int startIndex);
    }
}

