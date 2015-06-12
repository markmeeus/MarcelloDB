using System;

namespace MarcelloDB.Records
{
    public class EmptyRecordIndexKey : IComparable
    {
        public Int64 A { get; set; } //Address, abbreviated to save storage
        public Int32 S { get; set; } //Size

        #region IComparable implementation

        public int CompareTo(object obj)
        {
            var other = (EmptyRecordIndexKey)obj;
            var sizeCompared = S.CompareTo(other.S);
            if(sizeCompared == 0)
            {
                return A.CompareTo(other.A);
            }
            return sizeCompared;
        }

        #endregion
    }
}

