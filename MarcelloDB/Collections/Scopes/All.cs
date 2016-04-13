using System;
using MarcelloDB.Records;
using System.Collections;
using System.Collections.Generic;

namespace MarcelloDB.Collections.Scopes
{
    public class All<TObj, TAttribute> : BaseScope<TObj, TAttribute>
    {
        IndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        internal All(IndexedValue<TObj, TAttribute> indexedValue)
        {
            this.IndexedValue = indexedValue;

        }

        override internal CollectionEnumerator<TObj, ValueWithAddressIndexKey<TAttribute>> BuildEnumerator(bool descending)
        {
            return this.IndexedValue
                .BuildEnumerator(null, descending);
        }
    }
}

