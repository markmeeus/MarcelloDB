using NUnit.Framework;
using System.Collections.Generic;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;
using System.Linq;
using System;

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
        public void Walks_List_Of_Entries(){

            BuildTestTree();

            var walkedKeys = new List<int>();
            var entry = _walker.Next();
            while (entry != null)
            {
                walkedKeys.Add(entry.Key);
                entry = _walker.Next();
            }

            Assert.AreEqual(Enumerable.Range(0, 18), walkedKeys);
        }

        [Test]
        public void Walks_List_Of_Entries_Backwards()
        {
            BuildTestTree();

            _walker.Reverse();

            var walkedKeys = new List<int>();
            var entry = _walker.Next();
            while (entry != null)
            {
                walkedKeys.Add(entry.Key);
                entry = _walker.Next();
            }

            Assert.AreEqual(Enumerable.Range(0, 18).Reverse(), walkedKeys);
        }

        [Test]
        public void Walks_From_1st_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(0, 9);
        }

        [Test]
        public void Walks_From_2nd_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(1, 9);
        }

        [Test]
        public void Walks_From_3rd_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(2, 8);
        }

        [Test]
        public void Walks_From_4th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(3, 7);
        }

        [Test]
        public void Walks_From_5th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(4, 6);
        }


        [Test]
        public void Walks_From_6th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(5, 5);
        }

        [Test]
        public void Walks_From_7th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(6, 9);
        }

        [Test]
        public void Walks_From_8th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(7, 8);
        }

        [Test]
        public void Walks_From_9th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(8, 8);
        }

        [Test]
        public void Walks_From_10th_Item()
        {
            BuildTestTree();
            AssertItemsWalkedFrom(9, 9);
        }

        [Test]
        public void Walks_In_Range_Not_In_Tree()
        {
            BuildTestTree();
            _walker.SetRange(new BTreeWalkerRange<int>(-1, 20));

            var walkedKeys = new List<int>();
            var entry = _walker.Next();
            while (entry != null)
            {
                walkedKeys.Add(entry.Key);
                entry = _walker.Next();
            }

            var expected = Enumerable.Range(0, 18);
            Assert.AreEqual(expected, walkedKeys);
        }

        [Test]
        public void Walks_Items_Above()
        {
            BuildTestTree();
            var walkedKeys = new List<int>();

            var range = new BTreeWalkerRange<int>();
            range.SetStartAt(3);
            range.IncludeStartAt = false;
            _walker.SetRange(range);

            var entry = _walker.Next();
            while (entry != null)
            {
                walkedKeys.Add(entry.Key);
                entry = _walker.Next();
            }

            var expected = Enumerable.Range(4, 14);
            Assert.AreEqual(expected, walkedKeys);
        }


        [Test]
        public void Walks_Duplicate_Keys_Within_Range()
        {
            var rootNode = _mockDataProvider.GetRootNode(_degree);

            var entry1 = new Entry<int, int>{ Key = 1, Pointer = 2 };
            var entry2 = new Entry<int, int>{ Key = 2, Pointer = 4 };
            var entry3 = new Entry<int, int>{ Key = 2, Pointer = 5 };

            rootNode.EntryList.Add(entry1);
            rootNode.EntryList.Add(entry2);
            rootNode.EntryList.Add(entry3);

            var walkedPointers = new List<int>();

            _walker.SetRange(new BTreeWalkerRange<int>(2, 2));

            var result = _walker.Next();
            while (result != null)
            {
                walkedPointers.Add(result.Pointer);
                result = _walker.Next();
            }

            Assert.AreEqual(walkedPointers, new List<int>{4,5});
        }

        [Test]
        public void Ignores_All_Non_Included()
        {
            var rootNode = _mockDataProvider.GetRootNode(_degree);

            rootNode.EntryList.Add(new Entry<int, int>{ Key = 1, Pointer = 1 });
            rootNode.EntryList.Add(new Entry<int, int>{ Key = 1, Pointer = 2 });
            rootNode.EntryList.Add(new Entry<int, int>{ Key = 2, Pointer = 3 });
            rootNode.EntryList.Add(new Entry<int, int>{ Key = 3, Pointer = 4 });
            rootNode.EntryList.Add(new Entry<int, int>{ Key = 3, Pointer = 5 });
            rootNode.EntryList.Add(new Entry<int, int>{ Key = 4, Pointer = 6 });


            var walkedPointers = new List<int>();
            var range = new  BTreeWalkerRange<int>(1, 4);
            range.IncludeStartAt = false;
            range.IncludeEndAt = false;
            _walker.SetRange(range);

            var result = _walker.Next();
            while (result != null)
            {
                walkedPointers.Add(result.Pointer);
                result = _walker.Next();
            }

            Assert.AreEqual(walkedPointers, new List<int>{3, 4, 5});
        }

        [Test]
        public void Walks_Empty_List()
        {
            Assert.IsNull(_walker.Next());
        }

        [Test]
        public void Walks_Empty_Range_In_Empty_Tree()
        {
            _walker.SetRange(new BTreeWalkerRange<int>( 1, 2));
            Assert.IsNull(_walker.Next());
        }

        [Test]
        public void Walks_Empty_Range_In_FilledTree()
        {
            BuildTestTree();

            _walker.SetRange(new BTreeWalkerRange<int>(18, 19));
            Assert.IsNull(_walker.Next());
        }

        [Test]
        public void Throws_On_Invalid_Range()
        {
            Assert.Throws<InvalidOperationException>(
                () =>_walker.SetRange(new BTreeWalkerRange<int>(1, 0)));
        }

        void AssertItemsWalkedFrom(int startItem, int endItem)
        {
            var walkedKeys = new List<int>();

            _walker.SetRange(new BTreeWalkerRange<int>(startItem, endItem));
            var entry = _walker.Next();
            while (entry != null)
            {
                walkedKeys.Add(entry.Key);
                entry = _walker.Next();
            }

            var expected = Enumerable.Range(startItem, 1+ (endItem - startItem));
            Assert.AreEqual(expected, walkedKeys);

            //And backwards
            walkedKeys = new List<int>();
            _walker.Reverse();
            _walker.SetRange(new BTreeWalkerRange<int>(endItem, startItem));
            entry = _walker.Next();
            while (entry != null)
            {
                walkedKeys.Add(entry.Key);
                entry = _walker.Next();
            }

            expected = Enumerable.Range(startItem, 1 + (endItem - startItem));
            Assert.AreEqual(expected.Reverse(), walkedKeys);
        }


        /*
         *                         [9]
         *             [3    6]             [12         15]
         *    [0 1 2]   [4 5]  [7,8]  [10, 11]  [13, 14] [16, 17]
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

            var node1215 = _mockDataProvider.CreateNode(_degree);
            rootNode.ChildrenAddresses.Add(node1215.Address);
            node1215.EntryList.Add(new Entry<int, int> {
                Key = 12,
                Pointer = 12
            });
            node1215.EntryList.Add(new Entry<int, int> {
                Key = 15,
                Pointer = 15
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

            var node1011 = _mockDataProvider.CreateNode(_degree);
            node1215.ChildrenAddresses.Add(node1011.Address);
            node1011.EntryList.Add(new Entry<int, int> {
                Key = 10,
                Pointer = 10
            });
            node1011.EntryList.Add(new Entry<int, int> {
                Key = 11,
                Pointer = 11
            });

            var node1314 = _mockDataProvider.CreateNode(_degree);
            node1215.ChildrenAddresses.Add(node1314.Address);
            node1314.EntryList.Add(new Entry<int, int> {
                Key = 13,
                Pointer = 13
            });
            node1314.EntryList.Add(new Entry<int, int> {
                Key = 14,
                Pointer = 14
            });

            var node1617 = _mockDataProvider.CreateNode(_degree);
            node1215.ChildrenAddresses.Add(node1617.Address);
            node1617.EntryList.Add(new Entry<int, int> {
                Key = 16,
                Pointer = 16
            });
            node1617.EntryList.Add(new Entry<int, int> {
                Key = 17,
                Pointer = 17
            });
        }
	}
}