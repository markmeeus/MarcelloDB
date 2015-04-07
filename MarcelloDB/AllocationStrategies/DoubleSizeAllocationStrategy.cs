using System;
using MarcelloDB.Records;

namespace MarcelloDB.AllocationStrategies
{
    internal class DoubleSizeAllocationStrategy : IAllocationStrategy
    {
        public int CalculateSize(Record record)
        {
            return record.Header.DataSize * 2;
        }
    }
}

