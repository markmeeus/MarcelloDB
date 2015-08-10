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
            this.Added = new List<Int64>();
            this.Removed = new List<Int64>();
        }

        List<Int64> _addresses;

        public bool Dirty
        {
            get
            {
                return this.Added.Count() > 0 || this.Removed.Count() > 0;
            }
        }

        public void ClearChanges()
        {
            this.Added.Clear();
            this.Removed.Clear();
        }

        public List<Int64> Added {get; private set;}
        public List<Int64> Removed {get; private set;}


        public Int64[] Addresses
        {
            get
            {
                return _addresses.ToArray();
            }
            set
            {
                //Currently here for deserialization purposes only
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
            this.Added.Add(address);
        }

        internal void Insert(int index, Int64 address)
        {
            _addresses.Insert(index, address);
            this.Added.Add(address);
        }

        internal Int64 this [int index]
        {
            get
            {
                return _addresses[index];
            }
            set
            {
                if (_addresses[index] != value)
                {
                    this.Removed.Add(_addresses[index]);
                    _addresses[index] = value;
                    this.Added.Add(value);
                }
            }
        }

        internal void AddRange(IEnumerable<Int64> range)
        {
            _addresses.AddRange(range);
            this.Added.AddRange(range);
        }

        internal void RemoveRange(int index, int count)
        {
            this.Removed.AddRange(_addresses.GetRange(index, count));
            _addresses.RemoveRange(index, count);

        }

        internal void RemoveAt(int index)
        {
            this.Removed.Add(_addresses[index]);
            _addresses.RemoveAt(index);

        }
        #endregion mutating methods
    }
}

