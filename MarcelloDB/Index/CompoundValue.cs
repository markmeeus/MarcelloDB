using System;
using System.Linq;
using System.Collections.Generic;

namespace MarcelloDB.Index
{
    public abstract partial class CompoundValue : IComparable
    {
        internal abstract IEnumerable<object> GetValues();

        internal CompoundValue(){}

        #region IComparable implementation
        public int CompareTo(object objB)
        {
            var valuesA = GetValues();
            var valuesB = ((CompoundValue)objB).GetValues();
            return CompareValues(valuesA, valuesB);
        }
        #endregion

        int CompareValues(IEnumerable<object> valuesA, IEnumerable<object> valuesB)
        {
            if (valuesA.Count() == 0)
                return 0;
            var firstA = valuesA.First();
            var firstB = valuesB.First();
            var compareResult = CompareValue(firstA, firstB);
            if (compareResult == 0)
            {
                return CompareValues(valuesA.Skip(1), valuesB.Skip(1));
            }
            return compareResult;

        }

        int CompareValue(object valA, object valB)
        {
            if (valA != null && valB == null)
            {
                return 1;
            }
            if (valA == null && valB != null)
            {
                return -1;
            }
            if (valA == null && valB == null)
            {
                return 0;
            }
            return ((IComparable)valA).CompareTo(valB);
        }
    }
}
