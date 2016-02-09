using System;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Index.BTree;
using System.Collections;

namespace MarcelloDB.Collections.Scopes
{
    public class SmallerThan<TObj, TAttribute> : IEnumerable<TObj>
    {
        IndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        TAttribute Value { get; set; }

        bool OrEqual { get; set; }

        internal SmallerThan(IndexedValue<TObj, TAttribute> indexedValue, TAttribute value, bool orEqual)
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
            range.SetEndAt(startKey);
            range.IncludeEndAt = this.OrEqual;
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

