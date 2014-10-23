using System;

namespace Marcello
{
    internal interface IAllocationStrategy
    {
        int CalculateSize(Record record);
    }
}

