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
            bool reuseRecycledRecord = true, 
            IAllocationStrategy allocationStrategy = null)
        {
            this.UsedAllocationStrategy = allocationStrategy;

            var record = new Record();
            record.Header.AllocatedDataSize = data.Length;
            record.Data = data;
            record.Header.Address = Records.Count + 1;
            Records.Add(record.Header.Address, record);

            return record;
        }

        public Record UpdateRecord(
            Record record, 
            byte[] data, 
            bool reuseRecycledRecord = true, 
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

    internal class DummyAllocationStrategy : IAllocationStrategy 
    {
        #region IAllocationStrategy implementation
        public int CalculateSize(int dataSize)
        {
            return dataSize;
        }
        #endregion            
    }

    [TestFixture]
    public class RecordBTreeDataProviderTest
    {
        RecordBTreeDataProvider<object> _provider;
        TestRecordManager _manager;
        DummyAllocationStrategy _allocationStrategy;

        [SetUp]
        public void Initialize()
        {
            _manager = new TestRecordManager();
            _allocationStrategy = new DummyAllocationStrategy();
            _provider = new RecordBTreeDataProvider<object>(
                _manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root", 
                true,
                _allocationStrategy);
        }

        [Test]
        public void Create_Node_Appends_Record()
        {
            _provider.CreateNode(2);
            Assert.AreEqual(1, _manager.Records.Count);
        }

        [Test]
        public void Create_Node_Assigns_Node_Address()
        {
            var node = _provider.CreateNode(2);
            Assert.AreEqual(1, node.Address);
        }

        [Test]
        public void Create_Node_Uses_AllocationStrategy()
        {
            var node = _provider.CreateNode(2);
            Assert.AreSame(_allocationStrategy, _manager.UsedAllocationStrategy);
        }

        [Test]
        public void Get_Node_Loads_From_Record()
        {
            var node = _provider.CreateNode(2);
            var loadedNode = new RecordBTreeDataProvider<object>(
                                 _manager,
                new BsonSerializer<Node<object,Int64>>(),
                                "Root", true, null).GetNode(node.Address);
            Assert.AreEqual(node.Address, loadedNode.Address);
        }

        [Test]
        public void Get_Node_Caches_Node()
        {
            var node = _provider.CreateNode(2);
            var newProvider = new RecordBTreeDataProvider<object>(
                _manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root", 
                true,
                null);
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
            Assert.AreEqual(1, _manager.Records.Values.Count);
        }

        [Test]
        public void Get_RootNode_Returns_Same_Node()
        {
            _provider.GetRootNode(2);
            var newProvider = new RecordBTreeDataProvider<object>(
                _manager,
                new BsonSerializer<Node<object,Int64>>(),
                "Root",
                true,
                null);
            newProvider.GetRootNode(2);

            Assert.AreEqual(1, _manager.Records.Values.Count());
        }

        [Test]
        public void Caches_RootNode()
        {
            var node = _provider.GetRootNode(2);
            node.ChildrenAddresses.Add(1);
            var reloadedNode = _provider.GetRootNode(2);
            Assert.AreSame(node, reloadedNode);
        }                        
    }
}

