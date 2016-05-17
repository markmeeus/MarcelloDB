using System;
using System.Collections.Generic;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Index.BTree
{
    internal interface IBTreeDataProvider<TK>
    {
        Node<TK> GetRootNode(int degree);

        void SetRootNode(Node<TK> node);

        Node<TK> GetNode(Int64 address);

        Node<TK> CreateNode(int degree);

        void Flush();
    }
}

