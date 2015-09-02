using System;
using MarcelloDB.Records;
using NUnit.Framework;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using System.Linq;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Test.Index
{
    internal class TestRecordManager : IRecordManager
    {
        internal IAllocationStrategy UsedAllocationStrategy { get; set; }

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

        public Record AppendRecord(
            byte[] data,
            IAllocationStrategy allocationStrategy = null)
        {
            this.UsedAllocationStrategy = allocationStrategy;

            var record = new Record();
            record.Header.AllocatedDataSize = data.Length * 10; //To be sure
            record.Data = data;
            record.Header.Address = Records.Count + 1;
            Records.Add(record.Header.Address, record);

            return record;
        }

        public Record UpdateRecord(
            Record record,
            byte[] data,
            IAllocationStrategy allocationStrategy = null)
        {
            Records[record.Header.Address].Data = data;
            this.UsedAllocationStrategy = allocationStrategy;
            return record;
        }

        public void Recycle(Int64 address)
        {
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
        RecordBTreeDataProvider<object> _provider;
        TestRecordManager _manager;

        [SetUp]
        public void Initialize()
        {
            _manager = new TestRecordManager();
            _provider = new RecordBTreeDataProvider<object>(
                _manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root");
        }

        [Test]
        public void Construction_Creates_MetaRecord()
        {
            Assert.IsTrue(_provider.MetaRecord.Record.Header.Address > 0);
        }


        [Test]
        public void Construction_Registers_Root_Record_Address()
        {
            Assert.AreEqual(_provider.MetaRecord.Record.Header.Address,
                _manager.GetNamedRecordAddress("Root"));
        }

        [Test]
        public void Construction_Uses_Existing_Meta_Record()
        {
            var secondProvider = new RecordBTreeDataProvider<object>(
                                     _manager,
                                     new BsonSerializer<Node<object,Int64>>(),
                                     "Root");
            Assert.AreEqual(_provider.MetaRecord.Record.Header.Address,
                secondProvider.MetaRecord.Record.Header.Address);
        }

        [Test]
        public void Create_Node_Appends_Record()
        {
            _provider.CreateNode(2);
            Assert.AreEqual(2, _manager.Records.Count); //metarecord + node record
        }

        [Test]
        public void Create_Node_Assigns_Node_Address()
        {
            var node = _provider.CreateNode(2);
            Assert.AreEqual(2, node.Address); //metarecord is 1, node is 2
        }

        [Test]
        public void Flush_Updates_MetaRecord_NumberOfNodes()
        {
            var rootNode = _provider.GetRootNode(2);
            var node1 = _provider.CreateNode(2);
            var node2 = _provider.CreateNode(2);
            rootNode.ChildrenAddresses.Add(node1.Address);
            rootNode.ChildrenAddresses.Add(node2.Address);
            _provider.Flush();
            var newProvider = new RecordBTreeDataProvider<object>(
                _manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root");
            Assert.AreEqual(3, newProvider.MetaRecord.NumberOfNodes);
        }

        [Test]
        public void Unlinked_Nodes_Are_Removed_From_Total()
        {
            var rootNode = _provider.GetRootNode(2);
            var node1 = _provider.CreateNode(2);
            var node2 = _provider.CreateNode(2);
            rootNode.ChildrenAddresses.Add(node1.Address);
            rootNode.ChildrenAddresses.Add(node2.Address);
            _provider.Flush();

            var newProvider = new RecordBTreeDataProvider<object>(
                _manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root");

            rootNode = newProvider.GetRootNode(2);
            newProvider.GetNode(node2.Address);
            rootNode.ChildrenAddresses.RemoveRange(1,1);
            newProvider.Flush();

            newProvider = new RecordBTreeDataProvider<object>(
                _manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root");

            Assert.AreEqual(2, newProvider.MetaRecord.NumberOfNodes);
        }

        [Test]
        public void Get_Node_Loads_From_Record()
        {
            var node = _provider.CreateNode(2);
            var loadedNode = new RecordBTreeDataProvider<object>(
                                 _manager,
                                new BsonSerializer<Node<object,Int64>>(),
                                "Root").GetNode(node.Address);
            Assert.AreEqual(node.Address, loadedNode.Address);
        }

        [Test]
        public void Get_Node_Caches_Node()
        {
            var node = _provider.CreateNode(2);
            var newProvider = new RecordBTreeDataProvider<object>(
                _manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root");
            node = newProvider.GetNode(node.Address); // get with new provider
            node.ChildrenAddresses.Add(12);
            node = newProvider.GetNode(node.Address); //get it again
            Assert.AreEqual(12, node.ChildrenAddresses[0]);
        }

        [Test]
        public void Create_Node_Caches_Loaded_Nodes()
        {
            var node = _provider.CreateNode(2);
            node.ChildrenAddresses.Add(12);
            node = _provider.GetNode(node.Address);
            Assert.AreEqual(12, node.ChildrenAddresses[0]);
        }

        [Test]
        public void Get_RootNode_Creates_a_Node()
        {
            _provider.GetRootNode(2);
            Assert.AreEqual(2, _manager.Records.Values.Count); //metarecord and rootnode
        }

        [Test]
        public void Get_RootNode_Returns_Same_Node()
        {
            _provider.GetRootNode(2);
            _provider.Flush();
            var newProvider = new RecordBTreeDataProvider<object>(
                _manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root");
            newProvider.GetRootNode(2);

            Assert.AreEqual(2, _manager.Records.Values.Count()); //metarecord and rootnode
        }
    }
}

