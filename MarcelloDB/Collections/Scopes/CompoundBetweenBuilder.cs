
using MarcelloDB.Index;

namespace MarcelloDB.Collections.Scopes
{
    
	public class BetweenBuilder<TObj, T1, T2>
    {
        BaseIndexedValue<TObj, CompoundValue> IndexedValue { get; set; }

        CompoundValue StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue> indexedValue, CompoundValue startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

                
		public Between<TObj, CompoundValue> And(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1, p2);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
    }



	
	public class BetweenBuilder<TObj, T1, T2, T3>
    {
        BaseIndexedValue<TObj, CompoundValue> IndexedValue { get; set; }

        CompoundValue StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue> indexedValue, CompoundValue startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

                
		public Between<TObj, CompoundValue> And(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1, p2);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1, p2, p3);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
    }



	
	public class BetweenBuilder<TObj, T1, T2, T3, T4>
    {
        BaseIndexedValue<TObj, CompoundValue> IndexedValue { get; set; }

        CompoundValue StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue> indexedValue, CompoundValue startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

                
		public Between<TObj, CompoundValue> And(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1, p2);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1, p2, p3);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
    }



	
	public class BetweenBuilder<TObj, T1, T2, T3, T4, T5>
    {
        BaseIndexedValue<TObj, CompoundValue> IndexedValue { get; set; }

        CompoundValue StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue> indexedValue, CompoundValue startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

                
		public Between<TObj, CompoundValue> And(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1, p2);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1, p2, p3);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
    }



	
	public class BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6>
    {
        BaseIndexedValue<TObj, CompoundValue> IndexedValue { get; set; }

        CompoundValue StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue> indexedValue, CompoundValue startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

                
		public Between<TObj, CompoundValue> And(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1, p2);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1, p2, p3);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5, p6);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
    }



	
	public class BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7>
    {
        BaseIndexedValue<TObj, CompoundValue> IndexedValue { get; set; }

        CompoundValue StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue> indexedValue, CompoundValue startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

                
		public Between<TObj, CompoundValue> And(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1, p2);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1, p2, p3);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5, p6);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5, p6, p7);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
    }



	
	public class BetweenBuilder<TObj, T1, T2, T3, T4, T5, T6, T7, T8>
    {
        BaseIndexedValue<TObj, CompoundValue> IndexedValue { get; set; }

        CompoundValue StartAt { get; set; }

        bool IncludeStartAt { get; set; }

        internal BetweenBuilder(BaseIndexedValue<TObj, CompoundValue> indexedValue, CompoundValue startAt, bool includeStartAt)
        {
            this.IndexedValue = indexedValue;
            this.StartAt = startAt;
            this.IncludeStartAt = includeStartAt;
        }

                
		public Between<TObj, CompoundValue> And(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1, p2);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1, p2, p3);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5, p6);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5, p6, p7);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
		public Between<TObj, CompoundValue> And(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
		{
			var endAt = CompoundValue.Build(p1, p2, p3, p4, p5, p6, p7, p8);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, false);
		}

		public Between<TObj, CompoundValue> AndIncluding(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
		{
			var endAt = CompoundValue.Build(p1);
		    return new Between<TObj, CompoundValue>(this.IndexedValue, this.StartAt, this.IncludeStartAt, endAt, true);
		}
        
    }



	
}