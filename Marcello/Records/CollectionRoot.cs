using System;
using Marcello.Serialization;

namespace Marcello.Records
{
	internal class CollectionRoot
	{		
        internal Int64 NamedRecordIndexAddress { get; set;}
        internal Int64 Head { get; set;}

        internal CollectionRoot()
		{
            this.Head = ByteSize;
		}

        internal static int ByteSize
        {
            get { return 1024; } //some padding for future use 
        }

        internal byte[] AsBytes()
        {
            var bytes = new byte[ByteSize];
            var bufferWriter = new BufferWriter(bytes, BitConverter.IsLittleEndian);

            bufferWriter.WriteInt64(this.NamedRecordIndexAddress);
            bufferWriter.WriteInt64(Head);

            //do no use the trimmed buffer as we want some padding for future use
            return bufferWriter.Buffer; 
        }

        internal static CollectionRoot FromBytes(byte[] bytes)
        {        
            var bufferReader = new BufferReader(bytes, BitConverter.IsLittleEndian);

            var namedRecordIndexAddress = bufferReader.ReadInt64();
            var head = bufferReader.ReadInt64();

            if (head == 0)
            {
                head = ByteSize; //when reading from empty data
            }

            return new CollectionRoot(){                    
                NamedRecordIndexAddress = namedRecordIndexAddress,
                Head = head
            };
        }           
	}
}

