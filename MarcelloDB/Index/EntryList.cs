using System;
using MarcelloDB.Helpers;
using System.Collections.Generic;

namespace MarcelloDB.Index
{
    internal class EntryList<TKey, TAddress> : ChangeTrackingList<Entry<TKey, TAddress>>
    {
        public  List<Entry<TKey, TAddress>> Entries
        {
            get
            {
                return base.Items;
            }
        }

        internal void SetEntries(List<Entry<TKey, TAddress>> entries)
        {
            base.Items = entries;
        }
    }
}

