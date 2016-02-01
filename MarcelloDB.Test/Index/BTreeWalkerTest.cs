using NUnit.Framework;
using System.Collections.Generic;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;
using System.Linq;

namespace MarcelloDB.Test.Index
{

	[TestFixture]
	class BTreeWalkerTest
	{
        MockBTreeDataProvider<int,int> _mockDataProvider;

        BTreeWalker<int,int> _walker;

        int _degree = 2;

		[SetUp]
		public void Initialize()
        {
            _mockDataProvider = new MockBTreeDataProvider<int, int>();
            _walker = new BTreeWalker<int, int>(_degree, _mockDataProvider);
		}

        [Test]
		public void Returns_Null_When_No_Entries()
        {
            var result = _walker.Next();
            Assert.IsNull(result);
		}

        [Test]
        public void Returns_First_Item()
        {
            var rootNode = _mockDataProvider.GetRootNode(_degree);
            var entry = new Entry<int, int>{ Key = 1, Pointer = 2 };
            rootNode.EntryList.Add(entry);

            var result = _walker.Next();

            Assert.AreSame(entry, result);
        }

        [Test]
        public void Returns_All_From_Root()
        {
            var rootNode = _mockDataProvider.GetRootNode(_degree);
            for (int i = 0; i <= 2; i++)
            {
                var entry = new Entry<int, int>(){Key = i, Pointer = i};
                rootNode.EntryList.Add(entry);
            }

            for (int i = 0; i <= 2; i++)
            {
                var entry = _walker.Next();
                Assert.AreSame(rootNode.EntryList[i], entry);
            }
        }

        [Test]
        public void Walks_In_Depth_First()
        {
            var entries = new List<Entry<int,int>>();
            var rootNode = _mockDataProvider.GetRootNode(_degree);

            var node1 = _mockDataProvider.CreateNode(_degree);
            rootNode.ChildrenAddresses.Add(node1.Address);

            var node2 = _mockDataProvider.CreateNode(_degree);
            node1.ChildrenAddresses.Add(node2.Address);

            for (int i = 0; i <= 2; i++)
            {
                var entry = new Entry<int, int>(){Key = i, Pointer = i};
                entries.Add(entry);
            }

            rootNode.EntryList.Add(entries[2]);
            node1.EntryList.Add(entries[1]);
            node2.EntryList.Add(entries[0]);

            for (int i = 0; i <= 2; i++)
            {
                var entry = _walker.Next();
                Assert.AreSame(entries[i], entry);
            }
        }

        [Test]
        public void Walks_Up_And_Down()
        {
            /*
             *           [2]
             *       [1]     [4]
             *    [0]     [3]   [5]
             */

            var nodes = new Node<int,int>[6];

            var entries = new List<Entry<int,int>>();
            nodes[2] = _mockDataProvider.GetRootNode(_degree);

            nodes[1] = _mockDataProvider.CreateNode(_degree);
            nodes[2].ChildrenAddresses.Add(nodes[1].Address);

            nodes[0] = _mockDataProvider.CreateNode(_degree);
            nodes[1].ChildrenAddresses.Add(nodes[0].Address);

            nodes[4] = _mockDataProvider.CreateNode(_degree);
            nodes[2].ChildrenAddresses.Add(nodes[4].Address);

            nodes[3] = _mockDataProvider.CreateNode(_degree);
            nodes[4].ChildrenAddresses.Add(nodes[3].Address);

            nodes[5] = _mockDataProvider.CreateNode(_degree);
            nodes[4].ChildrenAddresses.Add(nodes[5].Address);


            for (int i = 0; i <= 5; i++)
            {
                var entry = new Entry<int, int>(){Key = i, Pointer = i};
                entries.Add(entry);
                nodes[i].EntryList.Add(entry);
            }

            for (int i = 0; i <= 5; i++)
            {
                var entry = _walker.Next();
                Assert.AreSame(entries[i], entry);
            }
        }

        [Test]
        public void Walk_List_Of_Entries(){

            BuildTestTree();

            var walkedKeys = new List<int>();
            var entry = _walker.Next();
            while (entry != null)
            {
                walkedKeys.Add(entry.Key);
                entry = _walker.Next();
            }


            Assert.AreEqual(walkedKeys, new List<int>(){0,1,2,3,4,5,6,7,8,9});
        }

        [Test]
        public void MoveTo_Finds_1st_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(0);
        }

        [Test]
        public void MoveTo_Finds_2nd_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(1);
        }

        [Test]
        public void MoveTo_Finds_3rd_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(2);
        }

        [Test]
        public void MoveTo_Finds_4th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(3);
        }

        [Test]
        public void MoveTo_Finds_5th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(4);
        }


        [Test]
        public void MoveTo_Finds_6th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(5);
        }

        [Test]
        public void MoveTo_Finds_7th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(6);
        }

        [Test]
        public void MoveTo_Finds_8th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(7);
        }

        [Test]
        public void MoveTo_Finds_9th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(8);
        }

        [Test]
        public void MoveTo_Finds_10th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(9);
        }

        [Test]
        public void MoveTo_Returns_Null_When_Not_Found()
        {
            BuildTestTree();
            Assert.IsNull(_walker.MoveTo(-1));
        }

        void AssertItemsWalkedFrom(int startItem)
        {
            var walkedKeys = new List<int>();
            var entry = _walker.MoveTo(startItem);
            while (entry != null)
            {
                walkedKeys.Add(entry.Key);
                entry = _walker.Next();
            }

            var expected = Enumerable.Range(startItem, 10 - startItem);
            Assert.AreEqual(walkedKeys, expected);
        }

        /*
         *                         [9]
         *             [3    6]
         *    [0 1 2]   [4 5]  [7,8]
         */
        void BuildTestTree()
        {
            var rootNode = _mockDataProvider.GetRootNode(_degree);
            rootNode.EntryList.Add(new Entry<int, int> {
                Key = 9,
                Pointer = 9
            });
            var node36 = _mockDataProvider.CreateNode(_degree);
            rootNode.ChildrenAddresses.Add(node36.Address);
            node36.EntryList.Add(new Entry<int, int> {
                Key = 3,
                Pointer = 3
            });
            node36.EntryList.Add(new Entry<int, int> {
                Key = 6,
                Pointer = 6
            });
            var node012 = _mockDataProvider.CreateNode(_degree);
            node36.ChildrenAddresses.Add(node012.Address);
            node012.EntryList.Add(new Entry<int, int> {
                Key = 0,
                Pointer = 0
            });
            node012.EntryList.Add(new Entry<int, int> {
                Key = 1,
                Pointer = 1
            });
            node012.EntryList.Add(new Entry<int, int> {
                Key = 2,
                Pointer = 2
            });
            var node45 = _mockDataProvider.CreateNode(_degree);
            node36.ChildrenAddresses.Add(node45.Address);
            node45.EntryList.Add(new Entry<int, int> {
                Key = 4,
                Pointer = 4
            });
            node45.EntryList.Add(new Entry<int, int> {
                Key = 5,
                Pointer = 5
            });
            var node78 = _mockDataProvider.CreateNode(_degree);
            node36.ChildrenAddresses.Add(node78.Address);
            node78.EntryList.Add(new Entry<int, int> {
                Key = 7,
                Pointer = 7
            });
            node78.EntryList.Add(new Entry<int, int> {
                Key = 8,
                Pointer = 8
            });
        }
	}



}