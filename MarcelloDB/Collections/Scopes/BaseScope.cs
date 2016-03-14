using System;
using System.Collections.Generic;
using System.Linq;
using MarcelloDB.Records;
using MarcelloDB.Collections;
using System.Collections;

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

        public IEnumerable<TAttribute> Keys
        {
            get
            {
                var keyEnumerator = (IEnumerable<ValueWithAddressIndexKey>) BuildEnumerator(false)
                    .GetKeyEnumerator();
                foreach (var key in keyEnumerator)
                {
                    yield return (TAttribute)key.V;
                }
            }
        }

        #region IEnumerable implementation

        IEnumerator<TObj> IEnumerable<TObj>.GetEnumerator()
        {
            //default to ascending enumeration
            return ((IEnumerable<TObj>)BuildEnumerator(false)).GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TObj>)this).GetEnumerator();
        }

        #endregion
    }
}

