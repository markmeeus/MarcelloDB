using System;

namespace Marcello
{
    internal class Record
    {
        internal RecordHeader Header { get; set;}
        internal byte[] data;

        internal Record(){
            Header = new RecordHeader ();
        }

        public byte[] AsBytes()
        {
            var bytes = new byte[RecordHeader.ByteSize + Header.AllocatedSize];
            Header.AsBytes ().CopyTo (bytes, 0);
            data.CopyTo (bytes, RecordHeader.ByteSize);
            return bytes;
        }
    }
}

