using System;
using System.Collections;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;

namespace MarcelloDB.Collections
{
    internal class CollectionEnumerator<T> : SessionBoundObject, IEnumerable<T>
	{
        Collection<T> Collection { get; set; }

        RecordManager RecordManager  { get; set; }

        IObjectSerializer<T> Serializer { get; set; }


        public CollectionEnumerator(
            Collection<T> collection,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<T> serializer) : base(session)
        {
            this.Collection = collection;
            this.RecordManager = recordManager;
            this.Serializer = serializer;
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock(Session.SyncLock){
                try{
                    this.Collection.BlockModification = true;
                    var index = new RecordIndex<object>(
                        this.Session,
                        this.RecordManager,
                        RecordIndex.GetIDIndexName<T>(Collection.Name),
                        this.Session.SerializerResolver.SerializerFor<Node<object, Int64>>()
                    );
                    var walker = index.GetWalker();
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
    }
}