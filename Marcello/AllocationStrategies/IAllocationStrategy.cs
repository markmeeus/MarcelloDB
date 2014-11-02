using System;
using Marcello.Records;

namespace Marcello.AllocationStrategies
{
    internal interface IAllocationStrategy
    {
        int CalculateSize(Record record);
    }
}

