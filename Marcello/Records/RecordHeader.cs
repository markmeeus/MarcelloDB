using System;

namespace Marcello.Records
{
    internal class RecordHeader
    {
        const int NEXT_OFFSET = 0;
        const int PREVIOUS_OFFSET = sizeof(Int64);
        const int DATASIZE_OFFSET = PREVIOUS_OFFSET + sizeof(Int64);
        const int ALLOCATEDSIZE_OFFSET = DATASIZE_OFFSET + sizeof(Int32);
        const int BYTE_SIZE = ALLOCATEDSIZE_OFFSET + sizeof(Int32);

        //Address is not stored
        internal long Address { get; set; }

        internal Int64 Next	 { get; set;}
        internal Int64 Previous { get; set;}
        internal Int32 DataSize { get; set; }
        internal Int32 AllocatedSize { get; set; }

        static internal int ByteSize 
        {
            get {
                return BYTE_SIZE;
            }
        }

        private RecordHeader(){} //only construction allow from factory methods

        internal byte[] AsBytes()
        {
            var bytes = new byte[ByteSize];
            BitConverter.GetBytes(this.Next).CopyTo (bytes, NEXT_OFFSET);
            BitConverter.GetBytes(this.Previous).CopyTo (bytes, PREVIOUS_OFFSET);
            BitConverter.GetBytes(this.DataSize).CopyTo (bytes, DATASIZE_OFFSET);
            BitConverter.GetBytes(this.AllocatedSize).CopyTo (bytes, ALLOCATEDSIZE_OFFSET);
            return bytes;
        }

        #region factory methods
        internal static RecordHeader New (){ return new RecordHeader();}

        internal static RecordHeader FromBytes(Int64 address, byte[] bytes)
        {         
            return new RecordHeader() {
                Next = BitConverter.ToInt64(bytes, NEXT_OFFSET),
                Previous = BitConverter.ToInt64(bytes, PREVIOUS_OFFSET),
                DataSize = BitConverter.ToInt32(bytes, DATASIZE_OFFSET),
                AllocatedSize = BitConverter.ToInt32(bytes, ALLOCATEDSIZE_OFFSET),
                Address = address
            };
        }
        #endregion
    }
}

