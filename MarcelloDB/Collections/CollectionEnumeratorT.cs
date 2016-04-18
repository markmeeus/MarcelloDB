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
        Collection<T> Collection { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<T> Serializer { get; set; }

        string IndexName { get; set; }

        BTreeWalkerRange<TKey> Range { get; set; }

        bool HasRange{ get { return this.Range != null; } }

        bool IsDescending { get; set; }

        public CollectionEnumerator(
            Collection<T> collection,
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
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (this.Session.SyncLock)
            {
                var index = new RecordIndex<TKey>(
                    this.Session,
                    this.RecordManager,
                    this.IndexName,
                    this.Session.SerializerResolver.SerializerFor<Node<TKey, Int64>>()
                );
                var indexEnumerator = new IndexEntryEnumerator<T, TKey>(
                                      this.Collection,
                                      this.Session,
                                      index,
                                      this.IsDescending);

                indexEnumerator.SetRange(this.Range);

                foreach (var node in indexEnumerator)
                {
                    var record = RecordManager.GetRecord(node.Pointer);
                    var obj = Serializer.Deserialize(record.Data);
                    yield return obj;
                }
            }
        }

        public IEnumerable<TKey> GetKeyEnumerator()
        {
            lock (this.Session.SyncLock)
            {
                var index = new RecordIndex<TKey>(
                    this.Session,
                    this.RecordManager,
                    this.IndexName,
                    this.Session.SerializerResolver.SerializerFor<Node<TKey, Int64>>()
                );
                var keyEnumerator = new KeysEnumerator<T, TKey>(
                                    this.Collection,
                                    this.Session,
                                    index,
                                    this.IsDescending);

                keyEnumerator.SetRange(this.Range);

                return keyEnumerator;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        internal void SetRange(BTreeWalkerRange<TKey> range)
        {
            this.Range = range;
        }
    }
}