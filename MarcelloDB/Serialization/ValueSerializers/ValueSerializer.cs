using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MarcelloDB.Records;

namespace MarcelloDB.Serialization.ValueSerializers
{
    abstract class ValueSerializer
    {
        static Dictionary<Type, ValueSerializer> ValueSerializerCache { get; set; }

        static ValueSerializer()
        {
            ValueSerializer.ValueSerializerCache = CreateCacheWithPrimitiveSerializers();
        }

        public static ValueSerializer Instance(Type valueType)
        {
            if(!ValueSerializerCache.ContainsKey(valueType)){
                if (valueType.IsConstructedGenericType)
                {
                    var genericType = valueType.GetGenericTypeDefinition();
                    if (genericType == typeof(ValueWithAddressIndexKey<>))
                    {
                        ValueSerializerCache[valueType] = BuildValueWithAddressSerializer(valueType);
                    }
                    else if(genericType.Name.StartsWith("CompoundValue")){
                        ValueSerializerCache[valueType] = BuildCompoundValueSerializer(valueType);
                    }
                }
            }
            if(!ValueSerializerCache.ContainsKey(valueType)){
                System.Diagnostics.Debug.WriteLine("?");
            }
            return ValueSerializer.ValueSerializerCache[valueType];

        }
        public static ValueSerializer<TValue> Instance<TValue>()
        {
            return (ValueSerializer<TValue>)ValueSerializer.Instance(typeof(TValue));
        }

        static ValueSerializer BuildValueWithAddressSerializer(Type valueWithAddressType)
        {
            var typeOfValue = valueWithAddressType.GenericTypeArguments[0];
            var innerValueSerializer = Instance(typeOfValue);
                     
            var serializerType =
               typeof (ValueWithAddressSerializer<>).GetGenericTypeDefinition ()
                                                    .GetTypeInfo ()
                                                    .MakeGenericType (new Type [] { typeOfValue });

            return (ValueSerializer)Activator.CreateInstance (serializerType, new object []{ innerValueSerializer });
        }

        static ValueSerializer BuildCompoundValueSerializer(Type compoundType)
        {
            var valueTypes = compoundType.GenericTypeArguments;
            var valueSerializers = valueTypes.Select(
                t => Instance(t));

            return (ValueSerializer)Activator.CreateInstance (
                CompoundValueSerializer.GetGenericTypeWithTypes (valueTypes), new object [] { valueSerializers });

        }

        static Dictionary<Type, ValueSerializer> CreateCacheWithPrimitiveSerializers()
        {
            return new Dictionary<Type, ValueSerializer>
            {
                { typeof(Boolean), new BooleanSerializer() },
                { typeof(Boolean?), new NullableBooleanSerializer() },
                { typeof(Byte), new ByteSerializer() },
                { typeof(Byte?), new NullableByteSerializer() },
                { typeof(Int16), new Int16Serializer() },
                { typeof(Int16?), new NullableInt16Serializer() },
                { typeof(Int32), new Int32Serializer() },
                { typeof(Int32?), new NullableInt32Serializer() },
                { typeof(Int64), new Int64Serializer() },
                { typeof(Int64?), new NullableInt64Serializer() },
                { typeof(Decimal), new DecimalSerializer() },
                { typeof(Decimal?), new NullableDecimalSerializer() },
                { typeof(Single), new SingleSerializer() },
                { typeof(Single?), new NullableSingleSerializer() },
                { typeof(Double), new DoubleSerializer() },
                { typeof(Double?), new NullableDoubleSerializer() },
                { typeof(DateTime), new DateTimeSerializer() },
                { typeof(DateTime?), new NullableDateTimeSerializer() },
                { typeof(String), new StringSerializer() }
            };
        }
    }

    abstract class ValueSerializer<TValue> : ValueSerializer
    {
        internal abstract void WriteValue(BinaryFormatter formatter, TValue value);

        internal abstract TValue ReadValue(BinaryFormatter formatter);

    }

}

