using System;
using NUnit.Framework;
using MarcelloDB.Index.LazyNode;
using System.Linq;

namespace MarcelloDB.Test.Index.LazyNode
{
    [TestFixture]
    public class LazyNodeTest
    {
        const int DEGREE = 6;
        LazyNode<int> _node;

        [SetUp]
        public void Initialize()
        {
            _node = new LazyNode<int>(DEGREE);
        }

        [Test]
        public void ToBytes_FromBytes_Does_Not_Throw()
        {
            Assert.DoesNotThrow(() => LazyNode<int>.FromBytes(DEGREE, _node.ToBytes()));
        }

        [Test]
        public void ToBytes_FromBytes_Preserves_ChildrenAddresses()
        {
            _node.ChildrenAddresses.Add(1);
            _node.ChildrenAddresses.Add(2);
            var reloadedNode = LazyNode<int>.FromBytes(DEGREE, _node.ToBytes());

            Assert.AreEqual(1, reloadedNode.ChildrenAddresses[0]);
            Assert.AreEqual(2, reloadedNode.ChildrenAddresses[1]);
        }

        [Test]
        public void TestSerializationSpeed()
        {
            var lazyList = new LazyValueList<string>();
            var list = new System.Collections.Generic.List<string>();
            for (int i = 0; i < 1000; i++)
            {
                lazyList.Add("Item " + i.ToString());
                list.Add("Item " + i.ToString());
            }

            var s = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                var bytes = lazyList.ToBytes();
            }
            s.Stop();
            var lazyToBytes = s.ElapsedMilliseconds;

            var serializer = new MarcelloDB.Serialization.SerializerResolver()
                .SerializerFor<System.Collections.Generic.List<string>>();
            
            s.Start();
            for (int i = 0; i < 10000; i++)
            {
                var bytes = serializer.Serialize(list);
            }
            s.Stop();
            var bsonToBytes = s.ElapsedMilliseconds;
            Assert.AreEqual(bsonToBytes, lazyToBytes);
        }
        [Test]
        public void TestDeSerializationSpeed()
        {
            var lazyList = new LazyValueList<string>();
            var list = new System.Collections.Generic.List<string>();
            for (int i = 0; i < 1000; i++)
            {
                lazyList.Add("Item " + i.ToString());
                list.Add("Item " + i.ToString());
            }
            var lazyBytes = lazyList.ToBytes();
            var serializer = new MarcelloDB.Serialization.SerializerResolver()
                .SerializerFor<System.Collections.Generic.List<string>>();
            var bsonBytes = serializer.Serialize(list);

            var s = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                //iterate to make sure it's loaded
                var l = new LazyValueList<string>(lazyBytes, 0).Select(v => v).ToList();
            }
            s.Stop();
            var lazyFromBytes = s.ElapsedMilliseconds;


            s.Start();
            for (int i = 0; i < 10000; i++)
            {
                serializer.Deserialize(bsonBytes);
            }
            s.Stop();
            var bsonFromBytes = s.ElapsedMilliseconds;
            Assert.AreEqual(bsonFromBytes, lazyFromBytes);
        }
    }
}

