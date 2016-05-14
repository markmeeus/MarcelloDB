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
               
        protected internal abstract void Register(object o, Int64 address);

        protected internal abstract void UnRegister(object o, Int64 address);

        protected internal string PropertyName { get; set; }
    }

    public class IndexedValue<TObj, TAttribute> : IndexedValue
    {
        protected Collection<TObj> Collection { get; set; }

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
            var key = new ValueWithAddressIndexKey<TAttribute>{ V = value };
            return BuildEnumerator(new BTreeWalkerRange<ValueWithAddressIndexKey<TAttribute>>(key, key));
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
                   
        internal virtual object GetKey(object o, Int64 address)
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

    public class IndexedIDValue<TObj> : IndexedValue<TObj, object>
    {
        internal IndexedIDValue():base(null)
        {
            this.PropertyName = "ID";
        }

        internal Func<object, object> IDValueFunction { get; set; }


        internal override object GetKey(object o, long address)
        {
            return IDValueFunction(o);
        }
            
        internal  override void RegisterKey(object key, 
            Int64 address, 
            Session session, 
            RecordManager recordManager, 
            string indexName)
        {
            var index = new RecordIndex<object>(
                this.Session,
                recordManager,
                indexName,
                this.Session.SerializerResolver.SerializerFor<Node<object>>()
            );

            index.Register(key, address);
        } 

        internal  override void UnRegisterKey(object key, 
            Int64 address, 
            Session session, 
            RecordManager recordManager, 
            string indexName)
        {
            var index = new RecordIndex<object>(
                this.Session,
                recordManager,
                indexName,
                this.Session.SerializerResolver.SerializerFor<Node<object>>()
            );

            index.UnRegister(key);
        } 
    }
}

