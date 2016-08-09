
using System.Runtime.CompilerServices;
using System.Linq;
using System;
using System.Collections.Generic;
using MarcelloDB.Collections;
using MarcelloDB.Index;

namespace MarcelloDB.Collections
{
    
	public class CompoundIndexedValue<TObj, T1, T2> : BaseIndexedValue<TObj, CompoundValue>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<T1, T2>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2));
        }

        
		public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2));
        }
        
    }

	
	public class CompoundIndexedValue<TObj, T1, T2, T3> : BaseIndexedValue<TObj, CompoundValue>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<T1, T2, T3>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3));
        }

        
		public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3));
        }
        
    }

	
	public class CompoundIndexedValue<TObj, T1, T2, T3, T4> : BaseIndexedValue<TObj, CompoundValue>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4));
        }

        
		public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4));
        }
        
    }

	
	public class CompoundIndexedValue<TObj, T1, T2, T3, T4, T5> : BaseIndexedValue<TObj, CompoundValue>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4, T5>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4, T5 val5)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4, val5));
        }

        
		public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5));
        }
        
    }

	
	public class CompoundIndexedValue<TObj, T1, T2, T3, T4, T5, T6> : BaseIndexedValue<TObj, CompoundValue>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4, T5, T6>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4, T5 val5, T6 val6)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4, val5, val6));
        }

        
		public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5, p6));
        }
        
    }

	
	public class CompoundIndexedValue<TObj, T1, T2, T3, T4, T5, T6, T7> : BaseIndexedValue<TObj, CompoundValue>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4, T5 val5, T6 val6, T7 val7)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4, val5, val6, val7));
        }

        
		public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5, p6));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5, p6, p7));
        }
        
    }

	
	public class CompoundIndexedValue<TObj, T1, T2, T3, T4, T5, T6, T7, T8> : BaseIndexedValue<TObj, CompoundValue>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(T1 val1, T2 val2, T3 val3, T4 val4, T5 val5, T6 val6, T7 val7, T8 val8)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4, val5, val6, val7, val8));
        }

        
		public IEnumerable<TObj> GreaterThan(T1 p1)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5, p6));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5, p6, p7));
        }
        
		public IEnumerable<TObj> GreaterThan(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return base.GreaterThanInternal(CompoundValue.Build(p1, p2, p3, p4, p5, p6, p7, p8));
        }
        
    }

	
}