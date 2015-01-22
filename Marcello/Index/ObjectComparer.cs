using System;

namespace Marcello
{
    public class ObjectComparer
    {
        public ObjectComparer()
        {
        }

        public int Compare(object a, object b)
        {
            try{
                long lngA;
                long lngB;
                if(TryLongify(a, out lngA) && TryLongify(b, out lngB))
                {
                    return lngA.CompareTo(lngB);
                }
                     
                return ((IComparable)a).CompareTo((IComparable)b);

            }catch(Exception e){
                throw;
            }
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

