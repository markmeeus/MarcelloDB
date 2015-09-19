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

            var fillFactor = (float) maxEntries / (float)Node.EntryList.Count;

            var predictedEntriesDataSize = dataSize * fillFactor;
            var maxSizeGuesstimation = 2 * (int)predictedEntriesDataSize;
            if (maxSizeGuesstimation < dataSize)
            {
                return dataSize * 2;
            }
            return maxSizeGuesstimation; //double the size to have some buffer
        }

        #endregion
    }
}

