using System;
using System.Collections.Generic;
using Marcello.Serialization;

namespace Marcello.Records.__
{
    public class NamedRecordsIndex
    {
        public Dictionary<string, Int64> NamedRecordIndexes { get; set;}

        public NamedRecordsIndex()
        {
            this.NamedRecordIndexes = new Dictionary<string, long>();
        }

        public byte[] ToBytes(){
            var bytes = GetSerializer().Serialize(this);
            return bytes;
        }

        public static NamedRecordsIndex FromBytes(byte[] bytes){
            return GetSerializer().Deserialize(bytes);
        }

        static IObjectSerializer<NamedRecordsIndex> GetSerializer(){
            return new BsonSerializer<NamedRecordsIndex>();
        }
    }
}

