using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using System.Collections.Generic;

namespace MarcelloDB.Index
{
    internal class RecordBTreeDataProvider :  IBTreeDataProvider<object, Int64>
    {
        IRecordManager RecordManager { get; set; }
        IObjectSerializer<Node<object, Int64>> Serializer { get; set; }
        Dictionary<Int64, Node<object, Int64>> NodeCache { get; set; }
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
                return GetNode(rootRecordAddress);
            }
            else
            {
                var node = CreateNode(degree);
                this.RecordManager.RegisterNamedRecordAddress(this.RootRecordName, node.Address);
                return node;
            }
        }            

        public void SetRootNodeAddress(long rootNodeAddress)
        {
            this.RecordManager.RegisterNamedRecordAddress(this.RootRecordName, rootNodeAddress);
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

                foreach (var node in NodeCache.Values)
                {
                    var record = RecordManager.GetRecord(node.Address);
                    var updateData = Serializer.Serialize(node);
                    var oldAddress = record.Header.Address;
                    var updatedRecord = RecordManager.UpdateRecord(record, updateData);
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

        private void CacheNode(Node<object, long> node)
        {
            this.NodeCache.Add(node.Address, node);
        }
    }
}

