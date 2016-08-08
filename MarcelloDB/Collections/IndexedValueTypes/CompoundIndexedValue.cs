
using System.Runtime.CompilerServices;
using System.Linq;
using System;
using System.Collections.Generic;
using MarcelloDB.Collections;
using MarcelloDB.Index;

namespace MarcelloDB.Collections
{    
    
	public class CompoundIndexedValue<TObj, TAtt1, TAtt2> : BaseIndexedValue<TObj, CompoundValue<TAtt1, TAtt2>>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<TAtt1, TAtt2>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(TAtt1 val1, TAtt2 val2)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2));
        }

        public IEnumerable<TObj> GreaterThan(TAtt1 val1, TAtt2 val2)
        {
            return base.GreaterThanInternal(CompoundValue.Build(val1, val2));
        }
    }

	
	public class CompoundIndexedValue<TObj, TAtt1, TAtt2, TAtt3> : BaseIndexedValue<TObj, CompoundValue<TAtt1, TAtt2, TAtt3>>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<TAtt1, TAtt2, TAtt3>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(TAtt1 val1, TAtt2 val2, TAtt3 val3)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3));
        }

        public IEnumerable<TObj> GreaterThan(TAtt1 val1, TAtt2 val2, TAtt3 val3)
        {
            return base.GreaterThanInternal(CompoundValue.Build(val1, val2, val3));
        }
    }

	
	public class CompoundIndexedValue<TObj, TAtt1, TAtt2, TAtt3, TAtt4> : BaseIndexedValue<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4>>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4));
        }

        public IEnumerable<TObj> GreaterThan(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4)
        {
            return base.GreaterThanInternal(CompoundValue.Build(val1, val2, val3, val4));
        }
    }

	
	public class CompoundIndexedValue<TObj, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5> : BaseIndexedValue<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4, TAtt5 val5)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4, val5));
        }

        public IEnumerable<TObj> GreaterThan(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4, TAtt5 val5)
        {
            return base.GreaterThanInternal(CompoundValue.Build(val1, val2, val3, val4, val5));
        }
    }

	
	public class CompoundIndexedValue<TObj, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6> : BaseIndexedValue<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4, TAtt5 val5, TAtt6 val6)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4, val5, val6));
        }

        public IEnumerable<TObj> GreaterThan(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4, TAtt5 val5, TAtt6 val6)
        {
            return base.GreaterThanInternal(CompoundValue.Build(val1, val2, val3, val4, val5, val6));
        }
    }

	
	public class CompoundIndexedValue<TObj, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7> : BaseIndexedValue<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4, TAtt5 val5, TAtt6 val6, TAtt7 val7)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4, val5, val6, val7));
        }

        public IEnumerable<TObj> GreaterThan(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4, TAtt5 val5, TAtt6 val6, TAtt7 val7)
        {
            return base.GreaterThanInternal(CompoundValue.Build(val1, val2, val3, val4, val5, val6, val7));
        }
    }

	
	public class CompoundIndexedValue<TObj, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8> : BaseIndexedValue<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>>
    {
        CompoundIndexedValue(): base(null){
        }

        internal CompoundIndexedValue(Func<TObj, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>> valueFunction)
            :base(valueFunction){}

        public IEnumerable<TObj> Find(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4, TAtt5 val5, TAtt6 val6, TAtt7 val7, TAtt8 val8)
        {
            return base.FindInternal(CompoundValue.Build(val1, val2, val3, val4, val5, val6, val7, val8));
        }

        public IEnumerable<TObj> GreaterThan(TAtt1 val1, TAtt2 val2, TAtt3 val3, TAtt4 val4, TAtt5 val5, TAtt6 val6, TAtt7 val7, TAtt8 val8)
        {
            return base.GreaterThanInternal(CompoundValue.Build(val1, val2, val3, val4, val5, val6, val7, val8));
        }
    }

	
}