using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using System.Collections.Generic;

namespace MarcelloDB.Index.BTree
{
    internal class RecordBTreeDataProvider :  IBTreeDataProvider<object, Int64>
    {
        IRecordManager RecordManager { get; set; }
        IObjectSerializer<Node<object, Int64>> Serializer { get; set; }
        Dictionary<Int64, Node<object, Int64>> NodeCache { get; set; }
        Node<object, Int64> RootNode { get; set; }
        string RootRecordName { get; set; }
        bool CanReuseRecycledRecords { get; set; }

        internal RecordBTreeDataProvider(
            IRecordManager recordManager, 
            IObjectSerializer<Node<object, Int64>> serializer,
            string rootRecordName,
            bool canReuseRecycledRecords)
        {
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.NodeCache = new Dictionary<long, Node<object, long>>();
            this.RootRecordName = rootRecordName;
            this.CanReuseRecycledRecords = canReuseRecycledRecords;
        }

        #region IBTreeDataProvider implementation
        public Node<object, long> GetRootNode(int degree)
        {
            var rootRecordAddress = this.RecordManager.GetNamedRecordAddress(this.RootRecordName);
            if (rootRecordAddress > 0)
            {
                this.RootNode = GetNode(rootRecordAddress);
            }
            else
            {
                this.RootNode = CreateNode(degree);
                this.RecordManager.RegisterNamedRecordAddress(this.RootRecordName, this.RootNode.Address, CanReuseRecycledRecords);
            }
            return this.RootNode;
        }            

        public void SetRootNodeAddress(long rootNodeAddress)
        {
            this.RecordManager.RegisterNamedRecordAddress(this.RootRecordName, rootNodeAddress, CanReuseRecycledRecords);
        }

        public Node<object, long> GetNode(long address)
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

        public Node<object, long> CreateNode(int degree)
        {
            var node = new Node<object, long>(degree);
            var data = Serializer.Serialize(node);

            var record = RecordManager.AppendRecord(data, reuseRecycledRecord:this.CanReuseRecycledRecords);

            node.Address = record.Header.Address;

            CacheNode(node);

            return node;
        }

        public void Flush()
        {
            var retry = true;
            while (retry)
            {
                retry = false;
                var nodesToKeep = new Dictionary<Int64, Node<Object,Int64>>();

                if (this.RootNode != null)
                {                    
                    nodesToKeep[this.RootNode.Address] = this.RootNode;
                    FindAllNodes(this.RootNode, nodesToKeep);
                }

                foreach (var nodeAddress in NodeCache.Keys)
                {                    
                    if (!nodesToKeep.ContainsKey(nodeAddress) && CanReuseRecycledRecords)
                    {
                        RecordManager.Recycle(nodeAddress);
                    }

                }

                NodeCache = nodesToKeep;
                foreach (var node in NodeCache.Values)
                {
                    var record = RecordManager.GetRecord(node.Address);
                    var updateData = Serializer.Serialize(node);
                    var oldAddress = record.Header.Address;
                    var updatedRecord = RecordManager.UpdateRecord(record, updateData, CanReuseRecycledRecords);
                    if (oldAddress != updatedRecord.Header.Address)
                    {
                        //update any children linking to this node
                        foreach(var n in NodeCache.Values){
                            if (n.Address == oldAddress)
                            {
                                n.Address = updatedRecord.Header.Address;
                            }
                            var indexOfOldAddress = n.ChildrenAddresses.IndexOf(oldAddress);
                            if (indexOfOldAddress > 0)
                            {
                                n.ChildrenAddresses[indexOfOldAddress] = updatedRecord.Header.Address;
                                retry = true;
                            }
                        }
                    }
                }
                    
            }                
        }            
        #endregion
        private void FindAllNodes(Node<object, Int64> node, Dictionary<Int64, Node<object, Int64>> acc )
        {
            foreach(var childNodeAddress in node.ChildrenAddresses)
            {
                var childNode = GetNode(childNodeAddress);
                acc[node.Address] = node;
                FindAllNodes(node, acc);
            }
        }

        private void CacheNode(Node<object, long> node)
        {
            try{                
                this.NodeCache.Add(node.Address, node);
            }catch(Exception e){
                
            }
        }
    }
}

