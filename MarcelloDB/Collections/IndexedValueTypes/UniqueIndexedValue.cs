using System;
using MarcelloDB.Collections;
using System.Linq;
using System.Collections.Generic;
using MarcelloDB.Records;

namespace MarcelloDB
{
    public class UniqueIndexedValue<TObj, TAttribute> : IndexedValue<TObj, TAttribute>
    {
        UniqueIndexedValue():base(){}

        internal UniqueIndexedValue(Func<TObj, TAttribute> valueFunction)
            :base( valueFunction)
        {
        }

        internal static new UniqueIndexedValue<TObj, TAttribute> Build()
        {
            return new UniqueIndexedValue<TObj, TAttribute>();
        }

        public TObj Find(TAttribute value){

            return base.Equals(value).First();
        }

        internal override IEnumerable<object> GetKeys(object o, long address)
        {
            var value = base.ValueFunction((TObj)o).FirstOrDefault();
            var indexKey = new ValueWithAddressIndexKey<TAttribute>
                {
                    V = value,
                    A = 0 //Makes entries unique
                };
            return new object[]{ indexKey };
        }
    }

}

