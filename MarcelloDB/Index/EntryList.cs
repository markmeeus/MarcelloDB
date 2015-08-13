using System;
using MarcelloDB.Helpers;
using System.Collections.Generic;

namespace MarcelloDB.Index
{
    public class EntryList<TKey, TAddress> : ChangeTrackingList<Entry<TKey, TAddress>>
    {
        public  IReadOnlyList<Entry<TKey, TAddress>> Entries
        {
            get
            {
                return base.Items;
            }
        }
    }
}

