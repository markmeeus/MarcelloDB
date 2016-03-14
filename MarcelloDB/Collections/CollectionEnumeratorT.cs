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

        RecordIndex<TKey> Index { get; set; }

        BTreeWalkerRange<TKey> Range { get; set; }

        bool HasRange{ get { return this.Range != null; } }

        bool IsDescending { get; set; }

        public CollectionEnumerator(
            Collection<T> collection,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<T> serializer,
            RecordIndex<TKey> index,
            bool IsDescending = false
        ) : base(session)
        {
            this.Collection = collection;
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.Index = index;
            this.IsDescending = IsDescending;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var indexEnumerator = new IndexEntryEnumerator<T, TKey>(
                this.Collection,
                this.Session,
                this.Index,
                this.IsDescending);

            indexEnumerator.SetRange(this.Range);

            foreach (var node in indexEnumerator)
            {
                var record = RecordManager.GetRecord(node.Pointer);
                var obj = Serializer.Deserialize(record.Data);
                yield return obj;
            }
        }

        public IEnumerable<TKey> GetKeyEnumerator()
        {
            return new KeysEnumerator<T, TKey>(
               this.Collection,
                this.Session,
                this.Index,
                this.IsDescending);
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