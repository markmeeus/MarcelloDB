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

        protected internal abstract object GetKey(object o, Int64 address);

        protected internal string PropertyName { get; set; }
    }

    public class IndexedValue<TObj, TAttribute> : IndexedValue
    {
        Collection<TObj> Collection { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<TObj> Serializer { get; set; }

        Func<TObj, TAttribute> _userValueFunction;

        Func<TObj, TAttribute> _propValueFunction;

        internal IndexedValue(Func<TObj, TAttribute> valueFunction):base(null){
            this._userValueFunction = valueFunction;
        }

        IndexedValue():base(null)
        {
        }

        public IEnumerable<TObj> Find(TAttribute value)
        {
            var index = new RecordIndex<ValueWithAddressIndexKey>(
                this.Session,
                this.RecordManager,
                RecordIndex.GetIndexName<TObj>(this.Collection.Name, this.PropertyName),
                this.Session.SerializerResolver.SerializerFor<Node<ValueWithAddressIndexKey, Int64>>()
            );


            var enumerator =  new CollectionEnumerator<TObj, ValueWithAddressIndexKey>(
                this.Collection, Session, RecordManager, Serializer, index);

            var key = new ValueWithAddressIndexKey{ V = (IComparable)value };

            enumerator.SetRange(key, key);

            return enumerator;
        }

        public IEnumerable<TObj> Find(IEnumerable<TAttribute> values)
        {
            return new List<TObj>{ default(TObj)};
        }

        protected internal override object GetValue(object o)
        {
            return (object)ValueFunction((TObj)o);
        }

        protected internal override object GetKey(object o, Int64 address)
        {
            return new ValueWithAddressIndexKey
            {
                V = (IComparable)GetValue(o),
                A = address
            };
        }

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

        internal static IndexedValue<TObj, TAttribute> Build()
        {
            return new IndexedValue<TObj, TAttribute>();
        }

        internal void SetContext(Collection<TObj> collection,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<TObj> serializer,
            string propertyName)
        {
            this.Collection = collection;
            this.Session = session;
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.PropertyName = propertyName;
        }
    }

    internal class IndexedIDValue<T> : IndexedValue<T, object>
    {
        internal IndexedIDValue():base(null)
        {
            this.PropertyName = "ID";
        }

        internal Func<object, object> IDValueFunction { get; set; }

        protected internal override object GetValue(object o)
        {
            return IDValueFunction(o);
        }

        protected internal override object GetKey(object o, long address)
        {
            //ID Index has it's value as key
            return base.GetValue(o);
        }
    }
}

