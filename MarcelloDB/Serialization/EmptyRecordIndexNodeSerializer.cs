using System;
using MarcelloDB.Records;
using MarcelloDB.Index;

namespace MarcelloDB.Serialization
{
    internal class EmptyRecordIndexNodeSerializer : IObjectSerializer<Node<EmptyRecordIndexKey, Int64>>
    {
        internal EmptyRecordIndexNodeSerializer()
        {
        }

        static internal int MaxSizeForDegree(int degree)
        {
            var sizeOfDegree = sizeof(Int32);
            var maxSizeOfEntry = sizeof(Int64) + sizeof(Int64) + sizeof(Int32);
            var maxSizeOfEntries = maxSizeOfEntry * (degree * 2);
            var maxSizeOfChildAddresses = sizeof(Int64);

            return sizeOfDegree +
                maxSizeOfEntries +
                maxSizeOfChildAddresses;
        }
        #region IObjectSerializer implementation

        public byte[] Serialize(Node<EmptyRecordIndexKey, Int64> node)
        {
            var bytes = new byte[MaxSizeForDegree(node.Degree)];
            var writer = new BufferWriter(bytes);
            writer.WriteInt32(node.Degree);                 //Degree

            writer.WriteInt32(node.Entries.Count);          //Nr of entries
            foreach (var entry in node.Entries)
            {
                WriteEntry(writer, entry);                  //every entry
            }

            writer.WriteInt32(node.ChildrenAddresses.Count); //Nr of child addresses
            foreach (var childAddress in node.ChildrenAddresses.Addresses)
            {
                writer.WriteInt64(childAddress);            //every child address
            }

            return writer.GetTrimmedBuffer();
        }

        public Node<EmptyRecordIndexKey, Int64> Deserialize(byte[] bytes)
        {
            var reader = new BufferReader(bytes);
            var degree = reader.ReadInt32();
            var node = new Node<EmptyRecordIndexKey, Int64>(degree);

            var nrOfEntries = reader.ReadInt32();
            for(int i = 0; i< nrOfEntries; i++)
            {
                node.Entries.Add(ReadEntry(reader));

            }
            var nrOfChildAddresses = reader.ReadInt32();
            for(int i = 0; i< nrOfChildAddresses; i++)
            {
                node.ChildrenAddresses.Add(reader.ReadInt64());

            }
            return node;
        }

        #endregion

        void WriteEntry(BufferWriter writer, Entry<EmptyRecordIndexKey, long> entry)
        {
            writer.WriteInt32(entry.Key.S);     //Size
            writer.WriteInt64(entry.Key.A);     //Address
            writer.WriteInt64(entry.Pointer);   //Pointer (which is address too);
        }

        Entry<EmptyRecordIndexKey, long> ReadEntry(BufferReader reader)
        {
            var s = reader.ReadInt32();
            var a = reader.ReadInt64();
            var pointer = reader.ReadInt64();
            return new Entry<EmptyRecordIndexKey, long>
            {
                Key = new EmptyRecordIndexKey{ S = s, A = a },
                Pointer = pointer
            };
        }
    }
}

