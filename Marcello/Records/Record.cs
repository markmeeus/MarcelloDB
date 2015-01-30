using System;
using System.Diagnostics;

namespace Marcello.Records
{
    internal class Record
    {
        internal RecordHeader Header { get; set;}
        internal byte[] Data;

        internal Record()
        {
            Header = RecordHeader.New();
        }

        internal Int32 ByteSize
        {
            get
            {
                return RecordHeader.ByteSize + Header.AllocatedDataSize;
            }
        }

        internal byte[] AsBytes()
        {
            var bytes = new byte[this.ByteSize];

            Header.AsBytes().CopyTo(bytes, 0);
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

