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

        internal GreaterThan(IndexedValue<TObj, TAttribute> indexedValue, TAttribute value)
        {
            this.IndexedValue = indexedValue;
            this.Value = value;
        }

        #region IEnumerable implementation

        public IEnumerator<TObj> GetEnumerator()
        {
            var startKey = new ValueWithAddressIndexKey{ V = (IComparable)this.Value };
            var range = new BTreeWalkerRange<ValueWithAddressIndexKey>();
            range.StartAt = startKey;
            range.IncludeStartAt = false;
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

