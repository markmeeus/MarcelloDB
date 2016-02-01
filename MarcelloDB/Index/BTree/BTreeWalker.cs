using System;
using System.Collections.Generic;
using System.Linq;

namespace MarcelloDB.Index.BTree
{
    public class BTreeWalker<TK, TP>
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

        internal BTreeWalker(int degree, IBTreeDataProvider<TK, TP> dataProvider )
        {
            this.Comparer = new ObjectComparer();

            this.DataProvider = dataProvider;
            this.Degree = degree;

            Reset();
        }

        public Entry<TK, TP> MoveTo(TK key)
        {
            Reset();
            return MoveToInternal(key);
        }


        public Entry<TK, TP> Next()
        {
            MoveNext();

            if (this.CurrentEntryIndex < CurrentNode.EntryList.Count)
            {
                return CurrentNode.EntryList[this.CurrentEntryIndex];
            }

            return null;
        }

        void Reset()
        {
            this.BreadCrumbs = new Stack<BreadCrumb>();
            this.CurrentEntryIndex = -1;
            this.CurrentNode = this.DataProvider.GetRootNode(this.Degree);
        }

        Entry<TK, TP> MoveToInternal(TK key)
        {
            int i = this.CurrentNode.EntryList.Entries
                .TakeWhile(entry => Comparer.Compare(key, entry.Key) > 0).Count();

            if (i < this.CurrentNode.EntryList.Count &&
                Comparer.Compare(this.CurrentNode.EntryList[i].Key, key) == 0)
            {
                //found it
                this.CurrentEntryIndex = i;
                return this.CurrentNode.EntryList[i];
            }

            if (this.CurrentNode.IsLeaf)
            {
                //it's a leaf, but no entry matches
                return null;
            }

            //push the breadcrumb
            var breadCrumb = new BreadCrumb(){
                CurrentEntryIndex = i - 1,
                Node = CurrentNode
            };
            this.BreadCrumbs.Push(breadCrumb);

            this.CurrentEntryIndex = 0;
            this.CurrentNode = this.DataProvider.GetNode(this.CurrentNode.ChildrenAddresses[i]);

            //and search in childnode
            return MoveToInternal(key);
        }

        void MoveNext()
        {
            //Walk down untill there are no more children on the right of current entry
            while (this.CurrentNode.ChildrenAddresses.Count > this.CurrentEntryIndex + 1)
            {
                var breadCrumb = new BreadCrumb(){
                    CurrentEntryIndex = this.CurrentEntryIndex,
                    Node = CurrentNode
                };
                this.BreadCrumbs.Push(breadCrumb);
                var childAddress = this.CurrentNode.ChildrenAddresses[this.CurrentEntryIndex + 1];
                this.CurrentEntryIndex = -1;
                this.CurrentNode = this.DataProvider.GetNode(childAddress);
            }

            //move to next entry
            this.CurrentEntryIndex += 1;

            //walk up untill an entry is found
            while (this.CurrentEntryIndex >= this.CurrentNode.EntryList.Count &&
                this.BreadCrumbs.Count > 0)
            {
                var breadCrumb = this.BreadCrumbs.Pop();
                this.CurrentEntryIndex = breadCrumb.CurrentEntryIndex;
                this.CurrentNode = breadCrumb.Node;
                this.CurrentEntryIndex += 1;
            }
        }
    }
}

