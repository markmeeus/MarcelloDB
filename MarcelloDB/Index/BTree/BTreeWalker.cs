using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace MarcelloDB.Index.BTree
{
    internal class BTreeWalkerRange<TK>
    {
        internal TK StartAt { get; set; }

        internal bool HasStartAt { get; set; }

        internal bool IncludeStartAt { get; set; }

        internal TK EndAt { get; set; }

        internal bool IncludeEndAt { get; set; }

        internal bool HasEndAt { get; set; }

        internal BTreeWalkerRange() {}

        internal BTreeWalkerRange(TK startAt, TK endAt)
        {
            this.SetStartAt(startAt);
            this.SetEndAt(endAt);
        }

        internal void SetStartAt(TK startAt)
        {
            this.StartAt = startAt;
            this.HasStartAt = true;
            this.IncludeStartAt = true;
        }

        internal void SetEndAt(TK endAt)
        {
            this.EndAt = endAt;
            this.HasEndAt = true;
            this.IncludeEndAt = true;
        }
    }

    internal class BTreeWalker<TK, TP>
    {
        class BreadCrumb
        {
            internal int CurrentEntryIndex { get; set;}
            internal Node<TK, TP> Node { get; set; }
        }

        IBTreeDataProvider<TK, TP> DataProvider { get; set; }

        int Degree { get; set; }

        int CurrentEntryIndex { get; set; }

        Node<TK, TP> CurrentNode { get; set; }

        Stack<BreadCrumb> BreadCrumbs { get; set; }

        ObjectComparer Comparer { get; set; }

        BTreeWalkerRange<TK> Range { get;set; }

        bool Reversed { get; set; }

        internal BTreeWalker(int degree, IBTreeDataProvider<TK, TP> dataProvider)
        {
            this.Comparer = new ObjectComparer();

            this.DataProvider = dataProvider;
            this.Degree = degree;

            Reset();
        }

        internal void Reverse()
        {
            this.Reversed = true;
            this.Comparer.Invert();
            Reset();
        }

        internal void SetRange(BTreeWalkerRange<TK> range)
        {
            if (range.HasStartAt && range.HasEndAt)
            {
                if (Comparer.Compare(range.StartAt, range.EndAt) > 0)
                {
                    throw new InvalidOperationException("startAt must be smaller then endAt");
                }
            }

            Reset();
            this.Range = range;
        }

        public Entry<TK, TP> Next()
        {
            if (this.Range != null && this.Range.HasStartAt
                && this.CurrentEntryIndex < 0 && this.BreadCrumbs.Count == 0)
            {
                if (!this.Range.IncludeStartAt)
                {
                    //searching backwards points us to the last item
                    MoveTo(this.Range.StartAt, true);
                    //move next to target the first greater item (unless there is no item found)
                    if (this.CurrentEntryIndex >= 0)
                    {
                        MoveNext();
                    }
                }
                else
                {
                    MoveTo(this.Range.StartAt, false);
                }

                if (this.CurrentEntryIndex < 0)
                {
                    //No current item, move next;
                    MoveNext();
                }
            }
            else
            {
                MoveNext();
            }

            if (this.CurrentEntryIndex >= 0 && this.CurrentEntryIndex < Entries(CurrentNode).Count())
            {
                var entry = Entries(CurrentNode).ElementAt(this.CurrentEntryIndex);
                if(EntryIsBeforeEndOfRange(entry))
                {
                    return entry;
                }
                return null;
            }
            return null;
        }

        void Reset()
        {
            this.BreadCrumbs = new Stack<BreadCrumb>();
            this.CurrentEntryIndex = -1;
            this.CurrentNode = this.DataProvider.GetRootNode(this.Degree);
        }

        void MoveTo(TK key, bool backwards)
        {

            var entries = Entries(this.CurrentNode) as IEnumerable<Entry<TK, TP>>;

            var childrenAddresses = ChildrenAddresses(this.CurrentNode) as IEnumerable<Int64>;

            if (backwards)
            {
                entries = entries.Reverse();
                childrenAddresses = childrenAddresses.Reverse();
            }

            int i = -1;
            if (!backwards)
            {
                i = entries.TakeWhile(entry => Comparer.Compare(key, entry.Key) > 0).Count();
            }
            else
            {
                i = entries.TakeWhile(entry => Comparer.Compare(key, entry.Key) < 0).Count();
            }

            int absoluteIndex = backwards ? (entries.Count() - 1) - i : i;

            if (i < entries.Count() &&
                Comparer.Compare(entries.ElementAt(i).Key, key) == 0)
            {
                //found it
                this.CurrentEntryIndex = absoluteIndex;
                return;
            }
            else if (this.CurrentNode.IsLeaf)
            {
                //it's a leaf, but no entry matches
                this.CurrentEntryIndex = absoluteIndex;
                return;
            }
            else if (i >= childrenAddresses.Count())
            {
                //tree seems unbalanced
                this.CurrentEntryIndex = absoluteIndex;
                return;
            }

            //push the breadcrumb
            var breadCrumb = new BreadCrumb(){
                CurrentEntryIndex = backwards ? absoluteIndex : absoluteIndex - 1,
                Node = CurrentNode
            };

            this.BreadCrumbs.Push(breadCrumb);

            // set the target child as current node

            this.CurrentNode = this.DataProvider.GetNode(childrenAddresses.ElementAt(i));

            //and search in childnode
            MoveTo(key, backwards);
        }

        void MoveNext()
        {
            //Walk down untill there are no more children on the right of current entry
            while (ChildrenAddresses(this.CurrentNode).Count() > this.CurrentEntryIndex + 1)
            {
                var breadCrumb = new BreadCrumb(){
                    CurrentEntryIndex = this.CurrentEntryIndex,
                    Node = CurrentNode
                };
                this.BreadCrumbs.Push(breadCrumb);
                var childAddress = ChildrenAddresses(this.CurrentNode).ElementAt(this.CurrentEntryIndex + 1);
                this.CurrentEntryIndex = -1;
                this.CurrentNode = this.DataProvider.GetNode(childAddress);
            }

            //move to next entry
            this.CurrentEntryIndex += 1;

            //walk up untill an entry is found
            while (this.CurrentEntryIndex >= Entries(this.CurrentNode).Count() &&
                this.BreadCrumbs.Count > 0)
            {
                var breadCrumb = this.BreadCrumbs.Pop();
                this.CurrentEntryIndex = breadCrumb.CurrentEntryIndex;
                this.CurrentNode = breadCrumb.Node;
                this.CurrentEntryIndex += 1;
            }
        }

        IEnumerable<Entry<TK, TP>> Entries(Node<TK, TP> node)
        {
            if (this.Reversed) {
                return ((IEnumerable<Entry<TK, TP>>)node.EntryList).Reverse();
            }
            return node.EntryList;
        }

        IEnumerable<Int64> ChildrenAddresses(Node<TK, TP> node)
        {
            if (this.Reversed) {
                return ((IEnumerable<Int64> )node.ChildrenAddresses).Reverse();
            }
            return node.ChildrenAddresses;
        }

        bool EntryIsBeforeEndOfRange(Entry<TK, TP> entry)
        {
            if (this.Range == null)
                return true;

            if(!this.Range.HasEndAt)
                return true;

            if (this.Range.IncludeEndAt)
                return Comparer.Compare(entry.Key, this.Range.EndAt) <= 0;
            else
                return Comparer.Compare(entry.Key, this.Range.EndAt) < 0;
        }
    }
}

