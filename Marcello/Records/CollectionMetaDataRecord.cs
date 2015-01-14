using System;
using Marcello.Serialization;

namespace Marcello.Records
{
	internal class CollectionMetaDataRecord
	{		
        internal ListEndPoints DataListEndPoints { get; set;}
        internal ListEndPoints EmptyListEndPoints { get; set;}
        internal Int64 NamedRecordIndexAddress { get; set;}

		internal CollectionMetaDataRecord()
		{
		}

        internal static int ByteSize
        {
            get { return 1024; } //some padding for future use 
        }

        internal byte[] AsBytes()
        {
            var bytes = new byte[ByteSize];
            var bufferWriter = new BufferWriter(bytes, BitConverter.IsLittleEndian);

            bufferWriter.WriteInt64(this.DataListEndPoints.StartAddress);
            bufferWriter.WriteInt64(this.DataListEndPoints.EndAddress);
            bufferWriter.WriteInt64(this.EmptyListEndPoints.StartAddress);
            bufferWriter.WriteInt64(this.EmptyListEndPoints.EndAddress);

            bufferWriter.WriteInt64(this.NamedRecordIndexAddress);

            //do no use the trimmed buffer as we want some padding for future use
            return bufferWriter.Buffer; 
        }

        internal static CollectionMetaDataRecord FromBytes(byte[] bytes)
        {        
            var bufferReader = new BufferReader(bytes, BitConverter.IsLittleEndian);

            var startAddress = bufferReader.ReadInt64();
            var endAddress = bufferReader.ReadInt64();
            var dataListEndPoints = new ListEndPoints(startAddress, endAddress);

            startAddress = bufferReader.ReadInt64();
            endAddress = bufferReader.ReadInt64();
            var emptyListEndPoints = new ListEndPoints(startAddress, endAddress);

            var namedRecordIndexAddress = bufferReader.ReadInt64();

            return new CollectionMetaDataRecord(){                    
                DataListEndPoints = dataListEndPoints,
                EmptyListEndPoints = emptyListEndPoints,
                NamedRecordIndexAddress = namedRecordIndexAddress
            };
        }
            
        internal void Sanitize(){
            //when the data list is empty, the empty list is empty too.
            if(this.DataListEndPoints.StartAddress == 0) 
            {
                this.EmptyListEndPoints.StartAddress = 0;
                this.EmptyListEndPoints.EndAddress = 0;
            }
        }
	}
}

