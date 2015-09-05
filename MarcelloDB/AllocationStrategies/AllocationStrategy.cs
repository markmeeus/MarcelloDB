using System;
using MarcelloDB.Index;
using MarcelloDB.Records;

namespace MarcelloDB.AllocationStrategies
{
    internal static class AllocationStrategy
    {
        internal static IAllocationStrategy StrategyFor(Node<EmptyRecordIndexKey, Int64> emptyRecordIndexNode)
        {
            return new ExactSizeAllocationStrategy();
        }

        internal static IAllocationStrategy StrategyFor<TK, TP>(Node<TK, TP> node)
        {
            return new PredictiveBTreeNodeAllocationStrategy<TK, TP>(node);
        }

        internal static IAllocationStrategy StrategyFor(object  obj)
        {
            if(obj.GetType() == typeof(Node<EmptyRecordIndexKey, Int64>))
            {
                return StrategyFor((Node<EmptyRecordIndexKey, Int64>)obj);
            }
            return new DoubleSizeAllocationStrategy();
        }
    }
}

