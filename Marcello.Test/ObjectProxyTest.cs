using System;
using NUnit.Framework;
using Marcello.Serialization;

namespace Marcello.Test
{   
    [TestFixture ()]
    public class ObjectProxyTest
    {

        [Test()]
        public void TestReturnsNullIfNoID()
        {
            Assert.AreEqual (null, new ObjectProxy (new {name = "123"}).ID, "Should return null when no ID");
        }
            
        [Test()]
        public void TestShouldReturnID()
        {
            Assert.AreEqual (1, new ObjectProxy (new ClassWithID(){ID = 1}).ID, "Should return null when no ID");
        }

        [Test()]
        public void TestShouldReturnId()
        {
            Assert.AreEqual (1, new ObjectProxy (new ClassWithId(){Id = 1}).ID, "Should return null when no ID");
        }

        [Test()]
        public void TestShouldReturnid()
        {
            Assert.AreEqual (1, new ObjectProxy (new ClassWithid(){id = 1}).ID, "Should return null when no ID");
        }


        [Test()]
        public void TestShouldReturnClassID()
        {
            Assert.AreEqual (1, new ObjectProxy (new ClassWithClassID(){ClassWithClassIDID = 1}).ID, "Should return null when no ID");
        }

        [Test()]
        public void TestShouldReturnClassId()
        {
            Assert.AreEqual (1, new ObjectProxy (new ClassWithClassId(){ClassWithClassIdId = 1}).ID, "Should return null when no ID");
        }

        [Test()]
        public void TestShouldReturnClassid()
        {
            Assert.AreEqual (1, new ObjectProxy (new ClassWithClassid(){ClassWithClassidid = 1}).ID, "Should return null when no ID");
        }

        [Test()]
        public void TestShouldReturnAttributedId()
        {
            Assert.AreEqual (1, new ObjectProxy (new ClassWithAttrID(){AttributedID = 1}).ID, "Should return null when no ID");
        }

    }
}

