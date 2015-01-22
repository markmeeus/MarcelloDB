using System;
using Marcello.Index;
using NUnit.Framework;

namespace Marcello.Test.Index
{
    [TestFixture]
    public class ObjectComparerTest
    {
        ObjectComparer _comparer;

        [SetUp]
        public void Initialize()
        {
            _comparer = new ObjectComparer();  
        }

        [Test]
        public void Compares_Shorts()
        {
            Assert.AreEqual(-1, _comparer.Compare((short)1, (short)2));
            Assert.AreEqual(0, _comparer.Compare((short)2, (short)2));
            Assert.AreEqual(1, _comparer.Compare((short)2, (short)1));
        }

        [Test]
        public void Compares_Integers()
        {
            Assert.AreEqual(-1, _comparer.Compare((int)1, (int)2));
            Assert.AreEqual(0, _comparer.Compare((int)2, (int)2));
            Assert.AreEqual(1, _comparer.Compare((int)2, (int)1));
        }

        [Test]
        public void Compares_Longs()
        {
            Assert.AreEqual(-1, _comparer.Compare((long)1, (long)2));
            Assert.AreEqual(0, _comparer.Compare((long)2, (long)2));
            Assert.AreEqual(1, _comparer.Compare((long)2, (long)1));
        }

        [Test]
        public void Compares_Strings(){
            Assert.AreEqual(-1, _comparer.Compare("1", "2"));
            Assert.AreEqual(0, _comparer.Compare("2", "2"));
            Assert.AreEqual(1, _comparer.Compare("2", "1"));
        }
    }
}

