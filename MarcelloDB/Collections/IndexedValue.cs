using System;
using System.Linq;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using System.Reflection;

namespace MarcelloDB.Collections
{
    public abstract class IndexedValue : SessionBoundObject
    {
        public IndexedValue(Session session) : base(session){}

        protected internal abstract object GetValue(object o);

        protected internal string PropertyName { get; set; }
    }

    public class IndexedValue<TObj, TAttribute> : IndexedValue
    {
        string CollectionName { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<TObj> Serializer { get; set; }

        Func<TObj, TAttribute> _userValueFunction;

        Func<TObj, TAttribute> _propValueFunction;

        internal Func<TObj, TAttribute> ValueFunction
        {
            get
            {
                if (_userValueFunction != null)
                {
                    return _userValueFunction;
                }
                else
                {
                    return _propValueFunction = _propValueFunction ?? (_propValueFunction = ((TObj o) => {
                        return (TAttribute)(typeof(TObj).GetRuntimeProperty(this.PropertyName)
                            .GetMethod.Invoke(o, new object[0]));
                    }));
                }
            }
        }

        protected internal override object GetValue(object o)
        {
            return (object)ValueFunction((TObj)o);
        }

        public IndexedValue(Func<TObj, TAttribute> valueFunction):base(null){
            this._userValueFunction = valueFunction;
        }

        IndexedValue():base(null)
        {
        }

        internal static IndexedValue<TObj, TAttribute> Build()
        {
            return new IndexedValue<TObj, TAttribute>();
        }

        internal void SetContext( string collectionName,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<TObj> serializer,
            string propertyName)
        {
            this.CollectionName = collectionName;
            this.Session = session;
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.PropertyName = propertyName;
        }

        public IEnumerable<TObj> Find(TAttribute value)
        {
            var index = new RecordIndex<ValueWithAddressIndexKey>(
                this.Session,
                this.RecordManager,
                RecordIndex.GetIndexName<TObj>(this.CollectionName, this.PropertyName),
                this.Session.SerializerResolver.SerializerFor<Node<ValueWithAddressIndexKey, Int64>>()
            );

            var indexKeyWithAddress = new ValueWithAddressIndexKey(){V=(IComparable)value}; //no address matches all
            var adress = index.Search(indexKeyWithAddress);
            if (adress > 0)
            {
                var record = this.RecordManager.GetRecord(adress);
                return new List<TObj>
                {
                    this.Serializer.Deserialize(record.Data)
                };
            }
            return new List<TObj>();
        }

        public IEnumerable<TObj> Find(IEnumerable<TAttribute> values)
        {
            return new List<TObj>{ default(TObj)};
        }
    }

    internal class IndexedIDValue : IndexedValue<object, object>
    {
        internal IndexedIDValue():base(null){}

        internal Func<object, object> IDValueFunction { get; set; }

        protected internal override object GetValue(object o)
        {
            return IDValueFunction(o);
        }
    }
}

