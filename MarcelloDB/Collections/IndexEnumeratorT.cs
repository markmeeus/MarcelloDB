using System;
using System.Linq;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;

namespace MarcelloDB.Collections
{
    public class IndexAccessor<T, TIndexKey> : SessionBoundObject
    {
        Collection<T> Collection { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<T> Serializer { get; set; }

        string IndexName {get; set;}

        internal IndexAccessor(
            Collection<T> collection,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<T> serializer,
            string indexName) : base(session)
        {
            this.Collection = collection;
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.IndexName = indexName;
        }

        public IEnumerable<T> Find(TIndexKey indexKey)
        {
            var index = new RecordIndex<TIndexKey>(
                this.Session,
                this.RecordManager,
                RecordIndex.GetIndexName<T>(Collection.Name, this.IndexName),
                this.Session.SerializerResolver.SerializerFor<Node<TIndexKey, Int64>>()
            );
            var adress = index.Search(indexKey);
            if (adress > 0)
            {
                var record = this.RecordManager.GetRecord(adress);
                return new List<T>
                {
                    this.Session.SerializerResolver.SerializerFor<T>().Deserialize(record.Data)
                };
            }
            return new List<T>(){default(T)};
        }
    }
}

