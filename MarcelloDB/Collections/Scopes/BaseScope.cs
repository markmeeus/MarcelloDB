using System;
using System.Collections.Generic;
using System.Linq;
using MarcelloDB.Records;
using MarcelloDB.Collections;
using System.Collections;
using MarcelloDB.Index;

namespace MarcelloDB
{

    public abstract class BaseScope<TObj, TAttribute> : IEnumerable<TObj>
    {
        ObjectComparer Comparer { get; set; } 

        public BaseScope()
        {
            this.Comparer = new ObjectComparer();
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

                ValueWithAddressIndexKey previousKey = null;
                bool first = true;

                foreach (var key in keyEnumerator)
                {
                    if (first || 
                        (this.Comparer.Compare(previousKey.V, key.V) != 0))
                    {                         
                        yield return (TAttribute)key.V;
                    }
                    first = false;
                    previousKey = key;
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

