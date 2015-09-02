using System;

namespace MarcelloDB.AllocationStrategies
{
    public class ExactSizeAllocationStrategy : IAllocationStrategy
    {
        #region IAllocationStrategy implementation

        public int CalculateSize(int dataSize)
        {
            return dataSize;
        }

        #endregion
    }
}

