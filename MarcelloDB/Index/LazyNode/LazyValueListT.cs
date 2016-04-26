using System;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using MarcelloDB.Index.LazyNode.ConcreteValues;
using System.Linq;

namespace MarcelloDB
{
    internal class LazyValueList<T> : IEnumerable<T>
    {
        bool _valuesLoaded = false;

        List<LazyValue<T>> Values = new List<LazyValue<T>>();

        byte[] Bytes { get; set; }

        int FirstByteIntex { get; set; }

        internal LazyValueList()
        {
            _valuesLoaded = true;
        }

        internal LazyValueList(byte[] bytes, int firstByteIndex)
        {
            this.Bytes = bytes;
            this.FirstByteIntex = firstByteIndex;
        }

        internal byte[] ToBytes()
        {
            var valueBytes = new List<byte[]>();
            foreach (var value in this.Values)
            {
                valueBytes.Add(value.ToBytes());
            }

            var bufferSize = sizeof(Int32) //nr of items
                + valueBytes.Sum(b => b.Length);

            var writer = new BufferWriter(new byte[bufferSize]);
            writer.WriteInt32(valueBytes.Count);

            foreach (var bytes in valueBytes)
            {
                writer.WriteBytes(bytes);
            }

            return writer.GetTrimmedBuffer();
        }

        internal void Add(T item)
        {
            this.Values.Add( LazyValue<T>.ConstructValue((dynamic)item));
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
                    var value = LazyValue<T>.ConstructValue(this.Bytes, itemStartByte);
                    this.Values.Add(value);
                    itemStartByte += value.ByteSize;
                }
            }
        }
    }
}

