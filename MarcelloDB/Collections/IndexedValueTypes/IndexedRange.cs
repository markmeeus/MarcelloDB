using System;
using System.Collections.Generic;
using System.Linq;

namespace MarcelloDB.Collections
{
    public class IndexedRange<TObj, TAttribute> : BaseIndexedValue<TObj, TAttribute>
    {
        internal IndexedRange(Func<TObj, IEnumerable<TAttribute>> valueFunction)
            :base((o)=>valueFunction(o).Distinct())
        {
        }

        public IEnumerable<TObj> Contains(TAttribute value){
            return base.FindInternal(new TAttribute[]{value});
        }

    }
}
