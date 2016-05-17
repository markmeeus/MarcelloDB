using System;
using MarcelloDB.Records;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Index.BTree;
using MarcelloDB.Serialization;

namespace MarcelloDB.Index
{
    internal class EmptyRecordIndex : RecordIndex<EmptyRecordIndexKey>
    {

        internal EmptyRecordIndex(
            Session session,
            IRecordManager recordManager,
            string indexName,
            IObjectSerializer<Node<EmptyRecordIndexKey>> serializer
        ):base(session, recordManager, indexName, serializer)
        {}
    }
}

