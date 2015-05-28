using System;
using MarcelloDB.Records;
using NUnit.Framework;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using System.Linq;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Test.Index
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
        public Record GetRecord(Int64 address)
        {
            return Records[address];
        }

        public Record AppendRecord(byte[] data, bool reuseRecycledRecord = true)
        {
            var record = new Record();
            record.Header.AllocatedDataSize = data.Length;
            record.Data = data;
            record.Header.Address = Records.Count + 1;
            Records.Add(record.Header.Address, record);

            return record;
        }

        public Record UpdateRecord(Record record, byte[] data, bool reuseRecycledRecord = true)
        {
            Records[record.Header.Address].Data = data;
            return record;
        }

        public void Recycle(Int64 address)
        {
        }   

        public void ReleaseRecord(Record record)
        {
            Records.Remove(record.Header.Address);
        }

        public void RegisterNamedRecordAddress(string name, Int64 recordAddress, bool reuseRecycledRecord = true)
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
                new BsonSerializer<Node<object,Int64>>(),
                "Root", true);
        }

        [Test]
        public void Create_Node_Appends_Record()
        {
            provider.CreateNode(2);
            Assert.AreEqual(1, manager.Records.Count);
        }

        [Test]
        public void Create_Node_Assigns_Node_Address()
        {
            var node = provider.CreateNode(2);
            Assert.AreEqual(1, node.Address);
        }

        [Test]
        public void Get_Node_Loads_From_Record()
        {
            var node = provider.CreateNode(2);
            var loadedNode = new RecordBTreeDataProvider(
                                 manager,
                new BsonSerializer<Node<object,Int64>>(),
                                "Root", true).GetNode(node.Address);
            Assert.AreEqual(node.Address, loadedNode.Address);
        }

        [Test]
        public void Get_Node_Caches_Node()
        {
            var node = provider.CreateNode(2);
            var newProvider = new RecordBTreeDataProvider(
                manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root", 
                true);
            node = newProvider.GetNode(node.Address); // get with new provider
            node.ChildrenAddresses.Add(12);
            node = newProvider.GetNode(node.Address); //get it again
            Assert.AreEqual(12, node.ChildrenAddresses[0]);
        }

        [Test]
        public void Create_Node_Caches_Loaded_Nodes()
        {
            var node = provider.CreateNode(2);
            node.ChildrenAddresses.Add(12);
            node = provider.GetNode(node.Address);
            Assert.AreEqual(12, node.ChildrenAddresses[0]);
        }

        [Test]
        public void Get_RootNode_Creates_a_Node()
        {
            provider.GetRootNode(2);
            Assert.AreEqual(1, manager.Records.Values.Count);
        }

        [Test]
        public void Get_RootNode_Returns_Same_Node()
        {
            provider.GetRootNode(2);
            var newProvider = new RecordBTreeDataProvider(
                manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root",
                true);
            newProvider.GetRootNode(2);

            Assert.AreEqual(1, manager.Records.Values.Count());
        }

        [Test]
        public void Caches_RootNode()
        {
            var node = provider.GetRootNode(2);
            node.ChildrenAddresses.Add(1);
            var reloadedNode = provider.GetRootNode(2);
            Assert.AreSame(node, reloadedNode);
        }                        
    }
}

