using System;
using Marcello.Collections;
using Marcello.Records;

namespace Marcello.Indexing
{
    internal class Index<T>
    {
        Collection<T> Collection { get; set; }

        RecordManager<T> recordManager { get; set; }

        internal Index(Collection<T> collection)
        {
        }

        internal void RegisterAddress(T o, Int64 address)
        {

        }

        internal Int64 GetAddressOf(T o)
        {
            return 0;
        }
    }
}

