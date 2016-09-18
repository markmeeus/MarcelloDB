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
            return base.EqualsInternal(new TAttribute[]{value});
        }

        public IEnumerable<TObj> ContainsAny(IEnumerable<TAttribute> values)
        {
            var enumerator = base.EqualsInternal(values);
            //enumerator will match the same object every time one of the values is in the index.
            //enumeratedAddresses tracks the enumerated addresses and filters the objects.
            enumerator.ShouldYieldObjectWithAddress = base.CreateDuplicateAddressFilter();
            return enumerator;
        }

        public IEnumerable<TObj> ContainsAll(IEnumerable<TAttribute> values)
        {
            var enumerator = base.EqualsInternal(values);

            //enumerator will match the same object every time one of the values is in the index.
            //enumeratedAddresses tracks the enumerated addresses and filters the objects.
            enumerator.ShouldYieldObjectWithAddress = base.CreateDuplicateAddressFilter();

            //enumerator will match any object with any value from the values parameter.
            enumerator.ShouldYieldObject = (o) =>
                {
                    var objectValues = ValueFunction(o).ToDictionary((obj)=>obj);
                    return values.All((v)=> objectValues.ContainsKey(v));
                };
            return enumerator;
        }
    }
}
