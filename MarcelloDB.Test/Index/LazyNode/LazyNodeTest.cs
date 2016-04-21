using System;
using NUnit.Framework;
using MarcelloDB.Index.LazyNode;

namespace MarcelloDB.Test.Index.LazyNode
{
    [TestFixture]
    public class LazyNodeTest
    {
        const int DEGREE = 6;
        LazyNode<int> _node;

        [SetUp]
        public void Initialize()
        {
            _node = new LazyNode<int>(DEGREE);
        }

        [Test]
        public void ToBytes_FromBytes_Does_Not_Throw()
        {
            Assert.DoesNotThrow(() => LazyNode<int>.FromBytes(DEGREE, _node.ToBytes()));
        }

        [Test]
        public void ToBytes_FromBytes_Preserves_ChildrenAddresses()
        {
            _node.ChildrenAddresses.Add(1);
            _node.ChildrenAddresses.Add(2);
            var reloadedNode = LazyNode<int>.FromBytes(DEGREE, _node.ToBytes());

            Assert.AreEqual(1, reloadedNode.ChildrenAddresses[0]);
            Assert.AreEqual(2, reloadedNode.ChildrenAddresses[1]);
        }

    }
}

