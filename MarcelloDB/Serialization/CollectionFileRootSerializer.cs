using System;
using System.Text;

namespace MarcelloDB.Serialization
{
    internal class CollectionFileRootSerializer : IObjectSerializer<CollectionFileRoot>
    {
        const int SERIALIZATION_VERSION = 1;

        public CollectionFileRootSerializer()
        {
        }

        #region IObjectSerializer implementation

        public byte[] Serialize(CollectionFileRoot root)
        {
            var writer = new BufferWriter(new byte[0]);
            writer.WriteByte(SERIALIZATION_VERSION);
            writer.WriteInt32(root.FormatVersion);
            writer.WriteInt64(root.Head);
            writer.WriteInt64(root.NamedRecordIndexAddress);

            return writer.GetTrimmedBuffer();
        }

        public CollectionFileRoot Deserialize(byte[] bytes)
        {
            var reader = new BufferReader(bytes);
            var root = new CollectionFileRoot();
            reader.ReadByte(); //read SERIALIZATION_VERSION
            root.FormatVersion = reader.ReadInt32();
            root.Head = reader.ReadInt64();
            root.NamedRecordIndexAddress = reader.ReadInt64();

            root.Clean();
            return root;
        }

        #endregion
    }
}

