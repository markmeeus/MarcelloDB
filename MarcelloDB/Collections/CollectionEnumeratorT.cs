using System;
using System.Collections;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Collections
{
    internal class CollectionEnumerator<T, TKey> : SessionBoundObject, IEnumerable<T>
	{
        internal Func<Int64, bool> ShouldYieldObjectWithAddress { get; set; }

        internal Func<T, bool> ShouldYieldObject { get; set; }

        Collection Collection { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<T> Serializer { get; set; }

        string IndexName { get; set; }

        IEnumerable<BTreeWalkerRange<TKey>> Ranges { get; set; }

        bool IsDescending { get; set; }

        public CollectionEnumerator(
            Collection collection,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<T> serializer,
            string indexName,
            bool IsDescending = false
        ) : base(session)
        {
            this.Collection = collection;
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.IndexName = indexName;
            this.IsDescending = IsDescending;

            this.Ranges = new BTreeWalkerRange<TKey>[]{ null };

            //Yield all records and objects by default.
            this.ShouldYieldObject = (o) => true;
            this.ShouldYieldObjectWithAddress = (a) => true;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (this.Session.SyncLock)
            {
                foreach (var range in this.Ranges)
                {
                    var index = new RecordIndex<TKey>(
                        this.Session,
                        this.RecordManager,
                        this.IndexName,
                        this.Session.SerializerResolver.SerializerFor<Node<TKey>>()
                    );
                    var indexEnumerator = new IndexEntryEnumerator<T, TKey>(
                        this.Collection,
                        this.Session,
                        index,
                        this.IsDescending);

                    indexEnumerator.SetRange(range);

                    foreach (var node in indexEnumerator)
                    {
                        if(this.ShouldYieldObjectWithAddress(node.Pointer))
                        {
                            var record = RecordManager.GetRecord(node.Pointer);
                            var obj = Serializer.Deserialize(record.Data);
                            if(this.ShouldYieldObject(obj))
                            {
                                yield return obj;
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<TKey> GetKeyEnumerator()
        {
            lock (this.Session.SyncLock)
            {
                foreach (var range in this.Ranges)
                {
                    var index = new RecordIndex<TKey>(
                        this.Session,
                        this.RecordManager,
                        this.IndexName,
                        this.Session.SerializerResolver.SerializerFor<Node<TKey>>()
                    );
                    var keyEnumerator = new KeysEnumerator<T, TKey>(
                        this.Collection,
                        this.Session,
                        index,
                        this.IsDescending);

                    keyEnumerator.SetRange(range);
                    foreach (var key in keyEnumerator)
                    {
                        yield return key;
                    }

                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        internal void SetRanges(IEnumerable<BTreeWalkerRange<TKey>> ranges)
        {
            this.Ranges = ranges;
        }
    }
}