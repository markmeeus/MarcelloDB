using System;

namespace MarcelloDB.AllocationStrategies
{
    internal class ExactSizeAllocationStrategy : IAllocationStrategy
    {
        #region IAllocationStrategy implementation

        public int CalculateSize(int dataSize)
        {
            return dataSize;
        }

        #endregion
    }
}

