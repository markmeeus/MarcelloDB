using System;
using System.Collections;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;

namespace MarcelloDB.Collections
{
    internal class CollectionEnumerator<T, TKey> : SessionBoundObject, IEnumerable<T>
	{
        Collection<T> Collection { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<T> Serializer { get; set; }

        RecordIndex<TKey> Index { get; set; }

        TKey MatchValue { get; set; }

        TKey StartAt { get; set; }

        TKey EndAt { get; set; }

        bool UseRange { get; set; }

        public CollectionEnumerator(
            Collection<T> collection,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<T> serializer,
            RecordIndex<TKey> index
        ) : base(session)
        {
            this.Collection = collection;
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.Index = index;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Session.AssertValid();
            lock(Session.SyncLock){
                try{
                    this.Collection.BlockModification = true;
                    var walker = this.Index.GetWalker();
                    if(this.UseRange)
                    {
                        walker.SetRange(this.StartAt, this.EndAt);
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

        internal void SetRange(TKey startAt, TKey endAt)
        {
            this.StartAt = startAt;
            this.EndAt = endAt;
            this.UseRange = true;
        }

    }
}