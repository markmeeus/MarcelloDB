using System;
using MarcelloDB.Records;

namespace MarcelloDB.AllocationStrategies
{
    internal interface IAllocationStrategy
    {
        int CalculateSize(Record record);
    }
}

