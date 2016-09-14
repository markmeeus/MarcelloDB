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

        public IEnumerable<TObj> Contains(TAttribute value)
        {
            return base.FindInternal(new TAttribute[]{value});
        }

        public IEnumerable<TObj> ContainsAny(IEnumerable<TAttribute> values)
        {
            var enumerator = base.FindInternal(values);
            //enumerator will match the same object every time one of the values is in the index.
            //enumeratedAddresses tracks the enumerated addresses and filters the objects.
            var enumeratedAddresses = new HashSet<Int64>();
            enumerator.ShouldYieldObjectWithAddress = (Int64 address) =>
                {
                    if (enumeratedAddresses.Contains(address))
                    {
                        return false;
                    }
                    enumeratedAddresses.Add(address);
                    return true;
                };
            return enumerator;
        }

    }
}
