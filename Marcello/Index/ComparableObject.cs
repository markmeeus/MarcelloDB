using System;

namespace Marcello
{
    public class ComparableObject : IComparable<ComparableObject>
    {
        public object Object { get; set;}

        public ComparableObject(object o)
        {
            if (o != null)
            {
                var t = o.GetType();
            }
            this.Object = o;
        }            

        #region IComparable implementation

        public int CompareTo(ComparableObject other)
        {
            if (this.Object is String)
            {
                return ((String)this.Object).CompareTo((String)other.Object);
            }
            return CompareLong(other);
        }

        public int CompareLong(ComparableObject other){
            long thisLong;
            if (this.Object is int)
            {
                thisLong = (long)(int)this.Object;
            }else{
                thisLong = (long)this.Object;
            }

            long otherLong;
            if (other.Object is int)
            {
                otherLong = (long)(int)other.Object;
            }else{
                otherLong = (long)other.Object;
            }

            return (thisLong).CompareTo(otherLong);
        }

        #endregion
    }
}

