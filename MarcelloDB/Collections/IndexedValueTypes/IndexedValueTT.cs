using System;
using System.Collections.Generic;

using MarcelloDB.Collections.Scopes;
using MarcelloDB.Index.BTree;
using MarcelloDB.Records;

namespace MarcelloDB.Collections
{
    public class IndexedValue<TObj, TAttribute> : BaseIndexedValue<TObj, TAttribute>
    {

        internal IndexedValue(Func<TObj, TAttribute> valueFunction):base(valueFunction)
        {
        }

        IndexedValue():base(null)
        {
        }

        internal static IndexedValue<TObj, TAttribute> Build()
        {
            return new IndexedValue<TObj, TAttribute>();
        }

        public IEnumerable<TObj> Find(TAttribute value)
        {
            return base.FindInternal(value);
        }

        public All<TObj, TAttribute> All
        {
            get
            {
                return new All<TObj, TAttribute>(this);
            }
        }

        public BetweenBuilder<TObj, TAttribute> Between(TAttribute startValue)
        {
            return new BetweenBuilder<TObj, TAttribute>(this, startValue, false);
        }

        public BetweenBuilder<TObj, TAttribute> BetweenIncluding(TAttribute startValue)
        {
            return new BetweenBuilder<TObj, TAttribute>(this, startValue, true);
        }

        public GreaterThan<TObj, TAttribute> GreaterThan(TAttribute value)
        {
            return new GreaterThan<TObj, TAttribute>(this, value, false);
        }

        public GreaterThan<TObj, TAttribute> GreaterThanOrEqual(TAttribute value)
        {
            return new GreaterThan<TObj, TAttribute>(this, value, true);
        }

        public SmallerThan<TObj, TAttribute> SmallerThan(TAttribute value)
        {
            return new SmallerThan<TObj, TAttribute>(this, value, false);
        }

        public SmallerThan<TObj, TAttribute> SmallerThanOrEqual(TAttribute value)
        {
            return new SmallerThan<TObj, TAttribute>(this, value, true);
        }
    }
}

