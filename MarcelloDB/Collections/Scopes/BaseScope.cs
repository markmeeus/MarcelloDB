using System;
using System.Collections.Generic;
using System.Linq;
using MarcelloDB.Records;
using MarcelloDB.Collections;
using System.Collections;
using MarcelloDB.Index;

namespace MarcelloDB.Collections.Scopes
{

    public abstract class BaseScope<TObj, TAttribute> : IEnumerable<TObj>
    {
        ObjectComparer Comparer { get; set; }

        public BaseScope()
        {
            this.Comparer = new ObjectComparer();
        }

        abstract internal CollectionEnumerator<TObj, ValueWithAddressIndexKey<TAttribute>> BuildEnumerator(bool descending);

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
                var keyEnumerator = (IEnumerable<ValueWithAddressIndexKey<TAttribute>>) BuildEnumerator(false)
                    .GetKeyEnumerator();

                IComparable previousKeyValue = null;

                bool first = true;

                foreach (var key in keyEnumerator)
                {
                    var currentKeyValue = (IComparable)key.V;
                    if (first)
                    {
                        yield return (TAttribute)currentKeyValue;
                    }
                    else
                    {
                        if (previousKeyValue == null)
                        {
                            if (currentKeyValue != null)
                            {
                                yield return (TAttribute)currentKeyValue;
                            }
                        }
                        else if (this.Comparer.Compare(previousKeyValue, currentKeyValue) != 0)
                        {
                            yield return (TAttribute)currentKeyValue;
                        }
                    }

                    first = false;
                    previousKeyValue = currentKeyValue;
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

