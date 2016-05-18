using System;

namespace MarcelloDB.Records
{
    internal class IndexMetaRecord
    {
        Int64 _rootNodeAddress;
        internal Int64 RootNodeAddress {
            get { return _rootNodeAddress; }
            set {
                if (_rootNodeAddress != value)
                {
                    _rootNodeAddress = value;
                    this.IsDirty = true;
                }
            }
        }

        Int64 _numberOfNodes;
        internal Int64 NumberOfNodes {
            get { return _numberOfNodes;}
            set
            {
                if (_numberOfNodes != value)
                {
                    _numberOfNodes = value;
                    this.IsDirty = true;
                }
            }
        }

        //not used yet
        Int64 _numberOfEntries;
        internal Int64 NumberOfEntries
        {
            get {return _numberOfEntries; }
            set
            {
                if (_numberOfEntries != value)
                {
                    _numberOfEntries = value;
                    this.IsDirty = true;
                }
            }
        }

        Int64 _totalAllocatedSize;
        internal Int64 TotalAllocatedSize
        {
            get {return _totalAllocatedSize; }
            set
            {
                if (_totalAllocatedSize != value)
                {
                    _totalAllocatedSize = value;
                    this.IsDirty = true;
                }
            }
        }

        Int64 _totalAllocatedDataSize;
        internal Int64 TotalAllocatedDataSize
        {
            get {return _totalAllocatedDataSize; }
            set
            {
                if (_totalAllocatedDataSize != value)
                {
                    _totalAllocatedDataSize = value;
                    this.IsDirty = true;
                }
            }
        }

        internal Record Record { get; set; }

        internal bool IsDirty { get; private set; }

        internal void Clean()
        {
            this.IsDirty = false;
        }
    }
}

