using System;
using NUnit.Framework;
using Marcello.Helpers;

namespace Marcello.Test.Helpers
{
    [TestFixture]
    public class AddressHelperTest
    {
        AddressHelper _addressHelper;

        [SetUp]
        public void Initialize(){
            _addressHelper = new AddressHelper();
        }

        [Test]
        public void Converts_Bytes_To_Address()
        {
            var bytes = new byte[]{0x11, 0x11, 0x22, 0x22,0x33, 0x33, 0x44, 0x55};
            Assert.AreEqual(0x5544333322221111, _addressHelper.BytesToAddress(bytes));
        }

        [Test]
        public void Converts_Address_To_Bytes()
        {
            var address = 0x5544333322221111;
            Assert.AreEqual(new byte[]{0x11, 0x11, 0x22, 0x22,0x33, 0x33, 0x44, 0x55}, 
                _addressHelper.AddressToBytes(address));
        }
    }
}

