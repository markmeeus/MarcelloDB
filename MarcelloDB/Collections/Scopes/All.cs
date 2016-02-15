using System;
using MarcelloDB.Records;
using System.Collections;
using System.Collections.Generic;

namespace MarcelloDB.Collections.Scopes
{
    public class All<TObj, TAttribute> : IEnumerable<TObj>
    {
        IndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        TAttribute Value { get; set; }

        bool OrEqual { get; set; }

        internal All(IndexedValue<TObj, TAttribute> indexedValue)
        {
            this.IndexedValue = indexedValue;
        }

        #region IEnumerable implementation

        public IEnumerator<TObj> GetEnumerator()
        {
            return this.IndexedValue
                .BuildEnumerator(null)
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

