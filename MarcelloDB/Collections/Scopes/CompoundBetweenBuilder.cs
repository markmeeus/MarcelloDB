
using MarcelloDB.Index;

namespace MarcelloDB.Collections.Scopes
{
    public class BetweenBuilder<TObj, T1, T2>
    {
        BaseIndexedValue<TObj, CompoundValue<T1, T2>> IndexedValue { get; set; }

        CompoundValue<T1, T2> StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue<T1, T2>> indexedValue, CompoundValue<T1, T2> startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

        public Between<TObj, CompoundValue<T1, T2>> And(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2>(p1);
            return new Between<TObj, CompoundValue<T1, T2>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2>> AndIncluding(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2>(p1);
            return new Between<TObj, CompoundValue<T1, T2>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2>> And(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2>(p1, p2);
            return new Between<TObj, CompoundValue<T1, T2>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2>> AndIncluding(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2>(p1);
            return new Between<TObj, CompoundValue<T1, T2>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
    }

    public class BetweenBuilder<TObj, T1, T2, T3>
    {
        BaseIndexedValue<TObj, CompoundValue<T1, T2, T3>> IndexedValue { get; set; }

        CompoundValue<T1, T2, T3> StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue<T1, T2, T3>> indexedValue, CompoundValue<T1, T2, T3> startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

        public Between<TObj, CompoundValue<T1, T2, T3>> And(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3>> AndIncluding(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3>> And(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3>(p1, p2);
            return new Between<TObj, CompoundValue<T1, T2, T3>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3>> AndIncluding(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3>> And(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3>(p1, p2, p3);
            return new Between<TObj, CompoundValue<T1, T2, T3>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3>> AndIncluding(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
    }

    public class BetweenBuilder<TObj, T1, T2, T3, T4>
    {
        BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4>> IndexedValue { get; set; }

        CompoundValue<T1, T2, T3, T4> StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4>> indexedValue, CompoundValue<T1, T2, T3, T4> startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4>> And(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4>> AndIncluding(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4>> And(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4>(p1, p2);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4>> AndIncluding(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4>> And(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4>(p1, p2, p3);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4>> AndIncluding(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4>> And(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4>(p1, p2, p3, p4);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
    }

    public class BetweenBuilder<TObj, T1, T2, T3, T4, T5>
    {
        BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5>> IndexedValue { get; set; }

        CompoundValue<T1, T2, T3, T4, T5> StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5>> indexedValue, CompoundValue<T1, T2, T3, T4, T5> startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> And(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> AndIncluding(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> And(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> AndIncluding(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> And(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> AndIncluding(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> And(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4, p5);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
    }

    public class BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>
    {
        BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> IndexedValue { get; set; }

        CompoundValue<T1, T2, T3, T4, T5, T6> StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> indexedValue, CompoundValue<T1, T2, T3, T4, T5, T6> startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> And(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> AndIncluding(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> And(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> AndIncluding(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> And(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> AndIncluding(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> And(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5, p6);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
    }

    public class BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>
    {
        BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> IndexedValue { get; set; }

        CompoundValue<T1, T2, T3, T4, T5, T6, T7> StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> indexedValue, CompoundValue<T1, T2, T3, T4, T5, T6, T7> startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> And(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> AndIncluding(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> And(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> AndIncluding(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> And(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> AndIncluding(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> And(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6, p7);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
    }

    public class BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>
    {
        BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> IndexedValue { get; set; }

        CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8> StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> indexedValue, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8> startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> And(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> AndIncluding(T1 p1)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> And(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> AndIncluding(T1 p1, T2 p2)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> And(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> AndIncluding(T1 p1, T2 p2, T3 p3)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> And(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7, p8);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
        }

        public Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
          var endAt = new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1);
            return new Between<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
        }
    }



}