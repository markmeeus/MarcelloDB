using System;
using NUnit.Framework;
using MarcelloDB.Records;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Index;

namespace MarcelloDB.Test.AllocationStrategies
{
    [TestFixture]
    public class AllocationStrategyTest
    {
        [Test]
        public void StrategyFor_EmptyRecordIndexNode_ReturnsFixedSizeStrategy()
        {
            var strategy = AllocationStrategy.StrategyFor(
                               new Node<EmptyRecordIndexKey, Int64>(2));
            Assert.AreEqual(typeof(ExactSizeAllocationStrategy), strategy.GetType());
        }

        [Test]
        public void StrategyFor_EmptyRecordIndexNode_As_Object_ReturnsFixedSizeStrategy()
        {
            var strategy = AllocationStrategy.StrategyFor(
                (object)new Node<EmptyRecordIndexKey, Int64>(2));
            Assert.AreEqual(typeof(ExactSizeAllocationStrategy), strategy.GetType());
        }

        [Test]
        public void Strategy_For_Object_Returns_DoubleSizeStrategy()
        {
            var strategy = AllocationStrategy.StrategyFor(new object());
            Assert.AreEqual(typeof(DoubleSizeAllocationStrategy), strategy.GetType());
        }
    }
}

