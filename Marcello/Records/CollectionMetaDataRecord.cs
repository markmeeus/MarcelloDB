using System;

namespace Marcello.Records
{
	internal class CollectionMetaDataRecord
	{		
        internal ListEndPoints DataListEndPoints { get; set;}
        internal ListEndPoints EmptyListEndPoints { get; set;}

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
            var emptyListEndPoints = new ListEndPoints(
                BitConverter.ToInt64(bytes, 2*sizeof(Int64)),
                BitConverter.ToInt64(bytes, 3*sizeof(Int64))
            );
            return new CollectionMetaDataRecord(){                    
                DataListEndPoints = dataListEndPoints,
                EmptyListEndPoints = emptyListEndPoints
            };

        }

        internal byte[] AsBytes()
        {
            var bytes = new byte[ByteSize];
            BitConverter.GetBytes(this.DataListEndPoints.StartAddress).CopyTo(bytes, 0);
            BitConverter.GetBytes(this.DataListEndPoints.EndAddress).CopyTo(bytes, sizeof(Int64));
            BitConverter.GetBytes(this.EmptyListEndPoints.StartAddress).CopyTo(bytes, 2*sizeof(Int64));
            BitConverter.GetBytes(this.EmptyListEndPoints.EndAddress).CopyTo(bytes, 3*sizeof(Int64));

            return bytes;
        }

        internal void Sanitize(){
            //when the data list is empty, the empty list is empty too.
            if (this.DataListEndPoints.StartAddress == 0) 
            {
                this.EmptyListEndPoints.StartAddress = 0;
                this.EmptyListEndPoints.EndAddress = 0;
            }
        }
	}
}

