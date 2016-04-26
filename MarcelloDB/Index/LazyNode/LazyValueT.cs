using System;
using MarcelloDB.Serialization;
using MarcelloDB.Records;
using MarcelloDB.Index.LazyNode.ConcreteValues;

namespace MarcelloDB
{
    internal abstract class LazyValue<T>
    {
        internal const byte TYPEID_INT16 = 0;
        internal const byte TYPEID_INT32 = 1;
        internal const byte TYPEID_INT64 = 2;
        internal const byte TYPEID_STRING = 3;
        internal const byte TYPEID_GUID = 4;
        internal const byte TYPEID_VALUE_WITH_ADDRESS_INDEX_KEY = 5;
        internal const byte TYPEID_ANY = 6;

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
                return sizeof(byte) + this.GetValueByteSize(this.FirstByteIndex + sizeof(byte));
            }
        }
        protected abstract int GetValueByteSize(int startIndex);

        protected abstract T LoadValue(int startIndex);


        internal static LazyValue<T> ConstructValue(byte[] bytes, int firstByteIndex)
        {
            if (typeof(T) == typeof(Int16))
            {
                return (LazyValue<T>)(object)new LazyInt16Value(bytes, firstByteIndex);
            }

            if (typeof(T) == typeof(Int32))
            {
                return (LazyValue<T>)(object)new LazyInt32Value(bytes, firstByteIndex);
            }

            if (typeof(T) == typeof(Int64))
            {
                return (LazyValue<T>)(object)new LazyInt64Value(bytes, firstByteIndex);
            }

            if (typeof(T) == typeof(string))
            {
                return (LazyValue<T>)(object)new LazyStringValue(bytes, firstByteIndex);
            }

            if (typeof(T) == typeof(Guid))
            {
                return (LazyValue<T>)(object)new LazyGuidValue(bytes, firstByteIndex);
            }

            if (typeof(T) == typeof(ValueWithAddressIndexKey<string>))
            {
                return (LazyValue<T>)(object)
                    new LazyValueWithAddress<string>(bytes, firstByteIndex);
            }

            if (typeof(T) == typeof(ValueWithAddressIndexKey<Int32>))
            {
                return (LazyValue<T>)(object)
                    new LazyValueWithAddress<Int32>(bytes, firstByteIndex);
            }

            if (typeof(T) == typeof(ValueWithAddressIndexKey<Int64>))
            {
                return (LazyValue<T>)(object)
                    new LazyValueWithAddress<Int64>(bytes, firstByteIndex);
            }

            if (typeof(T) == typeof(ValueWithAddressIndexKey<Guid>))
            {
                return (LazyValue<T>)(object)
                    new LazyValueWithAddress<Guid>(bytes, firstByteIndex);
            }

            throw new NotImplementedException();
        }


        internal static LazyValue<T> ConstructValue(T value)
        {
            if (typeof(T) == typeof(Int16))
            {
                return (LazyValue<T>)(object)new LazyInt16Value((Int16)(object)value);
            }
            if (typeof(T) == typeof(Int32))
            {
                return (LazyValue<T>)(object)new LazyInt32Value((Int32)(object)value);
            }
            if (typeof(T) == typeof(Int64))
            {
                return (LazyValue<T>)(object)new LazyInt64Value((Int64)(object)value);
            }
            if (typeof(T) == typeof(string))
            {
                return (LazyValue<T>)(object)new LazyStringValue((string)(object)value);
            }
            if (typeof(T) == typeof(Guid))
            {
                return (LazyValue<T>)(object)new LazyGuidValue((Guid)(object)value);
            }

            if (typeof(T) == typeof(ValueWithAddressIndexKey<string>))
            {
                return (LazyValue<T>)(object)
                    new LazyValueWithAddress<string>((ValueWithAddressIndexKey<string>)(object)value);
            }

            if (typeof(T) == typeof(ValueWithAddressIndexKey<Int16>))
            {
                return (LazyValue<T>)(object)
                    new LazyValueWithAddress<Int16>((ValueWithAddressIndexKey<Int16>)(object)value);
            }
                
            if (typeof(T) == typeof(ValueWithAddressIndexKey<Int32>))
            {
                return (LazyValue<T>)(object)
                    new LazyValueWithAddress<Int32>((ValueWithAddressIndexKey<Int32>)(object)value);
            }

            if (typeof(T) == typeof(ValueWithAddressIndexKey<Int64>))
            {
                return (LazyValue<T>)(object)
                    new LazyValueWithAddress<Int64>((ValueWithAddressIndexKey<Int64>)(object)value);
            }

            if (typeof(T) == typeof(ValueWithAddressIndexKey<Guid>))
            {
                return (LazyValue<T>)(object)
                    new LazyValueWithAddress<Guid>((ValueWithAddressIndexKey<Guid>)(object)value);
            }

            throw new NotImplementedException();
        }

        internal static LazyValueWithAddress<T> ConstructValue(ValueWithAddressIndexKey<T> value)
        {
            return new LazyValueWithAddress<T>(value);
        }
    }
}

