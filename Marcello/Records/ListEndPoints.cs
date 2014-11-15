using System;

namespace Marcello.Records
{
    internal class ListEndPoints
    {
        internal Int64 StartAddress { get; set;}

        internal Int64 EndAddress { get; set;}

        internal ListEndPoints(Int64 startAddress, Int64 endAddress)
        {
            this.StartAddress = startAddress;
            this.EndAddress = endAddress;
        }
    }
}

