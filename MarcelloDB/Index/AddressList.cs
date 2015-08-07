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
            this.Dirty = true;
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

        internal int Count{
            get{
                return _addresses.Count;
            }
        }

        internal void Add(Int64 address){
            _addresses.Add(address);
        }

        internal Int64 Single()
        {
            return _addresses.Single();
        }

        internal Int64 First()
        {
            return _addresses.First();
        }

        internal Int64 Last()
        {
            return _addresses.Last();
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
            }
        }

        internal int IndexOf(Int64 address)
        {
            return _addresses.IndexOf(address);
        }

        internal void Insert(int index, Int64 address)
        {
            _addresses.Insert(index, address);
        }


        internal IEnumerable<Int64> GetRange(int index, int count)
        {
            return _addresses.GetRange(index, count);
        }

        internal void AddRange(IEnumerable<Int64> range)
        {
            _addresses.AddRange(range);
        }

        internal void RemoveRange(int index, int count)
        {
            _addresses.RemoveRange(index, count);
        }

        internal void RemoveAt(int index)
        {
            _addresses.RemoveAt(index);
        }
    }
}

