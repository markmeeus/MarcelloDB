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
                    if (_propValueFunction == null)
                    {
                        var getMethod = typeof(TObj).GetRuntimeProperty(this.PropertyName).GetMethod;
                        _propValueFunction = (TObj o) =>
                        {
                            var value = (TAttribute)getMethod.Invoke(o, new object[0]);
                            return new TAttribute[]{ value };
                        };
                    }
                    return _propValueFunction;
                }
            }
        }


        internal static IndexedValue<TObj, TAttribute> Build()
        {
            return new IndexedValue<TObj, TAttribute>();
        }

        public IEnumerable<TObj> Find(TAttribute value)
        {
            return base.FindInternal(new TAttribute[]{ value });
        }

        public IEnumerable<TObj> Find(IEnumerable<TAttribute> values)
        {
            return base.FindInternal(values);
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

