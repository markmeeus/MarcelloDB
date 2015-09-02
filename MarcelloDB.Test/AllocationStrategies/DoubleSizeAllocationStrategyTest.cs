using System;
using NUnit.Framework;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Test.AllocationStrategies
{
    [TestFixture]
    public class DoubleSizeAllocationStrategyTest
    {
        [Test]
        public void Uses_Double_Size()
        {
            var allocationStrategy = new DoubleSizeAllocationStrategy();
            Assert.AreEqual(200, allocationStrategy.CalculateSize(100));
        }
    }
}

