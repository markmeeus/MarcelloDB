using System;
using MarcelloDB.Serialization;

namespace MarcelloDB.Records
{
	internal class CollectionRoot
	{		
        const int CURRENT_FORMAT_VERSION = 1;

        internal Int32 FormatVersion { get; set;}
        internal Int64 NamedRecordIndexAddress { get; set;}
        internal Int64 Head { get; set;}

        internal CollectionRoot()
		{
            this.Head = ByteSize;
            this.FormatVersion = CURRENT_FORMAT_VERSION;
		}

        internal static int ByteSize
        {
            get { return 1024; } //some padding for future use 
        }

        internal byte[] AsBytes()
        {
            var bytes = new byte[ByteSize];
            var bufferWriter = new BufferWriter(bytes, BitConverter.IsLittleEndian);

            bufferWriter.WriteInt32(this.FormatVersion);
            bufferWriter.WriteInt64(this.NamedRecordIndexAddress);
            bufferWriter.WriteInt64(Head);

            //do no use the trimmed buffer as we want some padding for future use
            return bufferWriter.Buffer; 
        }

        internal static CollectionRoot FromBytes(byte[] bytes)
        {        
            var bufferReader = new BufferReader(bytes, BitConverter.IsLittleEndian);
            var formatVersion = bufferReader.ReadInt32();
            var namedRecordIndexAddress = bufferReader.ReadInt64();
            var head = bufferReader.ReadInt64();

            if (head == 0)
            {
                head = ByteSize; //when reading from empty data
            }

            return new CollectionRoot(){                    
                NamedRecordIndexAddress = namedRecordIndexAddress,
                Head = head,
                FormatVersion = formatVersion
            };
        }           
	}
}

