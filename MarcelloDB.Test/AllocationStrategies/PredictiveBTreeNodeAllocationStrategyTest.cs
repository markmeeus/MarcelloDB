using System;
using NUnit.Framework;
using MarcelloDB.Index;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Test.AllocationStrategies
{
    [TestFixture]
    public class PredictiveBTreeNodeAllocationStrategyTest
    {
        [Test]
        public void It_Allocates_2000_Percent_For_A_10_Percent_Filled_Non_ValueType_Leaf_Node()
        {
            var node = new Node<string>(5);
            var bytes = new byte[100];
            node.EntryList.Add(new Entry<string>());

            var strategy = new PredictiveBTreeNodeAllocationStrategy<string>(node);
            var calculated = strategy.CalculateSize(bytes.Length);

            var diff = Math.Abs(bytes.Length * 20 - calculated);
            Assert.IsTrue(diff <= bytes.Length * 2); // max 10% off
        }

        [Test]
        public void It_Allocates_2000_Percent_For_A_10_Percent_Filled_Non_ValueType_Non_Leaf_Node()
        {
            var node = new Node<string>(5);
            var bytes = new byte[100];
            node.EntryList.Add(new Entry<string>());
            node.ChildrenAddresses.Add(1);

            var strategy = new PredictiveBTreeNodeAllocationStrategy<string>(node);
            var calculated = strategy.CalculateSize(bytes.Length);

            var diff = Math.Abs(bytes.Length * 20 - calculated);
            Assert.IsTrue(diff <= bytes.Length * 2); // max 10% off
        }

        [Test]
        public void It_Allocates_1000_Percent_For_A_10_Percent_Filled_ValueType_Non_Leaf_Node()
        {
            var node = new Node<int>(5);
            var bytes = new byte[100];
            node.EntryList.Add(new Entry<int>());
            node.ChildrenAddresses.Add(1);

            var strategy = new PredictiveBTreeNodeAllocationStrategy<int>(node);
            var calculated = strategy.CalculateSize(bytes.Length);

            var diff = Math.Abs(bytes.Length * 10 - calculated);
            Assert.IsTrue(diff <= bytes.Length * 2); // max 10% off
        }

        [Test]
        public void It_Allocates_Exact_Size_For_Empty_Nodes()
        {
            var node = new Node<int>(5);
            var bytes = new byte[100];

            var strategy = new PredictiveBTreeNodeAllocationStrategy<int>(node);
            var calculated = strategy.CalculateSize(bytes.Length);
            Assert.AreEqual(bytes.Length, calculated);

        }
    }
}

