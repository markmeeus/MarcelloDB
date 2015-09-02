using System;
using System.Collections.Generic;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Index.BTree
{
    internal interface IBTreeDataProvider<TK, TP>
    {
        Node<TK, TP> GetRootNode(int degree);

        void SetRootNode(Node<TK, TP> node);

        Node<TK, TP> GetNode(Int64 address);

        Node<TK, TP> CreateNode(int degree);

        void Flush();
    }
}

