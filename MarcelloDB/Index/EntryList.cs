using System;
using MarcelloDB.Helpers;

namespace MarcelloDB.Index
{
    public class EntryList<TKey, TAddress> : ChangeTrackingList<Entry<TKey, TAddress>>
    {
        public  Entry<TKey, TAddress>[] Entries
        {
            get
            {
                return base.Items;
            }
        }
    }
}

