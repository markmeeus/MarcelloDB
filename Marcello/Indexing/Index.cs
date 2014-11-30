using System;
using Marcello.Collections;
using Marcello.Records;
using Marcello.Serialization;

namespace Marcello.Indexing
{
    internal class Index<T>
    {
        Collection<T> Collection { get; set; }

        RecordManager<T> recordManager { get; set; }

        internal Index(Collection<T> collection)
        {
            this.Collection = collection;
        }

        internal void RegisterAddress(T o, Int64 address)
        {
            var indexRootBlockAddress = GetIndexRootBlockAddress();
            var indexBlockAddres = GetIndexBlockAddressForObject(indexRootBlockAddress, o);
            this.Collection.StorageEngine.Write(indexBlockAddres + RecordHeader.ByteSize, AddressToBytes(address));        
        }

        internal Int64 GetAddressOf(T o)
        {
            var indexRootBlockAddress = GetIndexRootBlockAddress();
            var indexBlockAddres = GetIndexBlockAddressForObject(indexRootBlockAddress, o);
            var bytes = this.Collection.StorageEngine.Read(indexBlockAddres + RecordHeader.ByteSize, sizeof(Int64));
            return BytesToAddress(bytes);
        }

        Int64 GetIndexRootBlockAddress()
        {
            var metaDataRecord = this.Collection.RecordManager.GetMetaDataRecord();
               
            if (metaDataRecord.IndexRootBlockAddress == 0)
            {
                var rootBlockRecord = new Record();
                rootBlockRecord.Data = new byte[512 * (sizeof(Int64) + sizeof(Int32))];
                rootBlockRecord.Header.AllocatedDataSize = rootBlockRecord.Data.Length;
                rootBlockRecord.Header.DataSize = rootBlockRecord.Header.AllocatedDataSize;
                this.Collection.RecordManager.AppendRecord(rootBlockRecord);
                this.Collection.RecordManager.WithMetaDataRecord((metaDataRecordToUpdate) =>
                {
                    metaDataRecordToUpdate.IndexRootBlockAddress = rootBlockRecord.Header.Address;
                });

            }
                
            return this.Collection.RecordManager.GetMetaDataRecord().IndexRootBlockAddress;
        }

        Int64 GetIndexBlockAddressForObject(Int64 indexRootBlockAddress, T o)
        {
            //read ahead to the data section
            var blockAddress = BytesToAddress(
                this.Collection.StorageEngine.Read(
                    indexRootBlockAddress + RecordHeader.ByteSize, 
                    sizeof(Int64)));

            if (blockAddress == 0)
            {
                var rootBlockRecord = new Record();
                rootBlockRecord.Data = new byte[(int)Math.Pow(2,15) * (sizeof(Int64))]; //32768 record addresses / block
                rootBlockRecord.Header.AllocatedDataSize = rootBlockRecord.Data.Length;
                rootBlockRecord.Header.DataSize = rootBlockRecord.Header.AllocatedDataSize;
                this.Collection.RecordManager.AppendRecord(rootBlockRecord);
                blockAddress = rootBlockRecord.Header.Address;

                //write the block's address in the rootblock
                this.Collection.StorageEngine.Write(
                    indexRootBlockAddress + RecordHeader.ByteSize,
                    AddressToBytes(blockAddress));

            }
            return blockAddress;
        }

        Int64 BytesToAddress(byte[] bytes)
        {
            return new BufferReader(bytes, BitConverter.IsLittleEndian).ReadInt64();
        }

        byte[] AddressToBytes(Int64 address)
        {
            var bytes = new byte[sizeof(Int64)];
            new BufferWriter(bytes, BitConverter.IsLittleEndian)
                .WriteInt64(address);
            return bytes;
        }
    }
}

