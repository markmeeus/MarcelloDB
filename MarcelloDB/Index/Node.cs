using System;
using System.Collections.Generic;

namespace MarcelloDB.Index
{
    public class Node<TK, TP>
    {
        public int Degree { get; set; }
        public Int64 Address{ get; set; }

        public AddressList ChildrenAddresses { get; set; }

        public List<Entry<TK, TP>> Entries { get; set; }

        public Node(int degree)
        {
            this.Degree = degree;
            this.ChildrenAddresses = new AddressList();
            this.Entries = new List<Entry<TK, TP>>(degree);
        }

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
                return this.Entries.Count == (2 * this.Degree) - 1;
            }
        }

        public bool HasReachedMinEntries
        {
            get
            {
                return this.Entries.Count == this.Degree - 1;
            }
        }
    }
}
