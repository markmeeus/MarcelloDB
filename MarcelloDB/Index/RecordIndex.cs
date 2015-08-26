using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index.BTree;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Index
{
    internal class RecordIndex
    {
        internal const string ID_INDEX_PREFIX = "__ID_INDEX__";
        internal const string EMPTY_RECORDS_BY_SIZE = "__EMPTY_RECORDS_BY_SIZE__";
        internal const int BTREE_DEGREE = 12;

        internal static string GetIDIndexName<T>(string collectionName)
        {
            return ID_INDEX_PREFIX + collectionName.ToUpper() + "_" + typeof(T).Name.ToUpper();
        }

        internal static RecordIndex<TNodeKey> Create<TNodeKey>(
            IRecordManager recordManager,
            string indexName,
            IObjectSerializer<Node<TNodeKey, Int64>> serializer,
            bool canReuseRecycledRecords = true            )
        {
            var dataProvider = new RecordBTreeDataProvider<TNodeKey>(
                recordManager,
                serializer,
                indexName,
                canReuseRecycledRecords
            );

            var btree = new BTree<TNodeKey, Int64>(dataProvider, BTREE_DEGREE);

            return new RecordIndex<TNodeKey>(btree, dataProvider);
        }
    }

    internal class RecordIndex<TNodeKey>
    {

        IBTree<TNodeKey, Int64> Tree { get; set; }

        IBTreeDataProvider<TNodeKey, Int64> DataProvider { get; set; }

        internal RecordIndex(IBTree<TNodeKey, Int64> btree,
            IBTreeDataProvider<TNodeKey, Int64> dataProvider)
        {
            this.Tree = btree;
            this.DataProvider = dataProvider;
        }

        internal BTreeWalker<TNodeKey, Int64> GetWalker()
        {
            return new BTreeWalker<TNodeKey, long>(RecordIndex.BTREE_DEGREE, this.DataProvider);
        }

        internal Int64 Search(TNodeKey keyValue)
        {
            var node = this.Tree.Search(keyValue);
            if (node != null)
            {
                return node.Pointer;
            }
            return 0;
        }

        internal void Register(TNodeKey keyValue, Int64 recordAddress)
        {
            this.Tree.Insert(keyValue, recordAddress);
            FlushProvider();
        }

        internal void UnRegister(TNodeKey keyValue)
        {
            this.Tree.Delete(keyValue);
            FlushProvider();
        }

        void FlushProvider()
        {
            this.DataProvider.SetRootNode(this.Tree.Root);
            this.DataProvider.Flush();
        }
    }
}

