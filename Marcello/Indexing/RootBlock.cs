using System;
using Marcello.Records;
using Marcello.Serialization;

namespace Marcello.Indexing
{
    internal class RootBlock
    {
        DirectRecordAccessor RecordAccessor{ get; set;}

        internal RootBlock(DirectRecordAccessor recordAccessor)
        {
            this.RecordAccessor = recordAccessor;
        }

        int DataOffset
        {
            get{
                return sizeof(Int32);
            }
        }

        internal int Count(){
            var bytes = RecordAccessor.Read(0, sizeof(Int32));
            return new BufferReader(bytes, BitConverter.IsLittleEndian).ReadInt32();
        }
            
        internal RootBlockEntry GetEntry(int entryIndex)
        {
            var index = EntryIndexToIndex(entryIndex);

            var bytes = RecordAccessor.Read(index, RootBlockEntry.ByteSize);

            return RootBlockEntry.FromBytes(bytes);
        }

        internal void AddEntry(int entryIndex, RootBlockEntry rootBlockEntry)
        {
            var bytes = rootBlockEntry.ToBytes();
            var index = EntryIndexToIndex(entryIndex);
            RecordAccessor.Write(index, bytes);
            IncreaseCount();
        }

        internal void UpdateEntry(int entryIndex, RootBlockEntry rootBlockEntry)
        {
            var bytes = rootBlockEntry.ToBytes();
            var index = EntryIndexToIndex(entryIndex);
            RecordAccessor.Write(index, bytes);
            IncreaseCount();
        }

        private void IncreaseCount()
        {
            var bytes = new byte[sizeof(Int32)];
            var writer = new BufferWriter(bytes, BitConverter.IsLittleEndian);
            writer.WriteInt32(Count() + 1);
            RecordAccessor.Write(0, writer.Buffer);
        }

        private int EntryIndexToIndex(int entryIndex)
        {
            return DataOffset + entryIndex * RootBlockEntry.ByteSize;
        }
    }
}

