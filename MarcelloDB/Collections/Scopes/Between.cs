using System;
using System.Collections.Generic;
using System.Collections;
using MarcelloDB.Records;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Collections.Scopes
{
    public class Between<TObj, TAttribute> : IEnumerable<TObj>
    {
        IndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        TAttribute StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        TAttribute EndAt { get; set; }

        bool IncludeEndAt { get; set; }

        internal Between(
            IndexedValue<TObj, TAttribute> indexedValue,
            TAttribute startAt, bool includeStartAt,
            TAttribute endAt, bool includeEndAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
            this.EndAt = endAt;
            this.IncludeEndAt = includeEndAt;
        }

        #region IEnumerable implementation

        public IEnumerator<TObj> GetEnumerator()
        {
            var startKey = new ValueWithAddressIndexKey{ V = (IComparable)this.StartAt };
            var endKey =  new ValueWithAddressIndexKey{ V = (IComparable)this.EndAt };
            var range = new BTreeWalkerRange<ValueWithAddressIndexKey>(startKey, endKey);
            range.IncludeStartAt = this.IncludeStartAt;
            range.IncludeEndAt = this.IncludeEndAt;
            return this.IndexedValue.BuildEnumerator(range).GetEnumerator();
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

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(IndexedValue<TObj, TAttribute> indexedValue, TAttribute startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

        public Between<TObj,TAttribute> And(TAttribute endAt)
        {
            return new Between<TObj, TAttribute>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj,TAttribute> AndIncluding(TAttribute endAt)
        {
            return new Between<TObj, TAttribute>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
    }
}

