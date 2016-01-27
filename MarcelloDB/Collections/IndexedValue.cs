using System;
using System.Linq;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;

namespace MarcelloDB.Collections
{
    public class IndexedValue<TObj, TAttribute> : SessionBoundObject
    {
        string CollectionName { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<TObj> Serializer { get; set; }

        string IndexName {get; set;}


        internal Func<TObj, TAttribute> ValueFunction { get; set; }

        public IndexedValue(Func<TObj, TAttribute> valueFunction):base(null){
            this.ValueFunction = valueFunction;
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
            string indexName)
        {
            this.CollectionName = collectionName;
            this.Session = session;
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.IndexName = indexName;
        }

        public IEnumerable<TObj> Find(TAttribute value)
        {
            var index = new RecordIndex<ValueWithAddressIndexKey>(
                this.Session,
                this.RecordManager,
                RecordIndex.GetIndexName<TObj>(this.CollectionName, this.IndexName),
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
}

