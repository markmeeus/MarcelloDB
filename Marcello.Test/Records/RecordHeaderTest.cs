using System;
using Marcello.Records;
using NUnit.Framework;

namespace Marcello.Test.Records
{
    [TestFixture]
    public class RecordHeaderTest
    {
        [Test]
        public void Serializes_To_And_From_Bytes()
        {
            var header = RecordHeader.New();

            header.DataSize = 3;
            header.AllocatedDataSize = 4;
            Int64 address = 5;

            var bytes = header.AsBytes();
            var loadedHeader = RecordHeader.FromBytes(address, bytes);

            Assert.AreEqual(header.DataSize, loadedHeader.DataSize, "DataSize");
            Assert.AreEqual(header.AllocatedDataSize, loadedHeader.AllocatedDataSize, "AllocatedSize");
            Assert.AreEqual(address, loadedHeader.Address, "Address");
        }
    }
}

