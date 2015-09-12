using System;
using System.Collections.Generic;
using MarcelloDB.Serialization;

namespace MarcelloDB.Records
{
    internal class NamedRecordsIndex
    {
        public Dictionary<string, Int64> NamedRecordIndexes { get; set;}

        internal NamedRecordsIndex()
        {
            this.NamedRecordIndexes = new Dictionary<string, long>();
        }
    }
}

