using System;

namespace MarcelloDB.Index
{
    public class ObjectComparer
    {
        bool Inverted { get; set; }

        public ObjectComparer()
        {
        }

        public int Compare(object a, object b)
        {
            long lngA;
            long lngB;

            if(TryLongify(a, out lngA) && TryLongify(b, out lngB))
            {
                return InvertIfNecessary(
                    lngA.CompareTo(lngB)
                );
            }
            return InvertIfNecessary(
                ((IComparable)a).CompareTo((IComparable)b)
            );
        }

        public void Invert()
        {
            this.Inverted = true;
        }

        int InvertIfNecessary(int compareValue)
        {
            if (this.Inverted)
            {
                if (compareValue < 0)
                    return 1;
                if (compareValue > 0)
                    return -1;
            }
            return compareValue;
        }
        bool TryLongify(object o, out long result)
        {
            if (o is short)
            {
                result = (long)(short)o;
                return true;
            }
            if (o is int)
            {
                result = (long)(int)o;
                return true;
            }
            if (o is long)
            {
                result = (long)o;
                return true;
            }

            result = 0;
            return false;
        }
    }
}

