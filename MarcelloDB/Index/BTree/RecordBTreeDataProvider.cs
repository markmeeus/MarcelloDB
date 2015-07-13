using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using System.Collections.Generic;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Index.BTree
{
    internal class RecordBTreeDataProvider<TNodeKey> :  IBTreeDataProvider<TNodeKey, Int64>
    {
        internal IndexMetaRecord MetaRecord{ get; private set; }

        IRecordManager RecordManager { get; set; }

        IObjectSerializer<Node<TNodeKey, Int64>> Serializer { get; set; }

        Dictionary<Int64, Node<TNodeKey, Int64>> NodeCache { get; set; }

        Node<TNodeKey, Int64> RootNode { get; set; }

        string RootRecordName { get; set; }

        bool ReuseRecycledRecords { get; set; }

        IAllocationStrategy AllocationStrategy { get; set; }

        internal RecordBTreeDataProvider(
            IRecordManager recordManager,
            IObjectSerializer<Node<TNodeKey, Int64>> serializer,
            string rootRecordName,
            bool reuseRecycledRecords
            )
        {
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.NodeCache = new Dictionary<long, Node<TNodeKey, long>>();
            this.RootRecordName = rootRecordName;
            this.ReuseRecycledRecords = reuseRecycledRecords;
            this.Initialize();
        }

        void Initialize()
        {
            var serializer = new IndexMetaRecordSerializer();
            var rootRecordAddress = this.RecordManager.GetNamedRecordAddress(this.RootRecordName);
            if (rootRecordAddress <= 0)
            {
                var metaRecord = new IndexMetaRecord();
                var bytes = serializer.Serialize(metaRecord);
                metaRecord.Record = this.RecordManager.AppendRecord(bytes, this.ReuseRecycledRecords, null);
                this.RecordManager.RegisterNamedRecordAddress(this.RootRecordName,
                    metaRecord.Record.Header.Address, this.ReuseRecycledRecords);
                this.MetaRecord = metaRecord;
            }
            else
            {
                var record = this.RecordManager.GetRecord(rootRecordAddress);
                this.MetaRecord = serializer.Deserialize(record.Data);
                this.MetaRecord.Record = record;
            }

        }

        #region IBTreeDataProvider implementation
        public Node<TNodeKey, long> GetRootNode(int degree)
        {
            var rootRecordAddress = this.MetaRecord.RootNodeAddress;
            if (rootRecordAddress > 0)
            {
                this.RootNode = GetNode(rootRecordAddress);
            }
            else
            {
                this.RootNode = CreateNode(degree);
            }
            return this.RootNode;
        }

        public void SetRootNode(Node<TNodeKey, long> rootNode)
        {
            this.RootNode = rootNode;
        }

        public Node<TNodeKey, long> GetNode(long address)
        {
            if (this.NodeCache.ContainsKey(address))
            {
                return this.NodeCache[address];
            }

            var record = RecordManager.GetRecord(address);
            var node = Serializer.Deserialize(record.Data);
            node.Address = record.Header.Address;

            CacheNode(node);

            return node;
        }

        public Node<TNodeKey, long> CreateNode(int degree)
        {
            var node = new Node<TNodeKey, long>(degree);
            var data = Serializer.Serialize(node);

            var record = RecordManager.AppendRecord(
                data,
                reuseRecycledRecord:this.ReuseRecycledRecords,
                allocationStrategy: this.AllocationStrategy);

            node.Address = record.Header.Address;

            this.MetaRecord.NumberOfNodes++;

            CacheNode(node);

            return node;
        }

        public void Flush()
        {
            var retry = true;
            while (retry)
            {
                retry = false;
                var nodesToKeep = new Dictionary<Int64, Node<TNodeKey,Int64>>();

                if (this.RootNode != null)
                {
                    nodesToKeep[this.RootNode.Address] = this.RootNode;
                    FindAllNodes(this.RootNode, nodesToKeep);
                }

                foreach (var nodeAddress in NodeCache.Keys)
                {
                    if (!nodesToKeep.ContainsKey(nodeAddress) && this.ReuseRecycledRecords)
                    {
                        RecordManager.Recycle(nodeAddress);
                        this.MetaRecord.NumberOfNodes--;
                    }
                }

                NodeCache = nodesToKeep;
                var updateNodes = new Dictionary<Int64, Node<TNodeKey, Int64>>() ;
                foreach (var node in NodeCache.Values)
                {
                    var record = RecordManager.GetRecord(node.Address);
                    var updateData = Serializer.Serialize(node);
                    var oldAddress = record.Header.Address;
                    var updatedRecord = RecordManager.UpdateRecord(
                        record,
                        updateData,
                        reuseRecycledRecord: this.ReuseRecycledRecords,
                        allocationStrategy: this.AllocationStrategy
                    );
                    if (oldAddress != updatedRecord.Header.Address)
                    {
                        node.Address = updatedRecord.Header.Address;
                        updateNodes.Add(oldAddress, node);
                        //update any children linking to this node
                        foreach(var n in NodeCache.Values){
                            var indexOfOldAddress = n.ChildrenAddresses.IndexOf(oldAddress);
                            if (indexOfOldAddress > 0)
                            {
                                n.ChildrenAddresses[indexOfOldAddress] = updatedRecord.Header.Address;
                                retry = true;
                            }
                        }
                    }
                }
                foreach (var oldAddress in updateNodes.Keys)
                {
                    NodeCache.Remove(oldAddress);
                    var node = updateNodes[oldAddress];
                    NodeCache[node.Address] = node;
                }
                this.MetaRecord.RootNodeAddress = this.RootNode.Address;
                SaveMetaRecord();
            }
        }
        #endregion
        private void FindAllNodes(Node<TNodeKey, Int64> node, Dictionary<Int64, Node<TNodeKey, Int64>> acc, int depth = 0)
        {
            if (depth > 64)
            {
                throw new InvalidOperationException("Panic, a btree node is linked self or one of its parents");
            }
            foreach(var childNodeAddress in node.ChildrenAddresses)
            {
                if (NodeCache.ContainsKey(childNodeAddress))
                {
                    var childNode = NodeCache[childNodeAddress];
                    acc[childNodeAddress] = childNode;
                    FindAllNodes(childNode, acc, depth +1);
                }
            }
        }

        private void CacheNode(Node<TNodeKey, long> node)
        {
            this.NodeCache.Add(node.Address, node);
        }

        private void SaveMetaRecord()
        {
            var serializer = new IndexMetaRecordSerializer();
            var bytes = serializer.Serialize(this.MetaRecord);
            this.RecordManager.UpdateRecord(this.MetaRecord.Record,bytes,this.ReuseRecycledRecords, null);
            this.RecordManager.RegisterNamedRecordAddress(
                this.RootRecordName,
                this.MetaRecord.Record.Header.Address,
                this.ReuseRecycledRecords);
        }
    }
}

