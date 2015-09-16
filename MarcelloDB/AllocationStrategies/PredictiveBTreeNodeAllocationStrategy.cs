using System;
using MarcelloDB.Index;

namespace MarcelloDB.AllocationStrategies
{
    internal class PredictiveBTreeNodeAllocationStrategy<TK, TP> : IAllocationStrategy
    {
        Node<TK, TP> Node {  get; set; }

        internal PredictiveBTreeNodeAllocationStrategy(Node<TK, TP> node)
        {
            this.Node = node;
        }

        #region IAllocationStrategy implementation

        public int CalculateSize(int dataSize)
        {
            if (Node.ChildrenAddresses.Count == 0 && Node.EntryList.Count == 0)
            {
                return dataSize;
            }

            var maxEntries = Index.Node.MaxEntriesForDegree(Node.Degree);
            var maxChildren = Index.Node.MaxChildrenForDegree(Node.Degree);

            var filledEntries = 100 * ((float)Node.EntryList.Count / (float) maxEntries);
            var filled = filledEntries;

            if (!Node.IsLeaf)//for non-leaf nodes, take children into account
            {
                var filledChildrenAddresses = 100 * ((float)Node.ChildrenAddresses.Count / (float) maxChildren);
                filled = (filledEntries + filledChildrenAddresses) / 2;
            }

            var predictedEntriesDataSize = dataSize * (int)filled;
            return 2 * predictedEntriesDataSize; //double the size to have some buffer
        }

        #endregion
    }
}

