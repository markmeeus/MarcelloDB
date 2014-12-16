using System;
using System.Collections.Generic;

namespace Marcello.Index
{
    public class Node<TK, TP>
    {
        int degree;
        public Int64 Address{ get; set; }

        public Node(int degree)
        {
            this.degree = degree;
            this.ChildrenAddresses = new List<Int64>(degree);
            this.Entries = new List<Entry<TK, TP>>(degree);
        }

        public List<Int64> ChildrenAddresses { get; set; }

        public List<Entry<TK, TP>> Entries { get; set; }

        public bool IsLeaf
        {
            get
            {
                return this.ChildrenAddresses.Count == 0;
            }
        }

        public bool HasReachedMaxEntries
        {
            get
            {
                return this.Entries.Count == (2 * this.degree) - 1;
            }
        }

        public bool HasReachedMinEntries
        {
            get
            {
                return this.Entries.Count == this.degree - 1;
            }
        }
    }
}
