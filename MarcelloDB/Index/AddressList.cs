using System;
using System.Collections.Generic;
using System.Linq;
using MarcelloDB.Helpers;

namespace MarcelloDB.Index
{
    public class AddressList : ChangeTrackingList<Int64>
    {
        internal Int64[] Addresses
        {
            get
            {
                return Items;
            }
        }
    }
}

