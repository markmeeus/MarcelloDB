using System;
using MarcelloDB.Index;
using System.Collections.Generic;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Test
{
    internal class MockBTreeDataProvider<TK> : IBTreeDataProvider<TK>
    {
        public bool WasFlushed { get; set; }

        Int64 _lastAddress = 1;
        Dictionary<Int64, Node<TK>> _nodes = new Dictionary<long, Node<TK>>();

        Node<TK> _rootNode;

        public Int64 RootNodeAddress { get; set; }

        Node<TK> IBTreeDataProvider<TK>.GetRootNode(int degree)
        {
            if (_rootNode == null)
            {
                _rootNode = new Node<TK>(degree);
                _rootNode.Address = 0;
                AddNode(_rootNode);
            }
            return _rootNode;
        }

        void IBTreeDataProvider<TK>.SetRootNode(Node<TK> rootNode)
        {
            _rootNode = rootNode;
            this.RootNodeAddress = rootNode.Address;
        }

        Node<TK> IBTreeDataProvider<TK>.GetNode(Int64 address)
        {
            return _nodes[address];
        }

        Node<TK> IBTreeDataProvider<TK>.CreateNode(int degree, bool allowRecordReuse = true)
        {
            var node = new Node<TK>(degree);
            node.Address = _lastAddress++;
            AddNode(node);
            return node;
        }

        void AddNode(Node<TK> node)
        {
            _nodes.Add(node.Address, node);
        }

        public void Flush()
        {
            this.WasFlushed = true;
        }
    }
}

