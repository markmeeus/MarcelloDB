using System;
using NUnit.Framework;
using Marcello.Helpers;

namespace Marcello.Test.Helpers
{
    [TestFixture]
    public class DataHelperTest
    {
        [Test]
        public void Same_Address_And_Length()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4 };
            DataHelper.CopyData (10, source, 10, target);
            Assert.AreEqual(new byte[]{1,2}, target, "Data should be copied");
        }

        [Test]
        public void Same_Address_Shorter_Source()
        {
            var source = new byte[]{ 1 };
            var target = new byte[]{ 3, 4 };
            DataHelper.CopyData (10, source, 10, target);
            Assert.AreEqual(new byte[]{1,4}, target, "Data should be copied");
        }

        [Test]
        public void Same_Address_Shorter_Target()
        {
            var source = new byte[]{ 1, 2, 3 };
            var target = new byte[]{ 3, 4 };
            DataHelper.CopyData (10, source, 10, target);
            Assert.AreEqual(new byte[]{1,2}, target, "Data should be copied");
        }

        [Test]
        public void Source_Embeds_Target()
        {
            var source = new byte[]{ 1, 2, 3, 4 };
            var target = new byte[]{ 5, 6 };
            DataHelper.CopyData (10, source, 11, target);
            Assert.AreEqual(new byte[]{2,3}, target, "Data should be copied");
        }

        [Test]
        public void Source_Overlaps_Target_Before()
        {
            var source = new byte[]{ 1, 2, 3, 4 };
            var target = new byte[]{ 5, 6, 7 };
            DataHelper.CopyData (10, source, 12, target);
            Assert.AreEqual(new byte[]{3, 4, 7}, target, "Data should be copied");
        }

        [Test]
        public void Source_Overlaps_Target_After()
        {
            var source = new byte[]{ 1, 2, 3, 4 };
            var target = new byte[]{ 5, 6, 7 };
            DataHelper.CopyData (11, source, 10, target);
            Assert.AreEqual(new byte[]{5, 1, 2}, target, "Data should be copied");
        }

        [Test]
        public void Target_Embeds_Source()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (11, source, 10, target);
            Assert.AreEqual(new byte[]{3, 1, 2, 6}, target, "Data should be copied");
        }

        [Test]
        public void Target_Right_Before_Source()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (14, source, 10, target);
            Assert.AreEqual(new byte[]{3, 4, 5, 6}, target, "Data should be copied");
        }

        [Test]
        public void Target_Far_Before_Source()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (14, source, 2, target);
            Assert.AreEqual(new byte[]{3, 4, 5, 6}, target, "Data should be copied");
        }


        [Test]
        public void Target_Right_After_Source()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (8, source, 10, target);
            Assert.AreEqual(new byte[]{3, 4, 5, 6}, target, "Data should be copied");
        }

        [Test]
        public void Target_Far_After_Source()
        {
            var source = new byte[]{ 1, 2 };
            var target = new byte[]{ 3, 4, 5, 6 };
            DataHelper.CopyData (8, source, 20, target);
            Assert.AreEqual(new byte[]{3, 4, 5, 6}, target, "Data should be copied");
        }
    }
}

