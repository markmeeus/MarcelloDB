using System;
using MarcelloDB.Records;
using MarcelloDB.Index;

namespace MarcelloDB.Serialization
{
    internal class EmptyRecordIndexNodeSerializer : IObjectSerializer<Node<EmptyRecordIndexKey>>
    {
        public EmptyRecordIndexNodeSerializer()
        {
        }

        static internal int MaxSizeForDegree(int degree)
        {
            var sizeOfDegree = sizeof(Int32);
            var sizeOfEntryListCount = sizeof(Int32);
            var maxSizeOfEntry = sizeof(Int64) + sizeof(Int64) + sizeof(Int32); //(Key, Address, Size)
            var maxSizeOfEntries = maxSizeOfEntry * Node<int>.MaxEntriesForDegree(degree);
            var sizeOfChildrenAddressessCount = sizeof(Int32);
            var maxSizeOfChildAddresses = sizeof(Int64) * Node<int>.MaxChildrenForDegree(degree);

            return sizeOfDegree +
                sizeOfEntryListCount +
                maxSizeOfEntries +
                sizeOfChildrenAddressessCount +
                maxSizeOfChildAddresses;
        }
        #region IObjectSerializer implementation

        public byte[] Serialize(Node<EmptyRecordIndexKey> node)
        {
            var bytes = new byte[MaxSizeForDegree(node.Degree)];
            var writer = new BufferWriter(bytes);
            writer.WriteInt32(node.Degree);                 //Degree

            writer.WriteInt32(node.EntryList.Count);          //Nr of entries
            foreach (var entry in node.EntryList.Entries)
            {
                WriteEntry(writer, entry);                  //every entry
            }

            writer.WriteInt32(node.ChildrenAddresses.Count); //Nr of child addresses
            foreach (var childAddress in node.ChildrenAddresses.Addresses)
            {
                writer.WriteInt64(childAddress);            //every child address
            }

            return writer.Buffer;
        }

        public Node<EmptyRecordIndexKey> Deserialize(byte[] bytes)
        {
            var reader = new BufferReader(bytes);
            var degree = reader.ReadInt32();
            var node = new Node<EmptyRecordIndexKey>(degree);

            var nrOfEntries = reader.ReadInt32();
            for(int i = 0; i< nrOfEntries; i++)
            {
                node.EntryList.Add(ReadEntry(reader));

            }
            var nrOfChildAddresses = reader.ReadInt32();
            for(int i = 0; i< nrOfChildAddresses; i++)
            {
                node.ChildrenAddresses.Add(reader.ReadInt64());

            }
            return node;
        }

        #endregion

        void WriteEntry(BufferWriter writer, Entry<EmptyRecordIndexKey> entry)
        {
            writer.WriteInt32(entry.Key.S);     //Size
            writer.WriteInt64(entry.Key.A);     //Address
            writer.WriteInt64(entry.Pointer);   //Pointer (which is address too);
        }

        Entry<EmptyRecordIndexKey> ReadEntry(BufferReader reader)
        {
            var s = reader.ReadInt32();
            var a = reader.ReadInt64();
            var pointer = reader.ReadInt64();
            return new Entry<EmptyRecordIndexKey>
            {
                Key = new EmptyRecordIndexKey{ S = s, A = a },
                Pointer = pointer
            };

        }
    }
}

