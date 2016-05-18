using System;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Transactions;
using MarcelloDB.Serialization;

namespace MarcelloDB
{
    internal class CollectionFileRoot
    {
        // first Int64 points to the collection file root record
        internal const int INITIAL_HEAD = sizeof(Int64);

        //Defines the format of file structure. Only change this value if backwards compatibility is broken.
        internal const int CURRENT_FORMAT_VERSION = 2;

        public int FormatVersion { get; set; }

        internal bool IsDirty { get; private set;}

        internal CollectionFileRoot()
        {
            this.Head = INITIAL_HEAD;
            this.FormatVersion = CURRENT_FORMAT_VERSION;
            this.IsDirty = true;
        }

        public static CollectionFileRoot Create()
        {
            return new CollectionFileRoot();
        }

        Int64 _head;
        public Int64 Head {
            get { return _head; }
            set
            {
                if (_head != value)
                {
                    _head = value;
                    this.IsDirty = true;
                }
            }
        }

        Int64 _namedRecordIndexAddress;
        public Int64 NamedRecordIndexAddress {
            get { return _namedRecordIndexAddress; }
            set
            {
                if (_namedRecordIndexAddress != value)
                {
                    _namedRecordIndexAddress = value;
                    this.IsDirty = true;
                }
            }
        }

        internal void Validate()
        {
            if (this.FormatVersion != CURRENT_FORMAT_VERSION)
            {
                throw new InvalidOperationException(
                    string.Format("This collectionfile was created with a previous and unsupported version of MarcellodB")
                );
            }
        }

        internal void Clean()
        {
            this.IsDirty = false;
        }
    }
}

