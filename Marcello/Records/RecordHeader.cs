using System;

namespace Marcello
{
    internal class RecordHeader
    {
        //Address is not stored
        internal long Address { get; set; }

        internal Int64 Next	 { get; set;}
        internal Int64 Previous { get; set;}
        internal Int32 DataSize { get; set; }
        internal Int32 AllocatedSize { get; set; }

        static internal int ByteSize {
            get {
                return sizeof(Int32) + ( 3 * sizeof(Int64));
            }
        }

        private RecordHeader(){} //only allow from factory methods

        internal byte[] AsBytes(){
            var bytes = new byte[ByteSize];
            BitConverter.GetBytes(this.Next).CopyTo (bytes, 0);
            BitConverter.GetBytes(this.Previous).CopyTo (bytes, sizeof(Int64));
            BitConverter.GetBytes(this.DataSize).CopyTo (bytes, 2 * sizeof(Int64));
            BitConverter.GetBytes(this.AllocatedSize).CopyTo (bytes, (sizeof(Int32) + 2 * sizeof(Int64)));
            return bytes;
        }

        #region factory methods
        internal static RecordHeader New (){ return new RecordHeader();}

        internal static RecordHeader FromBytes(Int64 address, byte[] bytes){
            return new RecordHeader() {
                Next = BitConverter.ToInt64(bytes, 0),
                Previous = BitConverter.ToInt64(bytes, sizeof(Int64)),
                DataSize = BitConverter.ToInt32(bytes, 2 * sizeof(Int64)),
                AllocatedSize = BitConverter.ToInt32(bytes, (sizeof(Int32) + 2 * sizeof(long))),
                Address = address
            };
        }
        #endregion
    }
}

