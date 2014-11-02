using System;

namespace Marcello
{
    internal class Record
    {
        internal RecordHeader Header { get; set;}
        internal byte[] Data;

        internal Record()
        {
            Header = RecordHeader.New();
        }

        internal byte[] AsBytes()
        {
            var bytes = new byte[RecordHeader.ByteSize + Header.AllocatedSize];
            Header.AsBytes ().CopyTo(bytes, 0);
            Data.CopyTo(bytes, RecordHeader.ByteSize);
            return bytes;
        }

        internal static Record FromBytes(Int64 address, byte[] bytes)
        {
            var header = RecordHeader.FromBytes(address, bytes);
            var data = new byte[header.DataSize];
        
            Array.Copy(bytes, RecordHeader.ByteSize, data, 0, header.DataSize);                
                
            return new Record(){
                Header = header,
                Data = data
            };
        }
    }
}

