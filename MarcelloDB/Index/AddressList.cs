using System;
using System.Collections.Generic;
using System.Linq;
using MarcelloDB.Helpers;

namespace MarcelloDB.Index
{
    internal class AddressList : ChangeTrackingList<Int64>, IEnumerable<Int64>
    {
        internal void SetAddresses(List<Int64> addresses)
        {
            base.Items = addresses;
        }

        #region IEnumerable implementation

        public IEnumerator<long> GetEnumerator()
        {
            return base.Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}

