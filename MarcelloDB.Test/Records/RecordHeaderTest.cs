using System;
using MarcelloDB.Records;
using NUnit.Framework;

namespace MarcelloDB.Test.Records
{
    [TestFixture]
    public class RecordHeaderTest
    {
        Session Session { get; set; }
        [SetUp]
        public void Initialize()
        {
            Session = new Session(new TestPlatform(), "/");
        }

        [Test]
        public void Serializes_To_And_From_Bytes()
        {
            var header = RecordHeader.New();

            header.DataSize = 3;
            header.AllocatedDataSize = 4;
            Int64 address = 5;

            var buffer = header.AsBuffer(this.Session);
            var loadedHeader = RecordHeader.FromBuffer(address, buffer);

            Assert.AreEqual(header.DataSize, loadedHeader.DataSize, "DataSize");
            Assert.AreEqual(header.AllocatedDataSize, loadedHeader.AllocatedDataSize, "AllocatedSize");
            Assert.AreEqual(address, loadedHeader.Address, "Address");
        }
    }
}

