using System;
using MarcelloDB.Index;
using System.Collections.Generic;

namespace MarcelloDB.Serialization
{
    internal class BTreeNodeBsonSerializer<TK> : IObjectSerializer<Node<TK>>
    {
        BsonSerializer<BTreeNodeData<TK>> DataSerializer { get; set; }

        public class BTreeNodeData<TKey>
        {
            public List<Entry<TKey>> Entries { get; set; }
            public List<Int64> ChildrenAddresses { get; set; }
        }

        #region IObjectSerializer implementation

        public BTreeNodeBsonSerializer()
        {
            this.DataSerializer = new BsonSerializer<BTreeNodeData<TK>>();
        }

        public byte[] Serialize(Node<TK> node)
        {
            var data = new BTreeNodeData<TK>();
            data.Entries = node.EntryList.Entries;
            data.ChildrenAddresses = node.ChildrenAddresses.Addresses;
            return this.DataSerializer.Serialize(data);
        }

        public Node<TK> Deserialize(byte[] bytes)
        {
            var data = this.DataSerializer.Deserialize(bytes);
            var node = new Node<TK>(RecordIndex.BTREE_DEGREE);
            node.EntryList.SetEntries(data.Entries);
            node.ChildrenAddresses.SetAddresses(data.ChildrenAddresses);

            return node;
        }

        #endregion
    }
}

