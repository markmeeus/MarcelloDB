using System;
using NUnit.Framework;
using MarcelloDB.Records;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Index;

namespace MarcelloDB.Test.AllocationStrategies
{
    [TestFixture]
    public class AllocationStrategyResolverTest
    {

        AllocationStrategyResolver resolver;

        [SetUp]
        public void Initialize()
        {
            resolver = new AllocationStrategyResolver();
        }

        [Test]
        public void StrategyFor_EmptyRecordIndexNode_ReturnsFixedSizeStrategy()
        {
            var strategy = resolver.StrategyFor(
                               new Node<EmptyRecordIndexKey, Int64>(2));
            Assert.AreEqual(typeof(ExactSizeAllocationStrategy), strategy.GetType());
        }

        [Test]
        public void StrategyFor_EmptyRecordIndexNode_As_Object_ReturnsFixedSizeStrategy()
        {
            var strategy = resolver.StrategyFor(
                (object)new Node<EmptyRecordIndexKey, Int64>(2));
            Assert.AreEqual(typeof(ExactSizeAllocationStrategy), strategy.GetType());
        }

        [Test]
        public void StrategyFor_BTreeNodes_Returns_PredictiveBTReeNodeAllocationStrategy()
        {
            var strategy = resolver.StrategyFor(
                new Node<int, Int64>(2));
            Assert.AreEqual(typeof(PredictiveBTreeNodeAllocationStrategy<int, Int64>), strategy.GetType());
        }

        [Test]
        public void Strategy_For_Object_Returns_DoubleSizeStrategy()
        {
            var strategy = resolver.StrategyFor(new object());
            Assert.AreEqual(typeof(DoubleSizeAllocationStrategy), strategy.GetType());
        }
    }
}

