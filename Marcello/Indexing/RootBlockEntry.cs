using System;
using Marcello.Serialization;

namespace Marcello.Indexing
{
    public class RootBlockEntry
    {
        public Int64 BlockAddress{ get; set;}

        public int Used{ get; set;}

        public RootBlockEntry()
        {
        }

        public static int ByteSize 
        {
            get{
                return sizeof(Int64) + sizeof(Int32);
            }
        }

        public static RootBlockEntry FromBytes(byte[] bytes)
        {
            BufferReader reader = new BufferReader(bytes, BitConverter.IsLittleEndian);
            var blockAddress = reader.ReadInt64();
            var used = reader.ReadInt32();
            return new RootBlockEntry()
            {
                BlockAddress = blockAddress,
                Used = used
            };
        }

        public byte[] ToBytes(){
            var bytes = new byte[ByteSize];
            var bufferWriter = new BufferWriter(bytes, BitConverter.IsLittleEndian);
            bufferWriter.WriteInt64(BlockAddress);
            bufferWriter.WriteInt32(Used);
            return bufferWriter.Buffer;
        }
    }
}

