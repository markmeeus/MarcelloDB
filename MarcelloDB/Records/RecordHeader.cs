using System;
using MarcelloDB.Serialization;
using MarcelloDB.Buffers;

namespace MarcelloDB.Records
{
    internal class RecordHeader
    {
        const int BYTE_SIZE = 3 * sizeof(Int32);

        //Address is not stored
        internal long Address { get; set; }

        internal Int32 DataSize { get; set; }
        internal Int32 AllocatedDataSize { get; set; }
        internal bool HasObject { get; set; }

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

        internal ByteBuffer AsBuffer(Marcello session)
        {
            var buffer = session.ByteBufferManager.Create(ByteSize);
            var writer = new BufferWriter(session, buffer, BitConverter.IsLittleEndian);
            writer.WriteInt32(this.DataSize);
            writer.WriteInt32(this.AllocatedDataSize);
            writer.WriteInt32(this.HasObject ? 1 : 0);
            return writer.GetTrimmedBuffer();
        }

        #region factory methods

        internal static RecordHeader New (){ return new RecordHeader();}

        internal static RecordHeader FromBuffer(Marcello session, Int64 address, ByteBuffer buffer)
        {   

            var reader = new BufferReader(session, buffer, BitConverter.IsLittleEndian);
            var recordHeader = new RecordHeader();
            recordHeader.Address = address;
            recordHeader.DataSize = reader.ReadInt32();
            recordHeader.AllocatedDataSize = reader.ReadInt32();
            recordHeader.HasObject = reader.ReadInt32() > 0 ? true : false;
            return recordHeader;
        }

        #endregion
    }
}

