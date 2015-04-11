using System;
using NUnit.Framework;
using MarcelloDB.Buffers;

namespace MarcelloDB.Test.Buffers
{
    internal class TestByteBufferManager : ByteBufferManager
    {
        internal int BytesCreated { get; set; }

        protected override byte[] CreateBytes(int minimumLength)
        {
            BytesCreated += minimumLength;
            return base.CreateBytes(minimumLength);
        }
    }

    [TestFixture]
    public class ByteBufferManagerTest
    {
        TestByteBufferManager _manager;

        [SetUp]
        public void Initialize()
        {
            _manager = new TestByteBufferManager();
        }

        [Test]
        public void Test_Creates_Byte_Buffer()
        {
            var buffer = _manager.Create(10);
            Assert.AreEqual(typeof(ByteBuffer), buffer.GetType());
        }

        [Test]
        public void Assigns_A_Byte_Array_Of_Correct_Minimum_Length()
        {
            var buffer = _manager.Create(10);
            Assert.IsTrue(buffer.Bytes.Length >= 10);
        }

        [Test]
        public void Assigns_The_Requested_Lenght()
        {
            var buffer = _manager.Create(10);
            Assert.AreEqual(10, buffer.Length);
        }

        [Test]
        public void Create_Reuses_Returned_Bytes()
        {
            var buffer = _manager.Create(10);
            _manager.Recycle(buffer);
            _manager.Create(10);
            Assert.AreEqual(10, _manager.BytesCreated);
        }

        [Test]
        public void Does_Not_Recycle_Twice()
        {
            var buffer = _manager.Create(10);
            _manager.Recycle(buffer);
            _manager.Create(10);
            _manager.Create(10);
            Assert.AreEqual(20, _manager.BytesCreated);
        }

        [Test]
        public void Recycles_Best_Fit()
        {
            var buffers = new ByteBuffer[3]
            {
                _manager.Create(200),
                _manager.Create(10),
                _manager.Create(100)
            };

            foreach (var buffer in buffers)
            {
                _manager.Recycle(buffer);
            }

            var newBuffer = _manager.Create(9);
            Assert.AreEqual(10, newBuffer.Bytes.Length);
        }
    }
}

