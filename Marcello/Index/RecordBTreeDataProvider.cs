using System;
using Marcello.Records;
using Marcello.Serialization;
using System.Collections.Generic;

namespace Marcello.Index
{
    internal class RecordBTreeDataProvider :  IBTreeDataProvider<object, Int64>
    {
        IRecordManager RecordManager { get; set; }
        IObjectSerializer<Node<object, Int64>> Serializer { get; set; }
        Dictionary<Int64, Node<object, Int64>> NodeCache { get; set; }
        string RootRecordName {get;set;}

        internal RecordBTreeDataProvider(
            IRecordManager recordManager, 
            IObjectSerializer<Node<object, Int64>> serializer,
            string rootRecordName)
        {
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.NodeCache = new Dictionary<long, Node<object, long>>();
            this.RootRecordName = rootRecordName;
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
            var record = RecordManager.AppendRecord(data);

            node.Address = record.Header.Address;

            CacheNode(node);

            return node;
        }

        #endregion
        //flushUnused

        #region private methods
        private void CacheNode(Node<object, long> node){
            this.NodeCache.Add(node.Address, node);
        }
        #endregion
    }
}

