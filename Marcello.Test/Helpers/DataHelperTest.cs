using System;
using NUnit.Framework;
using Marcello.Helpers;

namespace Marcello.Test.Helpers
{
    [TestFixture]
    public class DataHelperTest
    {
        [Test]
        public void TestSameAddressAndLength()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4 };
            DataHelper.CopyData (10, source, 10, target);
            Assert.AreEqual(new byte[]{1,2}, target, "Data should be copied");
        }

        [Test]
        public void TestSameAddressShorterSource()
        {
            var source = new byte[]{ 1 };
            var target = new byte[]{ 3, 4 };
            DataHelper.CopyData (10, source, 10, target);
            Assert.AreEqual(new byte[]{1,4}, target, "Data should be copied");
        }

        [Test]
        public void TestSameAddressShorterTarget()
        {
            var source = new byte[]{ 1, 2, 3 };
            var target = new byte[]{ 3, 4 };
            DataHelper.CopyData (10, source, 10, target);
            Assert.AreEqual(new byte[]{1,2}, target, "Data should be copied");
        }

        [Test]
        public void TestSourceEmbedsTarget()
        {
            var source = new byte[]{ 1, 2, 3, 4 };
            var target = new byte[]{ 5, 6 };
            DataHelper.CopyData (10, source, 11, target);
            Assert.AreEqual(new byte[]{2,3}, target, "Data should be copied");
        }

        [Test]
        public void TestSourceOverlapsTargetBefore()
        {
            var source = new byte[]{ 1, 2, 3, 4 };
            var target = new byte[]{ 5, 6, 7 };
            DataHelper.CopyData (10, source, 12, target);
            Assert.AreEqual(new byte[]{3, 4, 7}, target, "Data should be copied");
        }

        [Test]
        public void TestSourceOverlapsTargetAfter()
        {
            var source = new byte[]{ 1, 2, 3, 4 };
            var target = new byte[]{ 5, 6, 7 };
            DataHelper.CopyData (11, source, 10, target);
            Assert.AreEqual(new byte[]{5, 1, 2}, target, "Data should be copied");
        }

        [Test]
        public void TestTargetEmbedsSource()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (11, source, 10, target);
            Assert.AreEqual(new byte[]{3, 1, 2, 6}, target, "Data should be copied");
        }

        [Test]
        public void TestTargetRightBeforeSource()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (14, source, 10, target);
            Assert.AreEqual(new byte[]{3, 4, 5, 6}, target, "Data should be copied");
        }

        [Test]
        public void TestTargetFarBeforeSource()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (14, source, 2, target);
            Assert.AreEqual(new byte[]{3, 4, 5, 6}, target, "Data should be copied");
        }


        [Test]
        public void TestTargetRightAfterSource()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (8, source, 10, target);
            Assert.AreEqual(new byte[]{3, 4, 5, 6}, target, "Data should be copied");
        }

        [Test]
        public void TestTargetFarAfterSource()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (8, source, 20, target);
            Assert.AreEqual(new byte[]{3, 4, 5, 6}, target, "Data should be copied");
        }
    }
}

