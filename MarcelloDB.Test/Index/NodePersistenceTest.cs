using System;
using NUnit.Framework;
using MarcelloDB.Records;
using System.Collections.Generic;
using MarcelloDB.Index;
using MarcelloDB.Serialization;
using System.Linq;

namespace MarcelloDB.Test.Index
{
    [TestFixture]
    public class NodePersistenceTest
    {
        class MockSerializer<T> : IObjectSerializer<T>
        {
            public Func<T, byte[]>  SerializeFunc { get; set; }
            public Func<byte[], T> DeserializeFunc { get; set; }

            #region IObjectSerializer implementation
            public byte[] Serialize(T obj)
            {
                return this.SerializeFunc(obj);
            }
            public T Deserialize(byte[] bytes)
            {
                return this.DeserializeFunc(bytes);
            }
            #endregion

        }

        Session _session;

        NodePersistence<int> _persistence;

        InMemoryRecordManager _inMemoryRecordManager;

        [SetUp]
        public void Initialize()
        {
            _session = new Session(new TestPlatform(), "");
            _inMemoryRecordManager = new InMemoryRecordManager();
            _persistence = new NodePersistence<int>(_session, _inMemoryRecordManager);
        }

        [Test]
        public void Appends_A_New_Node()
        {
            var node = new Node<int>(2);
            node.ChildrenAddresses.Added.Add(1); // to make dirty
            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[]{ 1, 2, 3 };

            _persistence.Persist(node, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());
            Assert.AreEqual(new byte[]{ 1, 2, 3 }, _inMemoryRecordManager._records.Values.First().Data);
        }

        [Test]
        public void Sets_Address_Of_New_node()
        {
            var node = new Node<int>(2);
            node.ChildrenAddresses.Added.Add(1); // to make dirty
            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[]{ 1, 2, 3 };

            _persistence.Persist(node, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());
            Assert.AreEqual(_inMemoryRecordManager._records.Keys.First(), node.Address);
        }

        [Test]
        public void Updates_A_Node()
        {
            var node = new Node<int>(2);
            node.ChildrenAddresses.Added.Add(1); // to make it dirty
            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[]{ 1, 2, 3 };
            _persistence.Persist(node, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());

            node.ChildrenAddresses.Added.Add(2); // to make it dirty again
            serializer.SerializeFunc = (obj) => new byte[] { 4, 5, 6 };
            _persistence.Persist(node, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());

            Assert.AreEqual(new byte[]{ 4, 5, 6 }, _inMemoryRecordManager._records.Values.First().Data);
        }

        [Test]
        public void Updates_Nodes_Address_If_Record_Moved()
        {
            var node = new Node<int>(2);
            node.ChildrenAddresses.Added.Add(1); // to make dirty
            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[]{ 1, 2, 3 };
            _persistence.Persist(node, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());

            serializer.SerializeFunc = (obj) => new byte[1000];
            _persistence.Persist(node, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());

            Assert.AreEqual(_inMemoryRecordManager._records.Keys.Last(), node.Address);
        }

        [Test]
        public void Does_Not_Save_NonDirty_Node()
        {
            var node = new Node<int>(2);
            node.ChildrenAddresses.Added.Add(1); // to make dirty
            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[]{ 1, 2, 3 };
            _persistence.Persist(node, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());

            node.ClearChanges();
            serializer.SerializeFunc = (obj) => new byte[]{ 4, 5, 6 };
            _persistence.Persist(node, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());

            Assert.AreEqual(new byte[]{ 1, 2, 3 }, _inMemoryRecordManager._records[node.Address].Data);
        }

        [Test]
        public void Saves_Child_Nodes()
        {
            Node<int> node;
            Node<int> childNode;

            CreateParentAndChildNode(out node, out childNode);

            var loadedNodes = new Dictionary<long, Node<int>>();
            loadedNodes[node.Address] = node;
            loadedNodes[childNode.Address] = childNode;

            childNode.ChildrenAddresses.RemoveAt(0); //makes it dirty

            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[]{ 4, 5, 6 };
            _persistence.Persist(node, loadedNodes, serializer, new IndexMetaRecord());

            Assert.AreEqual(new byte[]{ 4, 5, 6 }, _inMemoryRecordManager._records[childNode.Address].Data);
        }

        [Test]
        public void Resets_Dirty_Flags_For_Saved_Nodes()
        {
            Node<int> node;
            Node<int> childNode;

            CreateParentAndChildNode(out node, out childNode);

            var loadedNodes = new Dictionary<long, Node<int>>();
            loadedNodes[node.Address] = node;
            loadedNodes[childNode.Address] = childNode;

            childNode.ChildrenAddresses.RemoveAt(0); //makes it dirty

            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[]{ 4, 5, 6 };
            _persistence.Persist(node, loadedNodes, serializer, new IndexMetaRecord());

            Assert.AreEqual(0, loadedNodes.Values.Where(n => n.Dirty).Count());
        }

        [Test]
        public void Updates_Child_Address_When_Child_Moved()
        {
            Node<int> node;
            Node<int> childNode;

            CreateParentAndChildNode(out node, out childNode);

            var loadedNodes = new Dictionary<long, Node<int>>();
            loadedNodes[node.Address] = node;
            loadedNodes[childNode.Address] = childNode;


            childNode.ChildrenAddresses.RemoveAt(0); //makes it dirty

            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[1000];
            _persistence.Persist(node, loadedNodes, serializer, new IndexMetaRecord());

            Assert.AreEqual(childNode.Address, node.ChildrenAddresses.Last());

        }

        [Test]
        public void Deletes_Child_Node_When_Not_Reachable()
        {
            Node<int> node;
            Node<int> childNode;

            CreateParentAndChildNode(out node, out childNode);

            var loadedNodes = new Dictionary<long, Node<int>>();
            loadedNodes[node.Address] = node;
            loadedNodes[childNode.Address] = childNode;

            childNode.ChildrenAddresses.RemoveAt(0); //makes it dirty
            node.ChildrenAddresses.RemoveRange(0, node.ChildrenAddresses.Count); //remove any children

            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[1000];
            _persistence.Persist(node, loadedNodes, serializer, new IndexMetaRecord());

            Assert.IsFalse(_inMemoryRecordManager._records.ContainsKey(childNode.Address));

        }

        void CreateParentAndChildNode(out Node<int> parentNode, out Node<int> childNode)
        {
            parentNode = new Node<int>(2);
            childNode = new Node<int>(2);
            var serializer = new MockSerializer<Node<int>>();
            serializer.SerializeFunc = (obj) => new byte[]{ 1, 2, 3 };

            parentNode.ChildrenAddresses.Add(100000);
            childNode.ChildrenAddresses.Add(100002);

            _persistence.Persist(parentNode, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());
            _persistence.Persist(childNode, new Dictionary<long, Node<int>>(), serializer, new IndexMetaRecord());
            //add the child to the parent
            parentNode.ChildrenAddresses.Add(childNode.Address);

        }
    }
}

