using System;
using Marcello.Records;
using Marcello.Serialization;
using System.Collections.Generic;

namespace Marcello.Index
{
    internal class RecordBTreeDataProvider :  IBTreeDataProvider<ComparableObject, Int64>
    {
        IRecordManager RecordManager { get; set; }
        IObjectSerializer<Node<ComparableObject, Int64>> Serializer { get; set; }
        Dictionary<Int64, Node<ComparableObject, Int64>> NodeCache { get; set; }
        string RootRecordName {get;set;}

        internal RecordBTreeDataProvider(
            IRecordManager recordManager, 
            IObjectSerializer<Node<ComparableObject, Int64>> serializer,
            string rootRecordName)
        {
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.NodeCache = new Dictionary<long, Node<ComparableObject, long>>();
            this.RootRecordName = rootRecordName;
        }

        #region IBTreeDataProvider implementation

        public Node<ComparableObject, long> GetRootNode(int degree)
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

        public Node<ComparableObject, long> GetNode(long address)
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

        public Node<ComparableObject, long> CreateNode(int degree)
        {
            var node = new Node<ComparableObject, long>(degree);
            var data = Serializer.Serialize(node);
            var record = RecordManager.AppendRecord(data);

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
                
            var rootNode = GetRootNode(1024);
            this.RecordManager.RegisterNamedRecordAddress(this.RootRecordName, rootNode.Address);

        }
        #endregion
        //flushUnused

        #region private methods
        private void CacheNode(Node<ComparableObject, long> node){
            this.NodeCache.Add(node.Address, node);
        }
        #endregion
    }
}

