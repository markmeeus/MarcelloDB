using System;

namespace MarcelloDB.Index
{
    internal class Entry<TK> : IEquatable<Entry<TK>>
    {
        TK _key;
        public TK Key
        {
            get { return _key; }
            set
            {
                if (value == null)
                {
                    throw new Exception("PANIC: Entry keys cannot be null");
                }
                _key = value;
            }
        }

        public Int64 Pointer { get; set; }

        public bool Equals(Entry<TK> other)
        {
            return this.Key.Equals(other.Key) && this.Pointer.Equals(other.Pointer);
        }
    }
}

