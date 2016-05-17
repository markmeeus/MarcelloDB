using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using System.Collections.Generic;
using MarcelloDB.AllocationStrategies;
using System.Linq;

namespace MarcelloDB.Index.BTree
{
    internal class RecordBTreeDataProvider<TNodeKey> :  SessionBoundObject, IBTreeDataProvider<TNodeKey>
    {
        internal IndexMetaRecord MetaRecord{ get; private set; }

        IRecordManager RecordManager { get; set; }

        IObjectSerializer<Node<TNodeKey>> Serializer { get; set; }

        Dictionary<Int64, Node<TNodeKey>> NodeCache { get; set; }

        string RootRecordName { get; set; }

        internal RecordBTreeDataProvider(
            Session session,
            IRecordManager recordManager,
            IObjectSerializer<Node<TNodeKey>> serializer,
            string rootRecordName
        ):base(session)
        {
            this.RecordManager = recordManager;
            this.Serializer = serializer;
            this.NodeCache = new Dictionary<long, Node<TNodeKey>>();
            this.RootRecordName = rootRecordName;
            this.Initialize();
        }

        void Initialize()
        {
            var serializer = this.Session.SerializerResolver.SerializerFor<IndexMetaRecord>();
            var rootRecordAddress = this.RecordManager.GetNamedRecordAddress(this.RootRecordName);
            if (rootRecordAddress <= 0)
            {
                var metaRecord = new IndexMetaRecord();
                var bytes = serializer.Serialize(metaRecord);

                metaRecord.Record =
                    this.RecordManager.AppendRecord(bytes, this.Session.AllocationStrategyResolver.StrategyFor(metaRecord));

                this.RecordManager.RegisterNamedRecordAddress(this.RootRecordName,
                    metaRecord.Record.Header.Address);

                this.MetaRecord = metaRecord;
            }
            else
            {
                var record = this.RecordManager.GetRecord(rootRecordAddress);
                this.MetaRecord = serializer.Deserialize(record.Data);
                this.MetaRecord.Record = record;
            }

        }

        Node<TNodeKey> RootNode {
            get
            {
                return this.GetRootNode();
            }
            set
            {
                this.SetRootNode(value);
            }
        }
        #region IBTreeDataProvider implementation
        public Node<TNodeKey> GetRootNode(int degree = -1)
        {
            if (this.MetaRecord.RootNodeAddress <= 0)
            {
                var rootNode = CreateNode(degree);
                this.MetaRecord.RootNodeAddress = rootNode.Address;
            }
            return GetNode(this.MetaRecord.RootNodeAddress);
        }

        public void SetRootNode(Node<TNodeKey> rootNode)
        {
            this.MetaRecord.RootNodeAddress = rootNode.Address;
        }

        public Node<TNodeKey> GetNode(long address)
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

        public Node<TNodeKey> CreateNode(int degree)
        {

            var node = new Node<TNodeKey>(degree);
            var data = Serializer.Serialize(node);
            var allocationStrategy = this.Session.AllocationStrategyResolver.StrategyFor(node);
            var record = RecordManager.AppendRecord(data, allocationStrategy);

            node.Address = record.Header.Address;

            this.MetaRecord.NumberOfNodes++;

            CacheNode(node);

            return node;
        }

        public void Flush()
        {
            var loadedNodes = this.NodeCache.ToDictionary(e => e.Key, e => e.Value);
            var rootNode = this.RootNode;

            //Clear node cache before persisting, persisting may cause re-entrancy
            this.NodeCache = new Dictionary<Int64, Node<TNodeKey>>();

            new NodePersistence<TNodeKey>(this.Session, this.RecordManager).
                Persist(
                        rootNode,
                    loadedNodes,
                    this.Serializer,
                this.MetaRecord);

            this.MetaRecord.RootNodeAddress = rootNode.Address;
            SaveMetaRecord();
        }

        #endregion
        private void FindAllNodes(Node<TNodeKey> node, Dictionary<Int64, Node<TNodeKey>> acc, int depth = 0)
        {
            if (depth > 64)
            {
                throw new InvalidOperationException("Panic, a btree node is linked self or one of its parents");
            }
            foreach(var childNodeAddress in node.ChildrenAddresses.Addresses)
            {
                if (NodeCache.ContainsKey(childNodeAddress))
                {
                    var childNode = NodeCache[childNodeAddress];
                    acc[childNodeAddress] = childNode;
                    FindAllNodes(childNode, acc, depth +1);
                }
            }
        }

        private void CacheNode(Node<TNodeKey> node)
        {
            this.NodeCache.Add(node.Address, node);
        }

        private void SaveMetaRecord()
        {
            var serializer = this.Session.SerializerResolver.SerializerFor<IndexMetaRecord>();
            var bytes = serializer.Serialize(this.MetaRecord);
            this.RecordManager.UpdateRecord(this.MetaRecord.Record,bytes, null);
            this.RecordManager.RegisterNamedRecordAddress(
                this.RootRecordName,
                this.MetaRecord.Record.Header.Address);
        }
    }
}

