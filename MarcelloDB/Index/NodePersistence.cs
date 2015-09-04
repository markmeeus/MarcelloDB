using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using System.Collections.Generic;
using System.Linq;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB
{
    internal class NodePersistence<TK, TP>
    {
        IRecordManager RecordManager { get; set; }

        internal NodePersistence(IRecordManager recordManager)
        {
            this.RecordManager = recordManager;
        }

        internal void Persist(
            Node<TK, TP> node,
            Dictionary<Int64, Node<TK, TP>> loadedNodes,
            IObjectSerializer<Node<TK, TP>> serializer,
            IndexMetaRecord metaRecord)
        {
            var touchedNodes = new Dictionary<Int64, Node<TK, TP>>();
            Persist(node, loadedNodes, serializer, touchedNodes, metaRecord);
            RecycleUntouchedNodes(loadedNodes, touchedNodes, metaRecord);
        }

        private void Persist(
            Node<TK, TP> node,
            Dictionary<Int64, Node<TK, TP>> loadedNodes,
            IObjectSerializer<Node<TK, TP>> serializer,
            Dictionary<Int64, Node<TK, TP>> touchedNodes,
            IndexMetaRecord metaRecord)
        {

            SaveChildren(node, loadedNodes, serializer, touchedNodes, metaRecord);

            if (!node.Dirty)
            {
                touchedNodes.Add(node.Address, node);
                return;
            }

            //setting it dirty now, EmptyRecordIndex nodes may become dirty while persisting due to re-use.
            node.ClearChanges();

            var bytes = serializer.Serialize(node);
            var allocationStrategy = AllocationStrategy.StrategyFor(node);
            if (node.Address <= 0)
            {
                var record = this.RecordManager.AppendRecord(bytes, allocationStrategy);
                node.Address = record.Header.Address;
            }
            else
            {
                var record = this.RecordManager.GetRecord(node.Address);
                record = this.RecordManager.UpdateRecord(record, bytes, allocationStrategy);
                node.Address = record.Header.Address;
            }

            touchedNodes.Add(node.Address, node);
        }

        void SaveChildren(
            Node<TK, TP> node,
            Dictionary<long, Node<TK, TP>> loadedNodes,
            IObjectSerializer<Node<TK, TP>> serializer,
            Dictionary<Int64, Node<TK, TP>> touchedNodes,
            IndexMetaRecord metaRecord)
        {
            var updatedChildAddresses = new List<Int64[]>();

            foreach (var childAddress in node.ChildrenAddresses.Addresses)
            {
                if(loadedNodes.ContainsKey(childAddress))
                {
                    var childNode = loadedNodes[childAddress];
                    Persist(loadedNodes[childAddress], loadedNodes, serializer, touchedNodes, metaRecord);
                    if (childNode.Address != childAddress)
                    {
                        updatedChildAddresses.Add(new Int64[]{childAddress, childNode.Address});
                    }
                }
            }

            foreach (var addresses in updatedChildAddresses)
            {
                node.ChildrenAddresses[node.ChildrenAddresses.IndexOf(addresses[0])] = addresses[1];
            }
        }

        void RecycleUntouchedNodes(
            Dictionary<Int64, Node<TK, TP>> loadedNodes,
            Dictionary<Int64, Node<TK, TP>> touchedNodes,
            IndexMetaRecord metaRecord)
        {
            foreach (var node in loadedNodes.Values)
            {
                if(!touchedNodes.ContainsKey(node.Address))
                {
                    metaRecord.NumberOfNodes--;
                    this.RecordManager.Recycle(node.Address);
                }
            }
        }
    }
}

