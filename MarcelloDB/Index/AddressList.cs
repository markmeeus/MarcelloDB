using System;
using System.Collections.Generic;
using System.Linq;
using MarcelloDB.Helpers;

namespace MarcelloDB.Index
{
    internal class AddressList : ChangeTrackingList<Int64>
    {
        internal List<Int64> Addresses
        {
            get
            {
                return Items;
            }
        }

        internal void SetAddresses(List<Int64> addresses)
        {
            base.Items = addresses;
        }
    }
}

