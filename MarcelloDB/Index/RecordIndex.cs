using System;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index.BTree;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Index
{
    internal class RecordIndex
    {
        internal const string EMPTY_RECORDS_BY_SIZE = "__EMPTY_RECORDS_BY_SIZE__";
        internal const int BTREE_DEGREE = 12;

        internal static string GetIndexName<T>(string collectionName, string indexName)
        {
            return string.Format("__INDEX_{0}_{1}",
                collectionName.ToUpper(),
                indexName.ToUpper());
        }
    }

    internal class RecordIndex<TNodeKey> : SessionBoundObject
    {
        internal IRecordManager RecordManager { get; set; }

        internal String IndexName { get; set; }

        internal IObjectSerializer<Node<TNodeKey>> Serializer { get; set; }

        internal RecordIndex(Session session, IRecordManager recordManager,
            string indexName,
            IObjectSerializer<Node<TNodeKey>> serializer) : base(session)
        {
            this.RecordManager = recordManager;
            this.IndexName = indexName;
            this.Serializer = serializer;
        }

        internal RecordIndex(Session session, IBTree<TNodeKey> btree,
            IBTreeDataProvider<TNodeKey> dataProvider): base(session)
        {
            this.DataProvider = dataProvider;
            this.Tree = btree;
        }
            
        IBTreeDataProvider<TNodeKey> _dataProvider;
        IBTreeDataProvider<TNodeKey> DataProvider 
        {
            get
            {
                if (_dataProvider == null)
                {
                    _dataProvider = new RecordBTreeDataProvider<TNodeKey>(
                        this.Session,
                        this.RecordManager,
                        this.Serializer,
                        this.IndexName
                    );
                }
                return _dataProvider;
            }
            set
            {
                _dataProvider = value;
            }
        }

        IBTree<TNodeKey> _tree;
        IBTree<TNodeKey> Tree {
            get
            {
                if (_tree == null)
                {
                    _tree = new BTree<TNodeKey>(this.DataProvider, RecordIndex.BTREE_DEGREE);
                }
                return _tree;

            }
            set
            {
                _tree = value;
            }
        }

        internal BTreeWalker<TNodeKey> GetWalker()
        {
            return new BTreeWalker<TNodeKey>(RecordIndex.BTREE_DEGREE, this.DataProvider);
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
            this.DataProvider.Flush();
        }
    }
}