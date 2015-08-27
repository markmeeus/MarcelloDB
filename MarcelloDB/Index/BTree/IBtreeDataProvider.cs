using System;
using System.Collections.Generic;

namespace MarcelloDB.Index.BTree
{
    public interface IBTreeDataProvider<TK, TP>
    {
        Node<TK, TP> GetRootNode(int degree);

        void SetRootNode(Node<TK, TP> node);

        Node<TK, TP> GetNode(Int64 address);

        Node<TK, TP> CreateNode(int degree);

        void Flush();
    }
}

