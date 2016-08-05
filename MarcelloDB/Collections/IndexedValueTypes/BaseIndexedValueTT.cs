using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;
using System.Reflection;
using System.Collections.Generic;
using MarcelloDB.Collections.Scopes;

namespace MarcelloDB.Collections
{
    public class BaseIndexedValue<TObj, TAttribute> : IndexedValue
    {
        protected Collection Collection { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<TObj> Serializer { get; set; }

        Func<TObj, TAttribute> _userValueFunction;

        Func<TObj, TAttribute> _propValueFunction;

        internal BaseIndexedValue(Func<TObj, TAttribute> valueFunction):base(null)
        {
            this._userValueFunction = valueFunction;
        }

        BaseIndexedValue() : base(null){}

        internal override object GetKey(object o, Int64 address)
        {
            return new ValueWithAddressIndexKey<TAttribute>
            {
                V = this.ValueFunction((TObj)o),
                A = address
            };
        }

        protected internal override void Register(object o, Int64 address)
        {
            var key = this.GetKey(o, address);

            var indexName = RecordIndex.GetIndexName<TObj>(this.Collection.Name, this.PropertyName);

            RegisterKey(key, address, this.Session, this.RecordManager, indexName);
        }

        protected internal override void UnRegister(object o, Int64 address)
        {
            var key = this.GetKey(o, address);

            var indexName = RecordIndex.GetIndexName<TObj>(this.Collection.Name, this.PropertyName);

            UnRegisterKey(key, address, this.Session, this.RecordManager, indexName);
        }

        internal virtual void RegisterKey(object key,
            Int64 address,
            Session session,
            RecordManager recordManager,
            string indexName)
        {
            var index = new RecordIndex<ValueWithAddressIndexKey<TAttribute>>(
                this.Session,
                this.RecordManager,
                indexName,
                this.Session.SerializerResolver.SerializerFor<Node<ValueWithAddressIndexKey<TAttribute>>>()
            );

            index.Register((ValueWithAddressIndexKey<TAttribute>)key, address);
        }

        internal virtual void UnRegisterKey(object key,
            Int64 address,
            Session session,
            RecordManager recordManager,
            string indexName)
        {
            var index = new RecordIndex<ValueWithAddressIndexKey<TAttribute>>(
                this.Session,
                this.RecordManager,
                indexName,
                this.Session.SerializerResolver.SerializerFor<Node<ValueWithAddressIndexKey<TAttribute>>>()
            );

            index.UnRegister((ValueWithAddressIndexKey<TAttribute>)key);
        }

        internal CollectionEnumerator<TObj, ValueWithAddressIndexKey<TAttribute>>
        BuildEnumerator(BTreeWalkerRange<ValueWithAddressIndexKey<TAttribute>> range,
            bool IsDescending = false)
        {
            var indexName = RecordIndex.GetIndexName<TObj>(this.Collection.Name, this.PropertyName);

            var enumerator =  new CollectionEnumerator<TObj, ValueWithAddressIndexKey<TAttribute>>(
                this.Collection, Session, RecordManager, Serializer, indexName, IsDescending);

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

        internal void SetContext(Collection collection,
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

        #region scope operations
        public All<TObj, TAttribute> All
        {
            get
            {
                return new All<TObj, TAttribute>(this);
            }
        }
        internal IEnumerable<TObj> FindInternal(TAttribute value)
        {
            var key = new ValueWithAddressIndexKey<TAttribute>{ V = value };
            return BuildEnumerator(new BTreeWalkerRange<ValueWithAddressIndexKey<TAttribute>>(key, key));
        }
        #endregion
    }
}

