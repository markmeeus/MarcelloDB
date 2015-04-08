using System;

namespace MarcelloDB.Index
{

    public class Entry<TK, TP> : IEquatable<Entry<TK, TP>>
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

        public TP Pointer { get; set; }

        public bool Equals(Entry<TK, TP> other)
        {
            return this.Key.Equals(other.Key) && this.Pointer.Equals(other.Pointer);
        }
    }
}

