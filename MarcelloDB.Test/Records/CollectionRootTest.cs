using System;
using NUnit.Framework;
using MarcelloDB.Records;

namespace MarcelloDB.Test.Records
{
    [TestFixture]
    public class CollectionRootTest
    {
        CollectionRoot _collectionRoot;

        [SetUp]
        public void Initialize()
        {
            _collectionRoot = new CollectionRoot();
        }


        [Test] 
        public void Is_Clean_After_Construction()
        {
            Assert.IsFalse(_collectionRoot.Dirty);
        }

        [Test]
        public void Is_Dirty_When_FormatVersion_Changed()
        {
            _collectionRoot.FormatVersion = 2;
            Assert.IsTrue(_collectionRoot.Dirty);
        }            

        [Test]
        public void Is_Not_Dirty_When_Set_Clean_Called()
        {
            _collectionRoot.FormatVersion = 2;
            _collectionRoot.Clean();
            Assert.False(_collectionRoot.Dirty);
        }


    }
}

