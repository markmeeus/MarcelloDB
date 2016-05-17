using System;
using MarcelloDB.Helpers;
using System.Collections.Generic;

namespace MarcelloDB.Index
{

    internal class EntryList<TKey> : ChangeTrackingList<Entry<TKey>>
    {
        public  List<Entry<TKey>> Entries
        {
            get
            {
                return base.Items;
            }
        }

        internal void SetEntries(List<Entry<TKey>> entries)
        {
            base.Items = entries;
        }
    }
}

