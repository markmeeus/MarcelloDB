using System;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Collections;

namespace MarcelloDB
{
    public abstract class BaseScope<TObj, TAttribute> : IEnumerable<TObj>
    {
        public BaseScope()
        {
        }

        abstract internal CollectionEnumerator<TObj, ValueWithAddressIndexKey> BuildEnumerator(bool descending);

        public Descending<TObj, TAttribute> Descending
        {
            get
            {
                return new Descending<TObj, TAttribute>(this);
            }
        }

        #region IEnumerable implementation

        public IEnumerator<TObj> GetEnumerator()
        {
            //default to ascending enumeration
            return BuildEnumerator(false).GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}

