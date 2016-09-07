using System;
using System.Collections.Generic;

using MarcelloDB.Collections.Scopes;
using MarcelloDB.Index.BTree;
using MarcelloDB.Records;
using System.Reflection;

namespace MarcelloDB.Collections
{
    public class IndexedValue<TObj, TAttribute> : BaseIndexedValue<TObj, TAttribute>
    {
        Func<TObj, IEnumerable<TAttribute>> _propValueFunction;

        internal IndexedValue(Func<TObj, TAttribute> valueFunction)
            :base( (o)=>new TAttribute[]{valueFunction(o)} )
        {
        }

        IndexedValue():base(null)
        {
        }

        internal override Func<TObj, IEnumerable<TAttribute>> ValueFunction
        {
            get
            {
                if (base.UserValueFunction != null)
                {
                    return UserValueFunction;
                }
                else
                {
                    return _propValueFunction = _propValueFunction ?? (_propValueFunction = ((TObj o) => {
                        var value = (TAttribute)(typeof(TObj).GetRuntimeProperty(this.PropertyName)
                            .GetMethod.Invoke(o, new object[0]));
                        return new TAttribute[]{value};
                    }));
                }
            }
        }


        internal static IndexedValue<TObj, TAttribute> Build()
        {
            return new IndexedValue<TObj, TAttribute>();
        }

        public IEnumerable<TObj> Find(TAttribute value)
        {
            return base.FindInternal(value);
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
            return base.GreaterThanInternal(value);
        }

        public GreaterThan<TObj, TAttribute> GreaterThanOrEqual(TAttribute value)
        {
            return base.GreaterThanOrEqualInternal(value);
        }

        public SmallerThan<TObj, TAttribute> SmallerThan(TAttribute value)
        {
            return base.SmallerThanInternal(value);
        }

        public SmallerThan<TObj, TAttribute> SmallerThanOrEqual(TAttribute value)
        {
            return base.SmallerThanOrEqualInternal(value);
        }
    }
}

