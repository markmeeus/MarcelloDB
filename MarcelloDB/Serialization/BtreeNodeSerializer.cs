using System;
using MarcelloDB.Index;
using System.Collections.Generic;

namespace MarcelloDB.Serialization
{
    internal class BTreeNodeSerializer<TK, TP> : IObjectSerializer<Node<TK, TP>>
    {
        BsonSerializer<BTreeNodeData<TK, TP>> DataSerializer { get; set; }

        public class BTreeNodeData<TKey,TPointer>
        {
            public List<Entry<TKey,TPointer>> Entries { get; set; }
            public List<Int64> ChildrenAddresses { get; set; }
            public int Degree { get; set; }
        }

        #region IObjectSerializer implementation

        internal BTreeNodeSerializer()
        {
            this.DataSerializer = new BsonSerializer<BTreeNodeData<TK, TP>>();
        }

        public byte[] Serialize(Node<TK, TP> node)
        {
            var data = new BTreeNodeData<TK, TP>();
            data.Entries = node.EntryList.Entries;
            data.ChildrenAddresses = node.ChildrenAddresses.Addresses;
            data.Degree = node.Degree;
            return this.DataSerializer.Serialize(data);
        }

        public Node<TK, TP> Deserialize(byte[] bytes)
        {
            var data = this.DataSerializer.Deserialize(bytes);
            var node = new Node<TK,TP>(data.Degree);
            node.EntryList.SetEntries(data.Entries);
            node.ChildrenAddresses.SetAddresses(data.ChildrenAddresses);

            return node;
        }

        #endregion
    }
}

