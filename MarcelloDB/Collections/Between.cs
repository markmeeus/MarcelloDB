using System;
using System.Collections.Generic;
using System.Collections;

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
            return this.IndexedValue.BuildEnumerator(this.StartAt, this.EndAt).GetEnumerator();
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

