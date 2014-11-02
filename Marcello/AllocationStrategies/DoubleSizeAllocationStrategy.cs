using System;
using Marcello.Records;

namespace Marcello.AllocationStrategies
{
    internal class DoubleSizeAllocationStrategy : IAllocationStrategy
    {
        #region IAllocationStrategy implementation

        public int CalculateSize(Record record)
        {
            return RecordHeader.ByteSize + (record.Header.DataSize * 2);
        }

        #endregion


    }
}

