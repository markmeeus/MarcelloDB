using System;
using Marcello.Helpers;
using Marcello.Indexing;

namespace Marcello.Records
{
    internal class RecordBasedAddressArray
    {
        private class AddressLocation
        {
            public DirectRecordAccessor Block{get;set;}
            public int Index{get;set;}
        }

        RecordManager RecordManager { get; set;}

        int MaxBlocks {get;set;}

        int BlockSize {get;set;}

        RootBlock RootBlock {get;set;}

        AddressHelper AddressHelper { get; set;}

        internal RecordBasedAddressArray(
            int rootBlockAddress,
            int maxBlocks, 
            int blockSize, 
            RecordManager recordManager)
        {
            this.MaxBlocks = maxBlocks;
            this.BlockSize = blockSize;
            this.RecordManager = recordManager;
            this.RootBlock = CreateRootBlock();
            this.AddressHelper = new AddressHelper();
        }

        internal Int64 At(int index)
        {
            var addressLocation = GetAddressLocation(index); 

            return AddressHelper.BytesToAddress(
                addressLocation.Block.Read(addressLocation.Index, sizeof(Int64))
            );
            
        }

        internal void Insert(Int64 address)
        {
            var addressLocation = GetNextAddressLocation(); 

            addressLocation.Block.Write(addressLocation.Index, AddressHelper.AddressToBytes(address));

        }

        #region private methods
        private RootBlock CreateRootBlock()
        {
            var rootBlockRecord = new RecordBuilder().Build(
                new byte[this.MaxBlocks * (sizeof(Int64) + sizeof(Int32))], 
                this.MaxBlocks * (sizeof(Int64) + sizeof(Int32))
            );
            this.RecordManager.AppendRecord(rootBlockRecord);
            return new RootBlock(
                new DirectRecordAccessor(rootBlockRecord.Header.Address, this.RecordManager)
            );
        }

        private AddressLocation GetNextAddressLocation()
        {
            if (RootBlock.Count() == 0)
            {                        
                var blockRecord = new RecordBuilder().Build(new byte[this.BlockSize * sizeof(Int64)]);
                this.RecordManager.AppendRecord(blockRecord);
                var rootBlockEntry = new RootBlockEntry()
                {
                    BlockAddress = blockRecord.Header.Address,
                    Used = 0
                };
                this.RootBlock.AddEntry(0, rootBlockEntry);                             
            }

            var blockEntry = RootBlock.GetEntry(0);

            int index = blockEntry.Used;
            blockEntry.Used += 1;
            RootBlock.UpdateEntry(0, blockEntry);

            return new AddressLocation(){
                Block = new DirectRecordAccessor(blockEntry.BlockAddress, this.RecordManager),
                Index = (int)(index * sizeof(Int64))
            };
        }

        private AddressLocation GetAddressLocation(int index)
        {
            var result = new AddressLocation();
            var blockIndex = 0;
            var blockStartIndex = 0;
            RootBlockEntry blockEntry = null;
            while (blockIndex < RootBlock.Count())
            {
                blockEntry = RootBlock.GetEntry(blockIndex);
                if (blockStartIndex + blockEntry.Used > index)                
                    break;
                blockStartIndex += blockEntry.Used;
            }
            result.Block = new DirectRecordAccessor(blockEntry.BlockAddress, this.RecordManager);
            result.Index = index - blockStartIndex;
            return result;
        }


        #endregion private methods

    }
}

