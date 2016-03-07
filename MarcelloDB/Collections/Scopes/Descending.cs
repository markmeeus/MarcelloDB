using System;
using MarcelloDB.Records;
using MarcelloDB.Collections;

namespace MarcelloDB
{
    public class Descending<TObj, TAttribute> : BaseScope<TObj, TAttribute>
    {
        BaseScope <TObj, TAttribute> OriginalScope { get; set; }

        internal Descending(BaseScope <TObj, TAttribute> originalScope)
        {
            this.OriginalScope = originalScope;
        }

        internal override CollectionEnumerator<TObj, ValueWithAddressIndexKey> BuildEnumerator(bool descending)
        {
            return OriginalScope.BuildEnumerator(true); //force descending enumerator
        }
    }
}

