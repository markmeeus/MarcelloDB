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

        public IEnumerator<T> GetEnumerator()
        {
            Session.AssertValid();
            lock(Session.SyncLock){
                try{
                    this.Collection.BlockModification = true;
                    var walker = this.Index.GetWalker();

                    if(this.IsDescending)
                    {
                        walker.Reverse();
                    }

                    if(this.HasRange)
                    {
                        walker.SetRange(this.Range);
                    }

                    var node = walker.Next();
                    while (node != null)
                    {
                        var record = RecordManager.GetRecord(node.Pointer);
                        var obj = Serializer.Deserialize(record.Data);
                        yield return obj;
                        node = walker.Next();
                    }
                }
                finally
                {
                    this.Collection.BlockModification = false;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void SetRange(BTreeWalkerRange<TKey> range)
        {
            this.Range = range;
        }
    }
}