using System;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using MarcelloDB.Index.LazyNode.ConcreteValues;

namespace MarcelloDB
{
    internal class LazyValueList<T> : IEnumerable<T>
    {
        bool _valuesLoaded = false;

        List<LazyValue<T>> Values = new List<LazyValue<T>>();

        byte[] Bytes { get; set; }

        int FirstByteIntex { get; set; }

        internal LazyValueList(byte[] bytes, int firstByteIndex)
        {
            this.Bytes = bytes;
            this.FirstByteIntex = firstByteIndex;
        }

        #region IEnumerable implementation

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            LoadValuesIfNecessary();
            foreach (var value in this.Values)
            {
                yield return value.Value;
            }
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        #endregion

        void LoadValuesIfNecessary()
        {
            if (!_valuesLoaded)
            {
                var bufferReader = new BufferReader(this.Bytes);
                bufferReader.MoveTo(this.FirstByteIntex);
                var nrOfItems = bufferReader.ReadInt32();
                var itemStartByte = this.FirstByteIntex + sizeof(Int32);
                for (int i = 0; i < nrOfItems; i++)
                {
                    var value = ConstructValue(itemStartByte);
                    this.Values.Add(value);
                    itemStartByte += value.ByteSize;
                }
            }
        }

        LazyValue<T> ConstructValue(int firstByteIndex)
        {
            if (typeof(T) == typeof(Int32))
            {
                return (LazyValue<T>)(object)new LazyInt32Value(this.Bytes, firstByteIndex);
            }
            if (typeof(T) == typeof(Int64))
            {
                return (LazyValue<T>)(object)new LazyInt64Value(this.Bytes, firstByteIndex);
            }

            throw new NotImplementedException();
        }
    }
}

