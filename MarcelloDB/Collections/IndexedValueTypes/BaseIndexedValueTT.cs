using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;
using System.Collections.Generic;
using MarcelloDB.Collections.Scopes;
using System.Linq;

namespace MarcelloDB.Collections
{
    public abstract class BaseIndexedValue<TObj, TAttribute> : IndexedValue
    {
        protected Collection Collection { get; set; }

        protected Func<TObj, IEnumerable<TAttribute>> UserValueFunction { get; set; }

        Func<TObj, bool> ShouldIndexPredicate { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<TObj> Serializer { get; set; }

        internal BaseIndexedValue(
            Func<TObj, IEnumerable<TAttribute>> valueFunction,
            Func<TObj, bool> shouldIndexPredicate)
        :base(null)
        {
            this.UserValueFunction = valueFunction;
            this.ShouldIndexPredicate = shouldIndexPredicate;
        }

        internal override object Build ()
        {
            throw new NotImplementedException ("Build not implemented in subclass.");
        }
        BaseIndexedValue () : base(null){}

        internal override IEnumerable<object> GetKeys(object o, Int64 address)
        {
            return this.ValueFunction((TObj)o)
                .Select((v) => new ValueWithAddressIndexKey<TAttribute>
                {
                    V = v,
                    A = address
                }
            );
        }

        protected internal override void Register(object o, Int64 address)
        {
            if (this.ShouldIndex((TObj)o))
            {
                var indexName = RecordIndex.GetIndexName<TObj>(this.Collection.Name, this.PropertyName, typeof(TAttribute));
                var keys = this.GetKeys(o, address);
                foreach (var key in keys)
                {
                    RegisterKey(key, address, this.Session, this.RecordManager, indexName);
                }
            }
        }

        protected internal override void UnRegister(object o, Int64 address)
        {
            var indexName = RecordIndex.GetIndexName<TObj>(this.Collection.Name, this.PropertyName, typeof(TAttribute));
            var keys = this.GetKeys(o, address);
            foreach (var key in keys)
            {
                UnRegisterKey(key, address, this.Session, this.RecordManager, indexName);
            }

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

        protected internal override void EnsureIndex()
        {
            var indexName = RecordIndex.GetIndexName<TObj> (this.Collection.Name, this.PropertyName, typeof (TAttribute));
            var index = new RecordIndex<TAttribute> (
                this.Session,
                this.RecordManager,
                indexName,
                this.Session.SerializerResolver.SerializerFor<Node<TAttribute>> ()
            );
            //Creating a walker makes sure the index is ready
            index.GetWalker();
        }


        internal CollectionEnumerator<TObj, ValueWithAddressIndexKey<TAttribute>>
        BuildEnumerator(IEnumerable<BTreeWalkerRange<ValueWithAddressIndexKey<TAttribute>>> ranges,
            bool IsDescending = false)
        {
            var indexName = RecordIndex.GetIndexName<TObj>(this.Collection.Name, this.PropertyName, typeof(TAttribute));

            var enumerator =  new CollectionEnumerator<TObj, ValueWithAddressIndexKey<TAttribute>>(
                this.Collection, Session, RecordManager, Serializer, indexName, IsDescending);

            enumerator.SetRanges(ranges);

            return enumerator;
        }

        internal virtual Func<TObj, IEnumerable<TAttribute>> ValueFunction
        {
            get
            {
                return this.UserValueFunction;
            }
        }

        override internal void SetContext(Collection collection,
            Session session,
            RecordManager recordManager,
            object serializer,
            string propertyName)
        {
            this.Collection = collection;
            this.Session = session;
            this.RecordManager = recordManager;
            this.Serializer = (IObjectSerializer<TObj>)serializer;
            this.PropertyName = propertyName;
        }

        protected Func<long, bool> CreateDuplicateAddressFilter()
        {
            var enumeratedAddresses = new HashSet<Int64>();
            return (Int64 address) =>
            {
                if (enumeratedAddresses.Contains(address))
                {
                    return false;
                }
                enumeratedAddresses.Add(address);
                return true;
            };
        }

        bool ShouldIndex(TObj o)
        {
            return this.ShouldIndexPredicate == null || this.ShouldIndexPredicate(o);
        }

        #region scope operations
        public All<TObj, TAttribute> All
        {
            get
            {
                return new All<TObj, TAttribute>(this);
            }
        }
        internal CollectionEnumerator<TObj, ValueWithAddressIndexKey<TAttribute>> EqualsInternal(IEnumerable<TAttribute> values)
        {
            var ranges = values.Select((value) => {
                var key = new ValueWithAddressIndexKey<TAttribute>{ V = value };
                return new BTreeWalkerRange<ValueWithAddressIndexKey<TAttribute>>(key, key);
            });

            return BuildEnumerator(ranges);
        }

        internal GreaterThan<TObj, TAttribute> GreaterThanInternal(TAttribute value){
            return new GreaterThan<TObj, TAttribute>(this, value, false);
        }

        internal GreaterThan<TObj, TAttribute> GreaterThanOrEqualInternal(TAttribute value){
            return new GreaterThan<TObj, TAttribute>(this, value, true);
        }

        internal SmallerThan<TObj, TAttribute> SmallerThanInternal(TAttribute value)
        {
            return new SmallerThan<TObj, TAttribute>(this, value, false);
        }

        internal SmallerThan<TObj, TAttribute> SmallerThanOrEqualInternal(TAttribute value)
        {
            return new SmallerThan<TObj, TAttribute>(this, value, true);
        }
        #endregion
    }
}

