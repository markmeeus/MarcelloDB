using System;

namespace Marcello.Records
{
	internal class CollectionMetaDataRecord
	{
		internal Int64 FirstRecordAddress { get; set; }
        internal Int64 LastRecordAddress { get; set; }

		internal CollectionMetaDataRecord()
		{
		}

        internal static int ByteSize
        {
            get { return 1024; } //some padding for future use 
        }

        internal static CollectionMetaDataRecord FromBytes(byte[] bytes)
        {
                return new CollectionMetaDataRecord(){
                    FirstRecordAddress = BitConverter.ToInt64(bytes, 0),
                    LastRecordAddress = BitConverter.ToInt64(bytes, sizeof(Int64))
                };
        }

        internal byte[] AsBytes()
        {
            var bytes = new byte[ByteSize];
            BitConverter.GetBytes (this.FirstRecordAddress).CopyTo(bytes, 0);
            BitConverter.GetBytes (this.LastRecordAddress).CopyTo(bytes, sizeof(Int64));
            return bytes;
        }
	}
}

