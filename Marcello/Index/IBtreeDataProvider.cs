using System;

namespace Marcello.Index
{
    public interface IBTreeDataProvider<TK, TP>
    {
        Node<TK, TP> GetRootNode(int degree);

        Node<TK, TP> GetNode(Int64 address);

        Node<TK, TP> CreateNode(int degree);
    }
}

