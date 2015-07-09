using System;
using MarcelloDB.Records;

namespace MarcelloDB.Serialization
{
    internal class IndexMetaRecordSerializer : IObjectSerializer<IndexMetaRecord>
    {
        #region IObjectSerializer implementation

        public byte[] Serialize(IndexMetaRecord record)
        {
            var bytes = new byte[ 5 * sizeof(Int64)];
            var writer = new BufferWriter(bytes);
            writer.WriteInt64(record.RootNodeAddress);
            writer.WriteInt64(record.NumberOfEntries);
            writer.WriteInt64(record.NumberOfNodes);
            writer.WriteInt64(record.TotalAllocatedSize);
            writer.WriteInt64(record.TotalAllocatedDataSize);
            return writer.GetTrimmedBuffer();
        }

        public IndexMetaRecord Deserialize(byte[] bytes)
        {
            var reader = new BufferReader(bytes);
            var deserializedRecord = new IndexMetaRecord();
            deserializedRecord.RootNodeAddress = reader.ReadInt64();
            deserializedRecord.NumberOfEntries = reader.ReadInt64();
            deserializedRecord.NumberOfNodes = reader.ReadInt64();
            deserializedRecord.TotalAllocatedSize = reader.ReadInt64();
            deserializedRecord.TotalAllocatedDataSize = reader.ReadInt64();
            return deserializedRecord;
        }

        #endregion

    }
}

