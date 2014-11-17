using System;

namespace Marcello.Records
{
	internal class CollectionMetaDataRecord
	{
		
        internal Int64 FirstEmptyRecordAddress { get; set; }
        internal Int64 LastEmptyRecordAddress { get; set; }

        internal ListEndPoints DataListEndPoints { get; set;}

		internal CollectionMetaDataRecord()
		{
		}

        internal static int ByteSize
        {
            get { return 1024; } //some padding for future use 
        }

        internal static CollectionMetaDataRecord FromBytes(byte[] bytes)
        {
            var dataListEndPoints = new ListEndPoints(
                BitConverter.ToInt64(bytes, 0),
                BitConverter.ToInt64(bytes, sizeof(Int64))
            );
            return new CollectionMetaDataRecord(){                    
                DataListEndPoints = dataListEndPoints    
            };
        }

        internal byte[] AsBytes()
        {
            var bytes = new byte[ByteSize];
            BitConverter.GetBytes(this.DataListEndPoints.StartAddress).CopyTo(bytes, 0);
            BitConverter.GetBytes(this.DataListEndPoints.EndAddress).CopyTo(bytes, sizeof(Int64));
            return bytes;
        }
	}
}

