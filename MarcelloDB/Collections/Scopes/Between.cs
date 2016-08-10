using System;
using System.Collections.Generic;
using System.Collections;
using MarcelloDB.Records;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Collections.Scopes
{
    public class Between<TObj, TAttribute> : BaseScope<TObj, TAttribute>
    {
        BaseIndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        TAttribute StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        TAttribute EndAt { get; set; }

        bool IncludeEndAt { get; set; }

        internal Between(
            BaseIndexedValue<TObj, TAttribute> indexedValue,
            TAttribute startAt, bool includeStartAt,
            TAttribute endAt, bool includeEndAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
            this.EndAt = endAt;
            this.IncludeEndAt = includeEndAt;
        }

        #region implemented abstract members of BaseScope

        internal override CollectionEnumerator<TObj, ValueWithAddressIndexKey<TAttribute>> BuildEnumerator(bool descending)
        {
            var startKey = new ValueWithAddressIndexKey<TAttribute>{ V = this.StartAt };
            var endKey =  new ValueWithAddressIndexKey<TAttribute>{ V = this.EndAt };

            BTreeWalkerRange<ValueWithAddressIndexKey<TAttribute>> range;
            if (!descending)
            {
                range = new BTreeWalkerRange<ValueWithAddressIndexKey<TAttribute>>(startKey, endKey);
                range.IncludeStartAt = this.IncludeStartAt;
                range.IncludeEndAt = this.IncludeEndAt;
            }
            else
            {
                //start and end are reversed
                range = new BTreeWalkerRange<ValueWithAddressIndexKey<TAttribute>>(endKey, startKey);
                range.IncludeStartAt = this.IncludeEndAt;
                range.IncludeEndAt = this.IncludeStartAt;
            }

            return this.IndexedValue.BuildEnumerator(range, descending);
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

