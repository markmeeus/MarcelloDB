using System;
using Marcello.Serialization;

namespace Marcello.Records
{
    internal class RecordHeader
    {
        const int BYTE_SIZE = (2 * sizeof(Int64)) + (2 * sizeof(Int32));

        //Address is not stored
        internal long Address { get; set; }

        internal Int64 Next	 { get; set;}
        internal Int64 Previous { get; set;}
        internal Int32 DataSize { get; set; }
        internal Int32 AllocatedDataSize { get; set; }

        internal Int32 TotalRecordSize 
        { 
            get{ 
                return ByteSize + AllocatedDataSize;    
            }
        }

        static internal int ByteSize 
        {
            get {
                return BYTE_SIZE;
            }
        }

        internal RecordHeader(){}

        internal byte[] AsBytes()
        {
            var bytes = new byte[ByteSize];
            var writer = new BufferWriter(bytes, BitConverter.IsLittleEndian);
            writer.WriteInt64(this.Next);
            writer.WriteInt64(this.Previous);
            writer.WriteInt32(this.DataSize);
            writer.WriteInt32(this.AllocatedDataSize);
            return writer.GetTrimmedBuffer();
        }

        #region factory methods

        internal static RecordHeader FromBytes(Int64 address, byte[] bytes)
        {   

            var reader = new BufferReader(bytes, BitConverter.IsLittleEndian);
            var recordHeader = new RecordHeader();
            recordHeader.Address = address;

            recordHeader.Next = reader.ReadInt64();
            recordHeader.Previous = reader.ReadInt64();
            recordHeader.DataSize = reader.ReadInt32();
            recordHeader.AllocatedDataSize = reader.ReadInt32();
            return recordHeader;
        }
        #endregion
    }
}

