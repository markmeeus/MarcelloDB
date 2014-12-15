using System;
using NUnit.Framework;
using Marcello.Records;
using Marcello.Test.Classes;

namespace Marcello.Test.Records
{
    [TestFixture]
    public class RecordBasedAddressArrayTest
    {
        Marcello _session;

        [SetUp]
        public void Initialize()
        {
            _session = new Marcello(new InMemoryStreamProvider());
        }

        [Test]
        public void Read_Write_Begin()
        {
            var ary = new RecordBasedAddressArray(0, 8, 8, _session.Collection<Article>().RecordManager);
            ary.Insert(1);
            Assert.AreEqual(1, ary.At(0));
        }

        [Test]
        public void Read_Write_Second(){
            var ary = new RecordBasedAddressArray(0, 8, 8, _session.Collection<Article>().RecordManager);
            ary.Insert(1);
            ary.Insert(2);
            Assert.AreEqual(2, ary.At(1));
        }

        [Test]
        public void Read_Write_Entire_Block(){
            var ary = new RecordBasedAddressArray(0, 8, 8, _session.Collection<Article>().RecordManager);
            for (int i = 0; i < 8; i++)
            {
                ary.Insert(i);
            }
            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(i, ary.At(i));
            }
        }

        [Test]
        public void Read_Write_Block_Size(){
            var ary = new RecordBasedAddressArray(0, 8, 8, _session.Collection<Article>().RecordManager);
            for (int i = 0; i < 8; i++)
            {
                ary.Insert(i);
            }
            ary.Insert(10);
            Assert.AreEqual(10, ary.At(8));
        }
    }
}

