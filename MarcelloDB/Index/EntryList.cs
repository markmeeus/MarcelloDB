using System;
using MarcelloDB.Helpers;
using System.Collections.Generic;

namespace MarcelloDB.Index
{
    internal class EntryList<TKey, TAddress> : ChangeTrackingList<Entry<TKey, TAddress>>, IEnumerable<Entry<TKey, TAddress>>
    {

        internal void SetEntries(List<Entry<TKey, TAddress>> entries)
        {
            base.Items = entries;
        }

        #region IEnumerable implementation

        public IEnumerator<Entry<TKey, TAddress>> GetEnumerator()
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

