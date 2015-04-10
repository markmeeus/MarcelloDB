using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;

namespace MarcelloDB.Index
{
    internal class RecordIndex
    {
        const int BTREE_DEGREE = 16;

        IBTree<object, Int64> Tree { get; set; }

        IBTreeDataProvider<object, Int64> DataProvider { get; set; }

        internal const string ID_INDEX_NAME = "__ID_INDEX__";
        internal const string EMPTY_RECORDS_BY_SIZE = "__EMPTY_RECORDS_BY_SIZE__";

        internal RecordIndex(IBTree<object, Int64> btree,
            IBTreeDataProvider<object, Int64> dataProvider)
        {
            this.Tree = btree;
            this.DataProvider = dataProvider;
        }

        internal BTreeWalker<object, Int64> GetWalker()
        {
            return new BTreeWalker<object, long>(BTREE_DEGREE, this.DataProvider);
        }

        internal Int64 Search(object keyValue)
        {
            var node = this.Tree.Search(keyValue);
            if (node != null)
            {
                return node.Pointer;
            }
            return 0;
        }

        internal void Register(object keyValue, Int64 recordAddress)
        {
            var entry = this.Tree.Search(keyValue);
            if (entry != null)
            {
                if (entry.Pointer != recordAddress)
                {
                    //keyvalue found, but record moved to other adress
                    this.Tree.Delete(keyValue);
                    this.Tree.Insert(keyValue, recordAddress);
                }
            }
            else
            {
                //keyvalue not yet in index
                this.Tree.Insert(keyValue, recordAddress);
            }

            this.DataProvider.Flush();           
            UpdateRootAddress();
        }

        internal void UnRegister(object keyValue)
        {
            this.Tree.Delete(keyValue);

            this.DataProvider.Flush();
            UpdateRootAddress();
        }

        internal static RecordIndex Create(
            IRecordManager recordManager, 
            string indexName, 
            bool canReuseRecycledRecords = true)
        {
            var dataProvider = new RecordBTreeDataProvider(
                recordManager, 
                new BsonSerializer<Node<object, Int64>>(),
                indexName,
                canReuseRecycledRecords);

            var btree = new BTree<object, Int64>(dataProvider, BTREE_DEGREE);

            return new RecordIndex(btree, dataProvider);
        }

        void UpdateRootAddress()
        {
            this.DataProvider.SetRootNodeAddress(this.Tree.Root.Address);
        }
    }
}

