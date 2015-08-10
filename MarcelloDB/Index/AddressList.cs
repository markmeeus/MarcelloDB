using System;
using System.Collections.Generic;
using System.Linq;

namespace MarcelloDB.Index
{
    public class AddressList
    {
        internal AddressList()
        {
            _addresses = new List<Int64>();
        }

        List<Int64> _addresses;

        public bool Dirty { get; set; }

        public Int64[] Addresses
        {
            get
            {
                return _addresses.ToArray();
            }
            set
            {
                _addresses = new List<long>(value);
            }
        }

        internal int Count
        {
            get
            {
                return _addresses.Count;
            }
        }

        internal Int64 First()
        {
            return _addresses.First();
        }

        internal Int64 Single()
        {
            return _addresses.Single();
        }

        internal Int64 Last()
        {
            return _addresses.Last();
        }

        internal int IndexOf(Int64 address)
        {
            return _addresses.IndexOf(address);
        }

        internal IEnumerable<Int64> GetRange(int index, int count)
        {
            return _addresses.GetRange(index, count);
        }

        #region mutating methods
        internal void Add(Int64 address)
        {
            _addresses.Add(address);
            this.Dirty = true;
        }

        internal void Insert(int index, Int64 address)
        {
            _addresses.Insert(index, address);
            this.Dirty = true;
        }

        internal Int64 this [int index]
        {
            get
            {
                return _addresses[index];
            }
            set
            {
                _addresses[index] = value;
                this.Dirty = true;
            }
        }

        internal void AddRange(IEnumerable<Int64> range)
        {
            _addresses.AddRange(range);
            this.Dirty = true;
        }

        internal void RemoveRange(int index, int count)
        {
            _addresses.RemoveRange(index, count);
            this.Dirty = true;
        }

        internal void RemoveAt(int index)
        {
            _addresses.RemoveAt(index);
            this.Dirty = true;
        }
        #endregion mutating methods
    }
}

