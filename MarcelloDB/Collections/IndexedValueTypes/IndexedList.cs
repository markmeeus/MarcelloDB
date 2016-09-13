using System;
using System.Collections.Generic;
using System.Linq;

namespace MarcelloDB.Collections
{
    public class IndexedList<TObj, TAttribute> : BaseIndexedValue<TObj, TAttribute>
    {
        internal IndexedList(Func<TObj, IEnumerable<TAttribute>> valueFunction)
            :base((o)=>valueFunction(o).Distinct())
        {
        }

        public IEnumerable<TObj> ContainsAny(TAttribute value){
            return base.FindInternal(new TAttribute[]{value});
        }

        public IEnumerable<TObj> ContainsAnyOf(IEnumerable<TAttribute> values){
            return base.FindInternal(values);
        }

    }
}
