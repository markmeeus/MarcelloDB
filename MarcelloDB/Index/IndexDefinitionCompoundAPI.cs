
using MarcelloDB.Collections;
using System.Runtime.CompilerServices;
using System.Linq;
using System;

namespace MarcelloDB.Index
{
    public partial class IndexDefinition<T>
    {
        protected IndexedValue<T, TAtt1, TAtt2> IndexedValue<TAtt1, TAtt2>
            (Func<T, CompoundValue<TAtt1, TAtt2>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new IndexedValue<T, TAtt1, TAtt2>(valueFunc);
            }
            else
            {
                return (IndexedValue<T, TAtt1, TAtt2>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }
	    protected CompoundValue<TAtt1, TAtt2> CompoundValue<TAtt1, TAtt2>(TAtt1 p1,TAtt2 p2)
	    {
	    	return new CompoundValue<TAtt1, TAtt2>(p1,p2);
	    }
        protected IndexedValue<T, TAtt1, TAtt2, TAtt3> IndexedValue<TAtt1, TAtt2, TAtt3>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new IndexedValue<T, TAtt1, TAtt2, TAtt3>(valueFunc);
            }
            else
            {
                return (IndexedValue<T, TAtt1, TAtt2, TAtt3>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }
	    protected CompoundValue<TAtt1, TAtt2, TAtt3> CompoundValue<TAtt1, TAtt2, TAtt3>(TAtt1 p1,TAtt2 p2,TAtt3 p3)
	    {
	    	return new CompoundValue<TAtt1, TAtt2, TAtt3>(p1,p2,p3);
	    }
        protected IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4> IndexedValue<TAtt1, TAtt2, TAtt3, TAtt4>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4>(valueFunc);
            }
            else
            {
                return (IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }
	    protected CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4> CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4>(TAtt1 p1,TAtt2 p2,TAtt3 p3,TAtt4 p4)
	    {
	    	return new CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4>(p1,p2,p3,p4);
	    }
        protected IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5> IndexedValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>(valueFunc);
            }
            else
            {
                return (IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }
	    protected CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5> CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>(TAtt1 p1,TAtt2 p2,TAtt3 p3,TAtt4 p4,TAtt5 p5)
	    {
	    	return new CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>(p1,p2,p3,p4,p5);
	    }
        protected IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6> IndexedValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>(valueFunc);
            }
            else
            {
                return (IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }
	    protected CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6> CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>(TAtt1 p1,TAtt2 p2,TAtt3 p3,TAtt4 p4,TAtt5 p5,TAtt6 p6)
	    {
	    	return new CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>(p1,p2,p3,p4,p5,p6);
	    }
        protected IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7> IndexedValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>(valueFunc);
            }
            else
            {
                return (IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }
	    protected CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7> CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>(TAtt1 p1,TAtt2 p2,TAtt3 p3,TAtt4 p4,TAtt5 p5,TAtt6 p6,TAtt7 p7)
	    {
	    	return new CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>(p1,p2,p3,p4,p5,p6,p7);
	    }
        protected IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8> IndexedValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>(valueFunc);
            }
            else
            {
                return (IndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }
	    protected CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8> CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>(TAtt1 p1,TAtt2 p2,TAtt3 p3,TAtt4 p4,TAtt5 p5,TAtt6 p6,TAtt7 p7,TAtt8 p8)
	    {
	    	return new CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>(p1,p2,p3,p4,p5,p6,p7,p8);
	    }
    }
}