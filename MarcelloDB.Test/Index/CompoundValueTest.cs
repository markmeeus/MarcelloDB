using System;
using NUnit.Framework;
using MarcelloDB.Index;

namespace MarcelloDB.Test.Index
{
    [TestFixture]
    public class CompoundValueTest
    {
        [Test]
        public void Compares_Ok_With_2_Params_On_First_Param()
        {
            var comp1 = new CompoundValue<int, int>(1,2);
            var comp2 = new CompoundValue<int, int>(1,3);
            Assert.AreEqual(-1, comp1.CompareTo(comp2));
            Assert.AreEqual(1, comp2.CompareTo(comp1));
        }

        [Test]
        public void Compares_Ok_With_2_Params_On_Second_Param()
        {
            var comp1 = new CompoundValue<int, int>(1,1);
            var comp2 = new CompoundValue<int, int>(2,1);
            Assert.AreEqual(-1, comp1.CompareTo(comp2));
            Assert.AreEqual(1, comp2.CompareTo(comp1));
        }

        [Test]
        public void Compares_Ok_When_Equal()
        {
            var comp1 = new CompoundValue<int, int>(1,1);
            var comp2 = new CompoundValue<int, int>(1,1);
            Assert.AreEqual(0, comp1.CompareTo(comp2));
            Assert.AreEqual(0, comp2.CompareTo(comp1));
        }

        [Test]
        public void Treats_First_Null_As_Smallest_Value()
        {
            var comp1 = new CompoundValue<string, string>(null, "1");
            var comp2 = new CompoundValue<string, string>("1", "1");
            Assert.AreEqual(-1, comp1.CompareTo(comp2));
            Assert.AreEqual(1, comp2.CompareTo(comp1));
        }

        [Test]
        public void Treats_Second_Null_As_Smallest_Value()
        {
            var comp1 = new CompoundValue<string, string>("1", null);
            var comp2 = new CompoundValue<string, string>("1", "1");
            Assert.AreEqual(-1, comp1.CompareTo(comp2));
            Assert.AreEqual(1, comp2.CompareTo(comp1));
        }

        [Test]
        public void Treats_Nulls_As_Equal()
        {
            var comp1 = new CompoundValue<string, string>("1", null);
            var comp2 = new CompoundValue<string, string>("1", null);
            Assert.AreEqual(0, comp1.CompareTo(comp2));
            Assert.AreEqual(0, comp2.CompareTo(comp1));
        }
    }
}

