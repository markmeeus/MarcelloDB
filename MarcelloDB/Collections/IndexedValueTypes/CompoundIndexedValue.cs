
using System.Runtime.CompilerServices;
using System.Linq;
using System;
using System.Collections.Generic;
using MarcelloDB.Collections;
using MarcelloDB.Collections.Scopes;
using MarcelloDB.Index;

namespace MarcelloDB.Collections
{
    public class IndexedValue<TObj, T1, T2> : BaseIndexedValue<TObj, CompoundValue<T1, T2>>
    {
        IndexedValue(): base(null){}

        internal IndexedValue(Func<TObj, CompoundValue<T1, T2>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2)
        {
            return base.FindInternal(new CompoundValue<T1, T2>(val1, val2));
        }

        public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2>(p1));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2>(p1));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2>(p1));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2>(p1));
        }

        public BetweenBuilder<TObj, T1, T2> Between(T1 p1)
        {
            var start = new CompoundValue<T1, T2>(p1);
            return new BetweenBuilder<TObj, T1, T2>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2> BetweenIncluding(T1 p1)
        {
            var start = new CompoundValue<T1, T2>(p1);
            return new BetweenBuilder<TObj, T1, T2>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2>(p1, p2));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2>(p1, p2));
        }

        public BetweenBuilder<TObj, T1, T2> Between(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2> BetweenIncluding(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2>(this, start, true);
        }

    }

    public class IndexedValue<TObj, T1, T2, T3> : BaseIndexedValue<TObj, CompoundValue<T1, T2, T3>>
    {
        IndexedValue(): base(null){}

        internal IndexedValue(Func<TObj, CompoundValue<T1, T2, T3>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3)
        {
            return base.FindInternal(new CompoundValue<T1, T2, T3>(val1, val2, val3));
        }

        public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3>(p1));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3>(p1));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3>(p1));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3>(p1));
        }

        public BetweenBuilder<TObj, T1, T2, T3> Between(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3> BetweenIncluding(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3>(p1, p2));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3>(p1, p2));
        }

        public BetweenBuilder<TObj, T1, T2, T3> Between(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3> BetweenIncluding(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3>(p1, p2, p3));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3>(p1, p2, p3));
        }

        public BetweenBuilder<TObj, T1, T2, T3> Between(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3> BetweenIncluding(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3>(this, start, true);
        }

    }

    public class IndexedValue<TObj, T1, T2, T3, T4> : BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4>>
    {
        IndexedValue(): base(null){}

        internal IndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4)
        {
            return base.FindInternal(new CompoundValue<T1, T2, T3, T4>(val1, val2, val3, val4));
        }

        public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4>(p1));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4>(p1));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4>(p1));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4>(p1));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4> Between(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4> BetweenIncluding(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4> Between(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4> BetweenIncluding(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2, p3));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2, p3));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4> Between(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4> BetweenIncluding(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4>(p1, p2, p3, p4));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4> Between(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4>(this, start, true);
        }

    }

    public class IndexedValue<TObj, T1, T2, T3, T4, T5> : BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5>>
    {
        IndexedValue(): base(null){}

        internal IndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4, T5>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4, T5 val5)
        {
            return base.FindInternal(new CompoundValue<T1, T2, T3, T4, T5>(val1, val2, val3, val4, val5));
        }

        public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> Between(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> BetweenIncluding(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> Between(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> BetweenIncluding(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> Between(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> BetweenIncluding(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> Between(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4, p5));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4, p5);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4, p5);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5>(this, start, true);
        }

    }

    public class IndexedValue<TObj, T1, T2, T3, T4, T5, T6> : BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>
    {
        IndexedValue(): base(null){}

        internal IndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4, T5 val5, T6 val6)
        {
            return base.FindInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(val1, val2, val3, val4, val5, val6));
        }

        public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> Between(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> BetweenIncluding(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> Between(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> BetweenIncluding(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> Between(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> BetweenIncluding(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> Between(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5, p6));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5, p6));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5, p6));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5, p6));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5, p6);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5, p6);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>(this, start, true);
        }

    }

    public class IndexedValue<TObj, T1, T2, T3, T4, T5, T6, T7> : BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>
    {
        IndexedValue(): base(null){}

        internal IndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4, T5 val5, T6 val6, T7 val7)
        {
            return base.FindInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(val1, val2, val3, val4, val5, val6, val7));
        }

        public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> Between(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> BetweenIncluding(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> Between(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> BetweenIncluding(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> Between(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> BetweenIncluding(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> Between(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6, p7));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6, p7));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6, p7));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6, p7));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6, p7);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6, p7);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>(this, start, true);
        }

    }

    public class IndexedValue<TObj, T1, T2, T3, T4, T5, T6, T7, T8> : BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>
    {
        IndexedValue(): base(null){}

        internal IndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4, T5 val5, T6 val6, T7 val7, T8 val8)
        {
            return base.FindInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(val1, val2, val3, val4, val5, val6, val7, val8));
        }

        public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> Between(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> BetweenIncluding(T1 p1)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> Between(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> BetweenIncluding(T1 p1, T2 p2)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> Between(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> BetweenIncluding(T1 p1, T2 p2, T3 p3)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> Between(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, true);
        }

        public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return base.GreaterThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7, p8));
        }

        public IEnumerable<TObj> GreaterThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return base.GreaterThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7, p8));
        }

        public IEnumerable<TObj> SmallerThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return base.SmallerThanInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7, p8));
        }

        public IEnumerable<TObj> SmallerThanOrEqual(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return base.SmallerThanOrEqualInternal(new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7, p8));
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> Between(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7, p8);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, false);
        }

        public BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8> BetweenIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            var start = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7, p8);
            return new BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>(this, start, true);
        }

    }

}