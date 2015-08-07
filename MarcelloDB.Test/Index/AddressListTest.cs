using System;
using NUnit.Framework;
using MarcelloDB.Index;

namespace MarcelloDB.Test.Index
{
    [TestFixture]
    public class AddressListTest
    {
        [Test]
        public void Test_New_Address_List_Is_Dirty()
        {
            Assert.IsTrue(new AddressList().Dirty);
        }
    }
}

