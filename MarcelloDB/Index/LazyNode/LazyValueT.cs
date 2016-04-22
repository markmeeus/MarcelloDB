using System;
using MarcelloDB.Serialization;
using MarcelloDB.Index.LazyNode.ConcreteValues;

namespace MarcelloDB
{
    internal abstract class LazyValue<T>
    {
        internal const byte TYPEID_INT32 = 0;
        internal const byte TYPEID_INT64 = 1;
        internal const byte TYPEID_INTSTRING = 2;
        internal const byte TYPEID_GUID = 3;
        internal const byte TYPEID_VALUE_WITH_ADDRESS_INDEX_KEY = 4;
        internal const byte TYPEID_ANY = 5;

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


        internal static LazyValue<T> ConstructValue(byte[] bytes, int firstByteIndex)
        {
            if (typeof(T) == typeof(Int32))
            {
                return (LazyValue<T>)(object)new LazyInt32Value(bytes, firstByteIndex);
            }
            if (typeof(T) == typeof(Int64))
            {
                return (LazyValue<T>)(object)new LazyInt64Value(bytes, firstByteIndex);
            }

            throw new NotImplementedException();
        }

        internal static LazyValue<T> ConstructValue(T value)
        {
            if (typeof(T) == typeof(Int32))
            {
                return (LazyValue<T>)(object)new LazyInt32Value((Int32)(object)value);
            }
            if (typeof(T) == typeof(Int64))
            {
                return (LazyValue<T>)(object)new LazyInt64Value((Int64)(object)value);
            }

            throw new NotImplementedException();
        }
    }
}

