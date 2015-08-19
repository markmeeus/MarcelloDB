using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using System.Collections.Generic;

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
            IObjectSerializer<Node<TK, TP>> serializer)
        {
            var touchedNodes = new Dictionary<long, Node<TK, TP>>();
            Persist(node, loadedNodes, serializer, touchedNodes);
            RecycleUntoucedNodes(loadedNodes, touchedNodes);
        }

        private void Persist(
            Node<TK, TP> node,
            Dictionary<Int64, Node<TK, TP>> loadedNodes,
            IObjectSerializer<Node<TK, TP>> serializer,
            Dictionary<Int64, Node<TK, TP>> touchedNodes)
        {

            SaveChildren(node, loadedNodes, serializer, touchedNodes);

            touchedNodes.Add(node.Address, node);

            if (!node.Dirty)
            {
                return;
            }
            var bytes = serializer.Serialize(node);

            if (node.Address <= 0)
            {
                var record = this.RecordManager.AppendRecord(bytes);
                node.Address = record.Header.Address;
            }
            else
            {
                var record = this.RecordManager.GetRecord(node.Address);
                record = this.RecordManager.UpdateRecord(record, bytes);
                node.Address = record.Header.Address;
            }
        }

        void SaveChildren(
            Node<TK, TP> node,
            Dictionary<long, Node<TK, TP>> loadedNodes,
            IObjectSerializer<Node<TK, TP>> serializer,
            Dictionary<Int64, Node<TK, TP>> touchedNodes)
        {
            var updatedChildAddresses = new List<Int64[]>();

            foreach (var childAddress in node.ChildrenAddresses.Addresses)
            {
                if(loadedNodes.ContainsKey(childAddress))
                {
                    var childNode = loadedNodes[childAddress];
                    Persist(loadedNodes[childAddress], loadedNodes, serializer, touchedNodes);
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

        void RecycleUntoucedNodes(Dictionary<long, Node<TK, TP>> loadedNodes, Dictionary<long, Node<TK, TP>> touchedNodes)
        {
            foreach (var node in loadedNodes.Values)
            {
                if(!touchedNodes.ContainsKey(node.Address))
                {
                    this.RecordManager.Recycle(node.Address);
                }
            }
        }
    }
}

