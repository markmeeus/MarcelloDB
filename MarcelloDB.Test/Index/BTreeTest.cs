using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Test.Index
{
    [TestFixture]
    public class BTreeTest
    {
        private MockBTreeDataProvider<int, int> _mockDataProvider;
        private const int Degree = 2;

        private readonly int[] testKeyData = new int[] { 10, 20, 30, 50 };
        private readonly int[] testPointerData = new int[] { 50, 60, 40, 20 };

        [SetUp]
        public void Initialize()
        {
            _mockDataProvider = new MockBTreeDataProvider<int, int>();
        }

        [Test]
        public void Create_BTree()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);

            Node<int, int> root = btree.Root;
            Assert.IsNotNull(root);
            Assert.IsNotNull(root.Entries);
            Assert.IsNotNull(root.ChildrenAddresses);
            Assert.AreEqual(0, root.Entries.Count);
            Assert.AreEqual(0, root.ChildrenAddresses.Count);
        }

        [Test]
        public void Insert_One_Node()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);
            this.InsertTestDataAndValidateTree(btree, 0);
            Assert.AreEqual(1, btree.Height);
        }

        [Test]
        public void Insert_Multiple_Nodes_To_Split()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestDataAndValidateTree(btree, i);
            }

            Assert.AreEqual(2, btree.Height);
        }

        [Test]
        public void Insert_Identical_Nodes_Throw_ArgumentException()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);
            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestDataAndValidateTree(btree, i);
            }

            Assert.Throws(typeof(ArgumentException), () =>
                {
                    btree.Insert(this.testKeyData[0], 0);
                });

            Assert.Throws(typeof(ArgumentException), () =>
                {
                    btree.Insert(this.testKeyData[this.testKeyData.Length / 2], 0);
                });
            Assert.Throws(typeof(ArgumentException), () =>
                {
                    btree.Insert(this.testKeyData[this.testKeyData.Length - 1], 0);
                });
        }

        [Test]
        public void Delete_Nodes()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestData(btree, i);
            }

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                btree.Delete(this.testKeyData[i]);
                ValidateTree(btree.Root, Degree, this.testKeyData.Skip(i + 1).ToArray());
            }

            Assert.AreEqual(1, btree.Height);
        }

        [Test]
        public void DeleteNodeBackwards()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestData(btree, i);
            }

            for (int i = this.testKeyData.Length - 1; i > 0; i--)
            {
                btree.Delete(this.testKeyData[i]);
                ValidateTree(btree.Root, Degree, this.testKeyData.Take(i).ToArray());
            }

            Assert.AreEqual(1, btree.Height);
        }

        [Test]
        public void Delete_NonExisting_Node()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestData(btree, i);
            }

            btree.Delete(99999);
            ValidateTree(btree.Root, Degree, this.testKeyData.ToArray());
        }

        [Test]
        public void Remove_From_Last_Node_Which_Reached_Its_Minimum()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);
            var leftNode = _mockDataProvider.CreateNode(Degree);
            var rightNode = _mockDataProvider.CreateNode(Degree);

            btree.Root.ChildrenAddresses.Add(leftNode.Address);
            btree.Root.ChildrenAddresses.Add(rightNode.Address);

            leftNode.Entries.Add(new Entry<int, int> (){ Key = 1, Pointer = 1 });
            leftNode.Entries.Add(new Entry<int, int> (){ Key = 2, Pointer = 2 });
            btree.Root.Entries.Add(new Entry<int, int> (){ Key = 3, Pointer = 3 });
            rightNode.Entries.Add(new Entry<int, int> (){ Key = 4, Pointer = 4 });

            btree.Delete(4);
        }

        [Test]
        public void Search_Nodes()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestData(btree, i);
                this.SearchTestData(btree, i);
            }
        }

        [Test]
        public void Search_NonExisting_Node()
        {
            var btree = new BTree<int, int>(_mockDataProvider, Degree);

            // search an empty tree
            Entry<int, int> nonExisting = btree.Search(9999);
            Assert.IsNull(nonExisting);

            for (int i = 0; i < this.testKeyData.Length; i++)
            {
                this.InsertTestData(btree, i);
                this.SearchTestData(btree, i);
            }

            // search a populated tree
            nonExisting = btree.Search(9999);
            Assert.IsNull(nonExisting);
        }


        #region Private Helper Methods
        private void InsertTestData(BTree<int, int> btree, int testDataIndex)
        {
            btree.Insert(this.testKeyData[testDataIndex], this.testPointerData[testDataIndex]);
        }

        private void InsertTestDataAndValidateTree(BTree<int, int> btree, int testDataIndex)
        {
            btree.Insert(this.testKeyData[testDataIndex], this.testPointerData[testDataIndex]);
            ValidateTree(btree.Root, Degree, this.testKeyData.Take(testDataIndex + 1).ToArray());
        }

        private void SearchTestData(BTree<int, int> btree, int testKeyDataIndex)
        {
            for (int i = 0; i <= testKeyDataIndex; i++)
            {
                Entry<int, int> entry = btree.Search(this.testKeyData[i]);
                Assert.IsNotNull(this.testKeyData[i]);
                Assert.AreEqual(this.testKeyData[i], entry.Key);
                Assert.AreEqual(this.testPointerData[i], entry.Pointer);
            }
        }

        public void ValidateTree(Node<int, int> tree, int degree, params int[] expectedKeys)
        {
            var foundKeys = new Dictionary<int, List<Entry<int, int>>>();
            ValidateSubtree(tree, tree, degree, int.MinValue, int.MaxValue, foundKeys);

            Assert.AreEqual(0, expectedKeys.Except(foundKeys.Keys).Count());
            foreach (var keyValuePair in foundKeys)
            {
                Assert.AreEqual(1, keyValuePair.Value.Count);
            }
        }

        private void UpdateFoundKeys(Dictionary<int, List<Entry<int, int>>> foundKeys, Entry<int, int> entry)
        {
            List<Entry<int, int>> foundEntries;
            if (!foundKeys.TryGetValue(entry.Key, out foundEntries))
            {
                foundEntries = new List<Entry<int, int>>();
                foundKeys.Add(entry.Key, foundEntries);
            }

            foundEntries.Add(entry);
        }

        private void ValidateSubtree(Node<int, int> root, Node<int, int> node, int degree, int nodeMin, int nodeMax, Dictionary<int, List<Entry<int, int>>> foundKeys)
        {
            if (root != node)
            {
                Assert.IsTrue(node.Entries.Count >= degree - 1);
                Assert.IsTrue(node.Entries.Count <= (2 * degree) - 1);
            }

            for (int i = 0; i <= node.Entries.Count; i++)
            {
                int subtreeMin = nodeMin;
                int subtreeMax = nodeMax;

                if (i < node.Entries.Count)
                {
                    var entry = node.Entries[i];
                    UpdateFoundKeys(foundKeys, entry);
                    Assert.IsTrue(entry.Key >= nodeMin && entry.Key <= nodeMax);

                    subtreeMax = entry.Key;
                }

                if (i > 0)
                {
                    subtreeMin = node.Entries[i - 1].Key;
                }

                if (!node.IsLeaf)
                {
                    Assert.IsTrue(node.ChildrenAddresses.Count >= degree);
                    ValidateSubtree(root, _mockDataProvider.GetNode(node.ChildrenAddresses[i]), degree, subtreeMin, subtreeMax, foundKeys);
                }
            }
        }
        #endregion
    }
}