
using MarcelloDB.Collections;
using System.Runtime.CompilerServices;
using System.Linq;
using System;

namespace MarcelloDB.Index
{
    public partial class IndexDefinition<T>
    {
        protected CompoundIndexedValue<T, TAtt1, TAtt2> CompoundIndexedValue<TAtt1, TAtt2>
            (Func<T, CompoundValue<TAtt1, TAtt2>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new CompoundIndexedValue<T, TAtt1, TAtt2>(valueFunc);
            }
            else
            {
                return (CompoundIndexedValue<T, TAtt1, TAtt2>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }

        protected CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3> CompoundIndexedValue<TAtt1, TAtt2, TAtt3>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3>(valueFunc);
            }
            else
            {
                return (CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }

        protected CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4> CompoundIndexedValue<TAtt1, TAtt2, TAtt3, TAtt4>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4>(valueFunc);
            }
            else
            {
                return (CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }

        protected CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5> CompoundIndexedValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>(valueFunc);
            }
            else
            {
                return (CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }

        protected CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6> CompoundIndexedValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>(valueFunc);
            }
            else
            {
                return (CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }

        protected CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7> CompoundIndexedValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>(valueFunc);
            }
            else
            {
                return (CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }

        protected CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8> CompoundIndexedValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>
            (Func<T, CompoundValue<TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>(valueFunc);
            }
            else
            {
                return (CompoundIndexedValue<T, TAtt1, TAtt2, TAtt3, TAtt4, TAtt5, TAtt6, TAtt7, TAtt8>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }

    }
}