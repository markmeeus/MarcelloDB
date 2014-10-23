using System;

namespace Marcello
{
    internal class DoubleSizeAllocationStrategy : IAllocationStrategy
    {
        #region IAllocationStrategy implementation

        public int CalculateSize (Record record)
        {
            return record.data.Length * 2;
        }

        #endregion


    }
}

