using System;
using System.Collections.Generic;
using System.Linq;

namespace MarcelloDB.Helpers
{
    public class ChangeTrackingList<T>
    {
        List<T> _items;

        internal List<T> Added {get; private set;}
        internal List<T> Removed {get; private set;}

        public bool Dirty
        {
            get
            {
                return this.Added.Count() > 0 || this.Removed.Count() > 0;
            }
        }

        internal ChangeTrackingList()
        {
            _items = new List<T>();
            this.Added = new List<T>();
            this.Removed = new List<T>();
        }

        public void ClearChanges()
        {
            this.Added.Clear();
            this.Removed.Clear();
        }

        public T[] Items
        {
            get
            {
                return _items.ToArray();
            }
            set
            {
                //Currently here for deserialization purposes only
                _items = new List<T>(value);
            }
        }

        internal int Count
        {
            get
            {
                return _items.Count;
            }
        }

        internal T First()
        {
            return _items.First();
        }

        internal T Single()
        {
            return _items.Single();
        }

        internal T Last()
        {
            return _items.Last();
        }

        internal int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        internal IEnumerable<T> GetRange(int index, int count)
        {
            return _items.GetRange(index, count);
        }

        #region mutating methods
        internal void Add(T item)
        {
            _items.Add(item);
            this.Added.Add(item);
        }

        internal void Insert(int index, T item)
        {
            _items.Insert(index, item);
            this.Added.Add(item);
        }

        internal T this [int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                if (!_items[index].Equals(value))
                {
                    this.Removed.Add(_items[index]);
                    _items[index] = value;
                    this.Added.Add(value);
                }
            }
        }

        internal void AddRange(IEnumerable<T> range)
        {
            _items.AddRange(range);
            this.Added.AddRange(range);
        }

        internal void RemoveRange(int index, int count)
        {
            this.Removed.AddRange(_items.GetRange(index, count));
            _items.RemoveRange(index, count);

        }

        internal void RemoveAt(int index)
        {
            this.Removed.Add(_items[index]);
            _items.RemoveAt(index);

        }
        #endregion mutating methods
    }
}

