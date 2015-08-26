using System;

namespace MarcelloDB.Records
{
    internal class IndexMetaRecord
    {
        internal Int64 RootNodeAddress { get; set; }
        internal Int64 NumberOfNodes { get; set; }
        //not used yet
        internal Int64 NumberOfEntries { get; set; }
        internal Int64 TotalAllocatedSize { get; set; }
        internal Int64 TotalAllocatedDataSize { get; set; }

        internal Record Record { get; set; }
    }
}

