using System;
using Marcello.Storage;

namespace Marcello.Records
{
    internal class DirectRecordAccessor
    {
        RecordManager RecordManager {get;set;}

        RecordHeader Header {get;set;}
        internal DirectRecordAccessor(Int64 recordAddress, RecordManager recordManager)
        {
            RecordManager = recordManager;

            Header = RecordManager.ReadRecordHeader(recordAddress);
        }

        internal byte[] Read(Int32 index, Int32 length)
        {
            var absoluteAddress = Header.Address + RecordHeader.ByteSize + index;
            if (absoluteAddress + length > Header.Address + Header.TotalRecordSize)
            {
                throw new IndexOutOfRangeException("Attempted to read outside of record bounds");
            }
             
            return RecordManager.StorageEngine.Read(absoluteAddress, length);
        }

        internal void Write(Int32 index, byte[] data)
        {
            var absoluteAddress = Header.Address + RecordHeader.ByteSize + index;
            if (absoluteAddress + data.Length > Header.Address + Header.TotalRecordSize)
            {
                throw new IndexOutOfRangeException("Attempted to write outside of record bounds");
            }
            RecordManager.StorageEngine.Write(absoluteAddress, data);
        }

    }
}

