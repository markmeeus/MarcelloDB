using System;
using System.Collections.Generic;
using System.Collections;
using MarcelloDB.Records;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Collections
{
    public class Between<TObj, TAttribute> : IEnumerable<TObj>
    {
        IndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        TAttribute StartAt { get; set; }

        TAttribute EndAt { get; set; }

        internal Between(IndexedValue<TObj, TAttribute> indexedValue, TAttribute startAt, TAttribute endAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.EndAt = endAt;
        }

        #region IEnumerable implementation

        public IEnumerator<TObj> GetEnumerator()
        {
            var startKey = new ValueWithAddressIndexKey{ V = (IComparable)this.StartAt };
            var endKey =  new ValueWithAddressIndexKey{ V = (IComparable)this.EndAt };
            return this.IndexedValue.BuildEnumerator(
                new BTreeWalkerRange<ValueWithAddressIndexKey>(startKey, endKey)).GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    public class BetweenBuilder<TObj, TAttribute>
    {
        IndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        TAttribute StartAt { get; set; }

        internal BetweenBuilder(IndexedValue<TObj, TAttribute> indexedValue, TAttribute startAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
        }

        public Between<TObj,TAttribute> And(TAttribute endAt)
        {
            return new Between<TObj, TAttribute>(this.IndexedValue, this.StartAt, endAt);
        }
    }
}

