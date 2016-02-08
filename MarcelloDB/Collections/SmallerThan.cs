using System;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Index.BTree;
using System.Collections;

namespace MarcelloDB.Collections
{
    public class SmallerThan<TObj, TAttribute> : IEnumerable<TObj>
    {
        IndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        TAttribute Value { get; set; }

        internal SmallerThan(IndexedValue<TObj, TAttribute> indexedValue, TAttribute value)
        {
            this.IndexedValue = indexedValue;
            this.Value = value;
        }

        #region IEnumerable implementation

        public IEnumerator<TObj> GetEnumerator()
        {
            var startKey = new ValueWithAddressIndexKey{ V = (IComparable)this.Value };
            var range = new BTreeWalkerRange<ValueWithAddressIndexKey>();
            range.SetEndAt(startKey);
            range.IncludeEndAt = false;
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

