using System;
using MarcelloDB.Index;
using System.Collections.Generic;

namespace MarcelloDB.Serialization
{
    internal class BTreeNodeSerializer<TK> : IObjectSerializer<Node<TK>>
    {
        BsonSerializer<BTreeNodeData<TK>> DataSerializer { get; set; }

        public class BTreeNodeData<TKey>
        {
            public List<Entry<TKey>> Entries { get; set; }
            public List<Int64> ChildrenAddresses { get; set; }
            public int Degree { get; set; }
        }

        #region IObjectSerializer implementation

        internal BTreeNodeSerializer()
        {
            this.DataSerializer = new BsonSerializer<BTreeNodeData<TK>>();
        }

        public byte[] Serialize(Node<TK> node)
        {
            var data = new BTreeNodeData<TK>();
            data.Entries = node.EntryList.Entries;
            data.ChildrenAddresses = node.ChildrenAddresses.Addresses;
            data.Degree = node.Degree;
            return this.DataSerializer.Serialize(data);
        }

        public Node<TK> Deserialize(byte[] bytes)
        {
            var data = this.DataSerializer.Deserialize(bytes);
            var node = new Node<TK>(data.Degree);
            node.EntryList.SetEntries(data.Entries);
            node.ChildrenAddresses.SetAddresses(data.ChildrenAddresses);

            return node;
        }

        #endregion
    }
}

