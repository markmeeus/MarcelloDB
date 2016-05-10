using System;
using MarcelloDB.Records;
using MarcelloDB.Index;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace MarcelloDB.Serialization
{
	internal class SerializerResolver
	{
        Dictionary<Type, object> _serializers;

        internal SerializerResolver()
        {
            _serializers = new Dictionary<Type, object> {
                {typeof(IndexMetaRecord),                   new  IndexMetaRecordSerializer()},
                {typeof(Node<EmptyRecordIndexKey>),  new EmptyRecordIndexNodeSerializer()},
            };
        }

        internal IObjectSerializer<T> SerializerFor<T>()
        {
            if (!_serializers.ContainsKey(typeof(T)))
            {
                _serializers[typeof(T)] = ConstructSerializer<T>();
            }
            return (IObjectSerializer<T>)_serializers[typeof(T)];
        }

        IObjectSerializer<T> ConstructSerializer<T>()
        {
            if(typeof(T).GetTypeInfo().IsSubclassOf(typeof(Node)))
            {
                return ConstructBTreeNodeSerializer<T>();
            }
            return new BsonSerializer<T>();
        }

        IObjectSerializer<T> ConstructBTreeNodeSerializer<T>()
        {
            var genericTypes = typeof(T).GenericTypeArguments;
            var typeInfo = typeof(BTreeNodeBsonSerializer<>).GetTypeInfo();
            var genericType = typeInfo.MakeGenericType(genericTypes);

            var constructor = genericType.GetTypeInfo().DeclaredConstructors.First();
            return (IObjectSerializer<T>)constructor.Invoke(new object[0]);
        }
	}
}

