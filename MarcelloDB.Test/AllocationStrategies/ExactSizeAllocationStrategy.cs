using System;
using NUnit.Framework;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Test.AllocationStrategies
{
    [TestFixture]
    public class ExactSizeAllocationStrategyTest
    {
        [Test]
        public void Uses_Exact_Size()
        {
            var allocationStrategy = new ExactSizeAllocationStrategy();
            Assert.AreEqual(100, allocationStrategy.CalculateSize(100));
        }
    }
}

