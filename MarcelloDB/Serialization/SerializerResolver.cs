using System;
using MarcelloDB.Records;
using MarcelloDB.Index;
using System.Collections.Generic;


namespace MarcelloDB.Serialization
{
	internal class SerializerResolver
	{
        Dictionary<Type, object> _serializers;

        internal SerializerResolver()
        {
            _serializers = new Dictionary<Type, object> {
                {typeof(IndexMetaRecord),                   new  IndexMetaRecordSerializer()},
                {typeof(Node<EmptyRecordIndexKey, Int64>),  new EmptyRecordIndexNodeSerializer()},
            };
        }

        internal IObjectSerializer<T> SerializerFor<T>()
        {
            if(_serializers.ContainsKey(typeof(T)))
            {
                return (IObjectSerializer<T>)_serializers[typeof(T)];
            }
            return new BsonSerializer<T>();
        }
	}
}

