using System;
using MarcelloDB.Index;

namespace MarcelloDB.Serialization
{
    public class BtreeNodeSerializer<TK, TP> : IObjectSerializer<Node<TK, TP>>
    {
        public BtreeNodeSerializer()
        {
        }

        #region IObjectSerializer implementation

        public byte[] Serialize(Node<TK, TP> obj)
        {
            return new BsonSerializer<Node<TK,TP>>().Serialize(obj);
        }

        public Node<TK, TP> Deserialize(byte[] bytes)
        {
            return new BsonSerializer<Node<TK,TP>>().Deserialize(bytes);
        }

        #endregion
    }
}

