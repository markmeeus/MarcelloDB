using System;
using MarcelloDB.Serialization;

namespace MarcelloDB.Records
{
    internal class RecordHeader
    {
        const int BYTE_SIZE = 3 * sizeof(Int32);

        //Address is not stored
        internal long Address { get; set; }

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

        private RecordHeader(){} //construction only allowed from factory methods

        internal byte[] AsBytes()
        {
            var bytes = new byte[ByteSize];
            var writer = new BufferWriter(bytes);
            writer.WriteInt32(this.DataSize);
            writer.WriteInt32(this.AllocatedDataSize);
            return writer.GetTrimmedBuffer();
        }

        #region factory methods

        internal static RecordHeader New (){ return new RecordHeader();}

        internal static RecordHeader FromBytes(Int64 address, byte[] bytes)
        {   
            var reader = new BufferReader(bytes);
            var recordHeader = new RecordHeader();
            recordHeader.Address = address;
            recordHeader.DataSize = reader.ReadInt32();
            recordHeader.AllocatedDataSize = reader.ReadInt32();
            return recordHeader;
        }

        #endregion
    }
}

