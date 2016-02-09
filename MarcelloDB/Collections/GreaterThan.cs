using System;
using System.Collections.Generic;
using System.Collections;
using MarcelloDB.Records;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Collections
{
    public class GreaterThan<TObj, TAttribute> : IEnumerable<TObj>
    {
        IndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        TAttribute Value { get; set; }

        bool OrEqual { get; set; }

        internal GreaterThan(IndexedValue<TObj, TAttribute> indexedValue, TAttribute value, bool orEqual)
        {
            this.IndexedValue = indexedValue;
            this.Value = value;
            this.OrEqual = orEqual;
        }

        #region IEnumerable implementation

        public IEnumerator<TObj> GetEnumerator()
        {
            var startKey = new ValueWithAddressIndexKey{ V = (IComparable)this.Value };
            var range = new BTreeWalkerRange<ValueWithAddressIndexKey>();
            range.SetStartAt(startKey);
            range.IncludeStartAt = this.OrEqual;
            return this.IndexedValue
                .BuildEnumerator(range)
                .GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}

