using System;
using NUnit.Framework;
using MarcelloDB.Serialization;

namespace MarcelloDB.Test.Serialization
{   
    [TestFixture]
    public class ObjectProxyTest
    {
        [Test]
        public void Returns_Null_If_No_ID()
        {
            Assert.AreEqual(null, new ObjectProxy(new {name = "123"}).ID, "Should return null when no ID");
        }
            
        [Test]
        public void Returns_ID()
        {
            Assert.AreEqual(1, new ObjectProxy(new ClassWithID(){ID = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Returns_Id()
        {
            Assert.AreEqual(1, new ObjectProxy(new ClassWithId(){Id = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Returns_id()
        {
            Assert.AreEqual(1, new ObjectProxy(new ClassWithid(){id = 1}).ID, "Should return null when no ID");
        }


        [Test]
        public void Return_ClassID()
        {
            Assert.AreEqual(1, new ObjectProxy(new ClassWithClassID(){ClassWithClassIDID = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_ClassId()
        {
            Assert.AreEqual(1, new ObjectProxy(new ClassWithClassId(){ClassWithClassIdId = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_Classid()
        {
            Assert.AreEqual(1, new ObjectProxy(new ClassWithClassid(){ClassWithClassidid = 1}).ID, "Should return null when no ID");
        }

        [Test]
        public void Return_Attributed_Id()
        {
            Assert.AreEqual(1, new ObjectProxy(new ClassWithAttrID(){AttributedID = 1}).ID, "Should return null when no ID");
        }

    }
}