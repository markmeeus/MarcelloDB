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
        BTreeWalkerRange<TKey> Range { get; set; }

        Collection<T> Collection { get; set; }

        RecordIndex<TKey> Index { get; set; }

        bool HasRange{ get { return this.Range != null; } }

        bool IsDescending { get; set; }

        internal KeysEnumerator(
            Collection<T> collection,
            Session session,
            RecordIndex<TKey> index,
            bool isDescending = false
        ) : base(session)
        {
            this.Collection = collection;
            this.Index = index;
            this.IsDescending = isDescending;
        }

        internal void SetRange(BTreeWalkerRange<TKey> range)
        {
            this.Range = range;
        }

        #region IEnumerable implementation
        IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
        {
            lock (this.Session.SyncLock)
            {
                var indexEnumerator = new IndexEntryEnumerator<T, TKey>(
                                      this.Collection,
                                      this.Session,
                                      this.Index,
                                      this.IsDescending);

                indexEnumerator.SetRange(this.Range);

                foreach (var node in indexEnumerator)
                {
                    yield return node.Key;
                }
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

