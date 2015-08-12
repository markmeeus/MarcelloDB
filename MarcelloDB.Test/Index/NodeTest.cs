using System;
using NUnit.Framework;
using MarcelloDB.Index;


namespace MarcelloDB.Test.Index
{
    [TestFixture]
	public class NodeTest
	{
        Node<int,int> _node;

        [SetUp]
        public void Initialize()
        {
            _node = new Node<int, int>(2);
        }

        [Test]
        public void New_Node_Is_Not_Dirty()
        {
            Assert.IsFalse(_node.Dirty);
        }

        [Test]
        public void Node_With_Added_Address_Is_Dirty()
        {
            _node.ChildrenAddresses.Add(123);
            Assert.IsTrue(_node.Dirty);
        }

        [Test]
        public void Clear_Changes_Clears_Address_Changes()
        {
            _node.ChildrenAddresses.Add(123);
            _node.ClearChanges();
            Assert.IsFalse(_node.Dirty);
        }

        [Test]
        public void Node_With_Added_Entries_Is_Dirty()
        {
            _node.EntryList.Add(new Entry<int, int>{Key=123, Pointer=456});
            Assert.IsTrue(_node.Dirty);Assert.IsTrue(_node.Dirty);
        }

        [Test]
        public void Clear_Changes_Clears_EntryList_Changes()
        {
            _node.EntryList.Add(new Entry<int, int>{Key=123, Pointer=456});
            _node.ClearChanges();
            Assert.IsFalse(_node.Dirty);
        }
	}
}

