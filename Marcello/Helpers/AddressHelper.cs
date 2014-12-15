using System;
using Marcello.Serialization;

namespace Marcello.Helpers
{
    internal class AddressHelper
    {
        internal AddressHelper()
        {
        }

        public Int64 BytesToAddress(byte[] bytes)
        {
            return new BufferReader(bytes, BitConverter.IsLittleEndian).ReadInt64();
        }

        public byte[] AddressToBytes(Int64 address)
        {
            var buffer = new byte[sizeof(Int64)];
            new BufferWriter(buffer, BitConverter.IsLittleEndian).WriteInt64(address);
            return buffer;
        }
    }
}

