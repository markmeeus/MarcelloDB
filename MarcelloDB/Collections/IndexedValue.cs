using System;
using System.Linq;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using System.Reflection;
using System.Collections;
using MarcelloDB.Index.BTree;
using MarcelloDB.Collections.Scopes;

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

        internal IndexedValue(Func<TObj, TAttribute> valueFunction):base(null)
        {
            this._userValueFunction = valueFunction;
        }

        IndexedValue():base(null)
        {
        }

        public IEnumerable<TObj> Find(TAttribute value)
        {
            var key = new ValueWithAddressIndexKey{ V = (IComparable)value };
            return BuildEnumerator(new BTreeWalkerRange<ValueWithAddressIndexKey>(key, key));
        }

        public All<TObj, TAttribute> All
        {
            get
            {
                return new All<TObj, TAttribute>(this);
            }
        }

        public BetweenBuilder<TObj, TAttribute> Between(TAttribute startValue)
        {
            return new BetweenBuilder<TObj, TAttribute>(this, startValue, false);
        }

        public BetweenBuilder<TObj, TAttribute> BetweenIncluding(TAttribute startValue)
        {
            return new BetweenBuilder<TObj, TAttribute>(this, startValue, true);
        }

        public GreaterThan<TObj, TAttribute> GreaterThan(TAttribute value)
        {
            return new GreaterThan<TObj, TAttribute>(this, value, false);
        }

        public GreaterThan<TObj, TAttribute> GreaterThanOrEqual(TAttribute value)
        {
            return new GreaterThan<TObj, TAttribute>(this, value, true);
        }

        public SmallerThan<TObj, TAttribute> SmallerThan(TAttribute value)
        {
            return new SmallerThan<TObj, TAttribute>(this, value, false);
        }

        public SmallerThan<TObj, TAttribute> SmallerThanOrEqual(TAttribute value)
        {
            return new SmallerThan<TObj, TAttribute>(this, value, true);
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

        internal CollectionEnumerator<TObj, ValueWithAddressIndexKey>
            BuildEnumerator(BTreeWalkerRange<ValueWithAddressIndexKey> range,
            bool IsDescending = false)
        {

            var index = new RecordIndex<ValueWithAddressIndexKey>(
                this.Session,
                this.RecordManager,
                RecordIndex.GetIndexName<TObj>(this.Collection.Name, this.PropertyName),
                this.Session.SerializerResolver.SerializerFor<Node<ValueWithAddressIndexKey, Int64>>()
            );

            var enumerator =  new CollectionEnumerator<TObj, ValueWithAddressIndexKey>(
                this.Collection, Session, RecordManager, Serializer, index, IsDescending);

            enumerator.SetRange(range);

            return enumerator;
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

