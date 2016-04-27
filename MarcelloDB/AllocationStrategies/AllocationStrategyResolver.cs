using System;
using MarcelloDB.Index;
using MarcelloDB.Records;

namespace MarcelloDB.AllocationStrategies
{
    internal class AllocationStrategyResolver
    {
        internal IAllocationStrategy StrategyFor(Node<EmptyRecordIndexKey> emptyRecordIndexNode)
        {
            return new ExactSizeAllocationStrategy();
        }

        internal  IAllocationStrategy StrategyFor<TK>(Node<TK> node)
        {
            return new PredictiveBTreeNodeAllocationStrategy<TK>(node);
        }

        internal IAllocationStrategy StrategyFor(object  obj)
        {
            if(obj.GetType() == typeof(Node<EmptyRecordIndexKey>))
            {
                return StrategyFor((Node<EmptyRecordIndexKey>)obj);
            }
            return new DoubleSizeAllocationStrategy();
        }
    }
}

