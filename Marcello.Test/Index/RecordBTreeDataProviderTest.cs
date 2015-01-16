using System;
using Marcello.Records;
using NUnit.Framework;
using System.Collections.Generic;
using Marcello.Serialization;
using System.Linq;
using Marcello.Index;

namespace Marcello.Test.Index
{
    internal class TestRecordManager : IRecordManager
    {
        internal Dictionary<Int64, Record> Records { get; set; }
        internal Dictionary<string, Int64> NamedRecords { get; set; }

        internal TestRecordManager()
        {
            Records = new Dictionary<Int64, Record>();
            NamedRecords = new Dictionary<string, Int64>();
        }
        #region IRecordManager implementation

        public Record GetRecord(Int64 address){
            return Records[address];
        }

        public Record AppendRecord(byte[] data, bool hasObject = false)
        {
            var record = new Record();
            record.Data = data;
            record.Header.Address = Records.Count + 1;
            record.Header.HasObject = hasObject;
            Records.Add(record.Header.Address, record);

            return record;
        }

        public Record UpdateRecord(Record record, byte[] data)
        {
            Records[record.Header.Address].Data = data;
            return record;
        }

        public void ReleaseRecord(Record record)
        {
            Records.Remove(record.Header.Address);
        }

        public void RegisterNamedRecordAddress(string name, Int64 recordAddress)
        {
            this.NamedRecords[name] = recordAddress;
        }

        public Int64 GetNamedRecordAddress(string name)
        {
            if (NamedRecords.ContainsKey(name))
            {
                return NamedRecords[name];
            }
            return 0;

        }
        #endregion
    }

    [TestFixture]
    public class RecordBTreeDataProviderTest
    {
        RecordBTreeDataProvider provider;
        TestRecordManager manager;

        [SetUp]
        public void Initialize()
        {
            manager = new TestRecordManager();
            provider = new RecordBTreeDataProvider(
                manager,
                new BsonSerializer<Node<ComparableObject,Int64>>(),
                "Root");
        }

        [Test]
        public void CreateNodeAppendsRecord()
        {
            provider.CreateNode(2);
            Assert.AreEqual(1, manager.Records.Count);
        }

        [Test]
        public void CreateNodeAssignsNodeAddress()
        {
            var node = provider.CreateNode(2);
            Assert.AreEqual(1, node.Address);
        }

        [Test]
        public void GetNodeLoadsFromRecord()
        {
            var node = provider.CreateNode(2);
            var loadedNode = new RecordBTreeDataProvider(
                                 manager,
                new BsonSerializer<Node<ComparableObject,Int64>>(),
                                "Root").GetNode(node.Address);
            Assert.AreEqual(node.Address, loadedNode.Address);
        }

        [Test]
        public void GetNodeCachesNode()
        {
            var node = provider.CreateNode(2);
            var newProvider = new RecordBTreeDataProvider(
                manager,
                new BsonSerializer<Node<ComparableObject,Int64>>(),
                "Root");
            node = newProvider.GetNode(node.Address); // get with new provider
            node.ChildrenAddresses.Add(12);
            node = newProvider.GetNode(node.Address); //get it again
            Assert.AreEqual(12, node.ChildrenAddresses[0]);
        }

        [Test]
        public void CreateNodeCachesLoadedNodes()
        {
            var node = provider.CreateNode(2);
            node.ChildrenAddresses.Add(12);
            node = provider.GetNode(node.Address);
            Assert.AreEqual(12, node.ChildrenAddresses[0]);
        }

        [Test]
        public void GetRootNodeCreatesANode()
        {
            var node = provider.GetRootNode(2);
            Assert.AreEqual(1, manager.Records.Values.Count);
        }

        [Test]
        public void GetRootNodeReturnsSameNode(){
            var node = provider.GetRootNode(2);
            var newProvider = new RecordBTreeDataProvider(
                manager,
                new BsonSerializer<Node<ComparableObject,Int64>>(),
                "Root");
            var secondNode = newProvider.GetRootNode(2);

            Assert.AreEqual(1, manager.Records.Values.Count());
        }

        [Test]
        public void CachesRootNode(){
            var node = provider.GetRootNode(2);
            node.ChildrenAddresses.Add(1);
            var reloadedNode = provider.GetRootNode(2);
            Assert.AreSame(node, reloadedNode);
        }            
    }
}

