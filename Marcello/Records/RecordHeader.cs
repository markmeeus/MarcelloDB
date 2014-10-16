using System;

namespace Marcello
{
    internal class RecordHeader
    {
        //Address is not stored
        internal long Address { get; set; }

        internal long Next	 { get; set;}
        internal long Previous { get; set;}
        internal long AllocatedSize { get; set; }

        static internal int ByteSize {
            get {
                return 3 * sizeof(long);
            }
        }
       
        internal byte[] AsBytes(){
            var bytes = new byte[ByteSize];
            BitConverter.GetBytes (this.Next).CopyTo (bytes, 0);
            BitConverter.GetBytes (this.Previous).CopyTo (bytes, sizeof(long));
            BitConverter.GetBytes (this.AllocatedSize).CopyTo (bytes, 2 * sizeof(long));
            return bytes;
        }
    }
}

