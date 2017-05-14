using System;

namespace MarcelloDB.Index
{
    internal class ObjectComparer
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

            if(a is DateTime) {
                return CompareDateTimes((DateTime)a, (DateTime)b);
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

        int CompareDateTimes(DateTime a, DateTime b)
        {
            var aToSecondPrecission  = a.AddTicks(-a.Ticks % (TimeSpan.TicksPerSecond / 1000));
            var bToSecondPrescission = b.AddTicks(-b.Ticks % (TimeSpan.TicksPerSecond / 1000));
            return aToSecondPrecission.CompareTo(bToSecondPrescission);
        }
    }
}

