using System;
using MarcelloDB.Records;

namespace MarcelloDB.AllocationStrategies
{
    internal class DoubleSizeAllocationStrategy : IAllocationStrategy
    {
        public int CalculateSize(int dataSize)
        {
            return dataSize * 2;
        }
    }
}

