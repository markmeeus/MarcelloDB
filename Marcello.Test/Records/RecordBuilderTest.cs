using System;
using NUnit.Framework;
using Marcello.Records;

namespace Marcello.Test.Records
{
    [TestFixture]
    public class RecordBuilderTest
    {
        RecordBuilder _builder;

        [SetUp]
        public void Initialize()
        {
            _builder = new RecordBuilder();
        }

        [Test]
        public void Builds_Record_With_Data()
        {
            var data = new byte[8]{1,2,3,4,5,6,7,8};
            var record = _builder.Build(data);
            Assert.AreEqual(data, record.Data);
        }

        [Test]
        public void Build_Record_With_DataSize(){
            var data = new byte[8]{1,2,3,4,5,6,7,8};
            var record = _builder.Build(data);
            Assert.AreEqual(data.Length, record.Header.DataSize);
        }

        [Test]
        public void Build_Record_With_Size_As_AllocationSize(){
            var data = new byte[8]{1,2,3,4,5,6,7,8};
            var record = _builder.Build(data);
            Assert.AreEqual(data.Length, record.Header.AllocatedDataSize);
        }

        [Test]
        public void Build_Record_With_Overridden_AllocationSize(){
            var data = new byte[8]{1,2,3,4,5,6,7,8};
            var record = _builder.Build(data, 100);
            Assert.AreEqual(100, record.Header.AllocatedDataSize);
        }
    }
}

