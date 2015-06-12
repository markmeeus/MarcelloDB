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

        //to make it serializable
        public Dictionary<string, Int64> CollectionRootAddresses { get; set; }

        internal bool IsDirty { get; private set;}

        internal CollectionFileRoot()
        {
            this.Head = INITIAL_HEAD;
            this.CollectionRootAddresses = new Dictionary<string, long>();
            this.IsDirty = true;
        }

        Int64 _head;
        public Int64 Head {
            get { return _head; }
            set {
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
            set {
                if (_namedRecordIndexAddress != value)
                {
                    _namedRecordIndexAddress = value;
                    this.IsDirty = true;
                }
            }
        }


        internal void SetCollectionRootAddress(string collectionName, Int64 address)
        {
            if (!this.CollectionRootAddresses.ContainsKey(collectionName)
                || this.CollectionRootAddresses[collectionName] != address)
            {
                this.IsDirty = true;
                this.CollectionRootAddresses[collectionName] = address;
            }
        }

        internal Int64 CollectionRootAddress(string collectionName)
        {
            return this.CollectionRootAddresses[collectionName];
        }


        internal void Clean()
        {
            this.IsDirty = false;
        }

        public static CollectionFileRoot Create()
        {
            return new CollectionFileRoot();
        }


        internal byte[] Serialize()
        {
            var serializer = new BsonSerializer<CollectionFileRoot>();
            return serializer.Serialize(this);
        }

        public static CollectionFileRoot Deserialize(byte[] bytes)
        {
            var serializer = new BsonSerializer<CollectionFileRoot>();
            var deser = serializer.Deserialize(bytes);
            return deser;
        }
    }
}

