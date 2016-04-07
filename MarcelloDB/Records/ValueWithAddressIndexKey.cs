using System;

namespace MarcelloDB.Records
{
    internal class ValueWithAddressIndexKey : IComparable, IEquatable<ValueWithAddressIndexKey>
    {
        public Int64 A { get; set; } //Address, abbreviated to save storage
        public IComparable V { get; set; } //Size

        #region IComparable implementation

        public int CompareTo(object obj)
        {
            var other = (ValueWithAddressIndexKey)obj;
            var valueCompared = CompareValues(other);
            if(valueCompared == 0)
            {
                if (A > 0 && other.A > 0) //no address is any of them considered equal
                {
                    return  A.CompareTo(other.A);
                }
            }
            return valueCompared;
        }

        #endregion

        #region IEquatable implementation

        public bool Equals(ValueWithAddressIndexKey other)
        {
            return this.CompareTo(other) == 0;
        }

        #endregion

        int CompareValues(ValueWithAddressIndexKey other)
        {
            if (this.V == null)
            {
                if (other.V == null)
                {
                    return 0;
                }
                return -1;
            }
            else if (other.V == null)
            {
                return 1;
            }

            return V.CompareTo(other.V);
        }
    }
}