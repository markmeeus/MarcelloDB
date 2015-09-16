using System;
using MarcelloDB.Index;
using System.Collections.Generic;

namespace MarcelloDB.Serialization
{
    internal class BTreeNodeSerializer<TK, TP> : IObjectSerializer<Node<TK, TP>>
    {
        public class BTreeNodeData<TKey,TPointer>
        {
            public List<Entry<TKey,TPointer>> Entries { get; set; }
            public List<Int64> ChildrenAddresses { get; set; }
            public int Degree { get; set; }
        }

        #region IObjectSerializer implementation

        public byte[] Serialize(Node<TK, TP> node)
        {
            var data = new BTreeNodeData<TK, TP>();
            data.Entries = node.EntryList.Entries;
            data.ChildrenAddresses = node.ChildrenAddresses.Addresses;
            data.Degree = node.Degree;
            return new BsonSerializer<BTreeNodeData<TK, TP>>().Serialize(data);
        }

        public Node<TK, TP> Deserialize(byte[] bytes)
        {
            var data = new BsonSerializer<BTreeNodeData<TK, TP>>().Deserialize(bytes);
            var node = new Node<TK,TP>(data.Degree);
            node.EntryList.SetEntries(data.Entries);
            node.ChildrenAddresses.SetAddresses(data.ChildrenAddresses);

            return node;
        }

        #endregion
    }
}

