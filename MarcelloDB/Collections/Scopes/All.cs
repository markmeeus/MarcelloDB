using System;
using MarcelloDB.Records;
using System.Collections;
using System.Collections.Generic;

namespace MarcelloDB.Collections.Scopes
{
    public class All<TObj, TAttribute> : IEnumerable<TObj>
    {
        IndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        bool IsDescending { get; set; }

        internal All(IndexedValue<TObj, TAttribute> indexedValue, bool isDescending = false)
        {
            this.IndexedValue = indexedValue;
            this.IsDescending = isDescending;
        }            

        public All<TObj, TAttribute> Descending 
        {
            get 
            {
                return new All<TObj, TAttribute>(this.IndexedValue, true);
            }
        }

        #region IEnumerable implementation

        public IEnumerator<TObj> GetEnumerator()
        {
            return this.IndexedValue
                .BuildEnumerator(null, this.IsDescending)
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

