using System;
using MarcelloDB.Records;
using System.Collections;
using System.Collections.Generic;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Collections.Scopes
{
    public class All<TObj, TAttribute> : BaseScope<TObj, TAttribute>
    {
        BaseIndexedValue<TObj, TAttribute> IndexedValue { get; set; }

        internal All(BaseIndexedValue<TObj, TAttribute> indexedValue)
        {
            this.IndexedValue = indexedValue;

        }

        override internal CollectionEnumerator<TObj, ValueWithAddressIndexKey<TAttribute>> BuildEnumerator(bool descending)
        {
            return this.IndexedValue
                .BuildEnumerator(new BTreeWalkerRange<ValueWithAddressIndexKey<TAttribute>>[]{null}, descending);
        }
    }
}

