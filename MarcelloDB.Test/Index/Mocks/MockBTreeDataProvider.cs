﻿using System;
using MarcelloDB.Index;
using System.Collections.Generic;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Test
{
    internal class MockBTreeDataProvider<TK, TP> : IBTreeDataProvider<TK, TP>
    {
        public bool WasFlushed { get; set; }

        Int64 _lastAddress = 1;
        Dictionary<Int64, Node<TK, TP>> _nodes = new Dictionary<long, Node<TK, TP>>();

        Node<TK, TP> _rootNode;

        public Int64 RootNodeAddress { get; set; }

        Node<TK, TP> IBTreeDataProvider<TK, TP>.GetRootNode(int degree)
        {
            if (_rootNode == null)
            {
                _rootNode = new Node<TK, TP>(degree);
                _rootNode.Address = 0;
                AddNode(_rootNode);
            }
            return _rootNode;
        }

        void IBTreeDataProvider<TK, TP>.SetRootNode(Node<TK, TP> rootNode)
        {
            _rootNode = rootNode;
            this.RootNodeAddress = rootNode.Address;
        }

        Node<TK, TP> IBTreeDataProvider<TK, TP>.GetNode(Int64 address)
        {
            return _nodes[address];
        }

        Node<TK, TP> IBTreeDataProvider<TK, TP>.CreateNode(int degree)
        {
            var node = new Node<TK, TP>(degree);
            node.Address = _lastAddress++;
            AddNode(node);
            return node;
        }

        void AddNode(Node<TK, TP> node)
        {
            _nodes.Add(node.Address, node);
        }


        public void Flush()
        {
            this.WasFlushed = true;
        }
    }
}

