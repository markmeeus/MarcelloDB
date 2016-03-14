using System;
using MarcelloDB.Collections;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;
using System.Collections.Generic;
using System.Collections;

namespace MarcelloDB
{
    internal class KeysEnumerator<T, TKey>  : SessionBoundObject, IEnumerable<TKey>
    {
        Collection<T> Collection { get; set; }

        RecordIndex<TKey> Index { get; set; }

        BTreeWalkerRange<TKey> Range { get; set; }

        bool HasRange{ get { return this.Range != null; } }

        bool IsDescending { get; set; }

        internal KeysEnumerator(
            Collection<T> collection,
            Session session,
            RecordIndex<TKey> index,
            bool IsDescending = false
        ) : base(session)
        {
            this.Collection = collection;
            this.Index = index;
        }

        #region IEnumerable implementation
        IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
        {
            var indexEnumerator = new IndexEntryEnumerator<T, TKey>(
                                      this.Collection,
                                      this.Session,
                                      this.Index,
                                      this.IsDescending);

            foreach (var node in indexEnumerator)
            {
                yield return node.Key;
            }
        }
        #endregion
        #region IEnumerable implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TKey>)this).GetEnumerator();
        }
        #endregion
    }
}

