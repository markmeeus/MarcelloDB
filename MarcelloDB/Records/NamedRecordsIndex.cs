using System;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using MarcelloDB.Buffers;

namespace MarcelloDB.Records.__
{
    public class NamedRecordsIndex
    {
        public Dictionary<string, Int64> NamedRecordIndexes { get; set;}

        public NamedRecordsIndex()
        {
            this.NamedRecordIndexes = new Dictionary<string, long>();
        }

        public ByteBuffer ToBuffer(Session session)
        {
            var bytes = GetSerializer().Serialize(this);
            return session.ByteBufferManager.FromBytes(bytes);
        }

        public static NamedRecordsIndex FromBuffer(ByteBuffer buffer)
        {
            return GetSerializer().Deserialize(buffer);
        }

        static IObjectSerializer<NamedRecordsIndex> GetSerializer()
        {
            return new BsonSerializer<NamedRecordsIndex>();
        }
    }
}

