using System;
using System.Collections.Generic;
using MarcelloDB.Serialization;
using System.Linq;
using MarcelloDB.Index.LazyNode.ConcreteValues;

namespace MarcelloDB.Index.LazyNode
{
    

    internal class LazyChildrenAddresses: IEnumerable<Int64>
    {
        List<LazyValue<Int64>> ChildAddresses { get; set; }

        internal LazyChildrenAddresses()
        {
            this.ChildAddresses = new List<LazyValue<Int64>>();
        }

        internal LazyChildrenAddresses(byte[] bytes, int startIndex, int nrOfItemsInBytes)
        { 
            this.ChildAddresses = new List<LazyValue<Int64>>();
            for (int i = 0; i < nrOfItemsInBytes; i++)
            {
                this.ChildAddresses.Add(new LazyInt64Value(bytes, startIndex + (i * sizeof(Int64))));
            }
        }

        internal void Add(Int64 address)
        {
            this.ChildAddresses.Add(new LazyInt64Value(address));
        }


        internal Int64 this [int index]
        {
            get{ return this.ChildAddresses[index].Value; }

        }                

        internal int Count 
        {
            get
            {
                return this.ChildAddresses.Count;
            }
        }

        internal byte[] ToBytes()
        {
            var bytes = new byte[this.ChildAddresses.Count * sizeof(Int64)];
            var bufferWriter = new BufferWriter(bytes);
            foreach (var address in this.ChildAddresses)
            {
                bufferWriter.WriteInt64(address.Value);
            }
            return bytes;
        }

        #region IEnumerable implementation
        public IEnumerator<long> GetEnumerator()
        {
            foreach (var value in this.ChildAddresses)
            {
                yield return value.Value;
            }
        }
        #endregion
        #region IEnumerable implementation
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }

    internal class LazyNode<T>
    {
        internal int Degree { get; private set;}

        byte[] Bytes { get; set; }

        internal LazyChildrenAddresses ChildrenAddresses { get; set; }

        internal LazyNode(int degree) 
        {
            Initialize(degree, null);
        }

        LazyNode(int degree, byte[] bytes)
        {
            Initialize(degree, bytes);
        }                        

        void Initialize(int degree, byte[] bytes)
        {          
            this.Bytes = bytes;
            this.Degree = degree;

            InitializeChildrenAddresses();

        }

        void InitializeChildrenAddresses()
        {           
            if (this.Bytes != null)
            {
                var reader = new BufferReader(this.Bytes);
                var numberOfChildrenAdresses = reader.ReadInt32();
                this.ChildrenAddresses = new LazyChildrenAddresses(
                    this.Bytes,
                    sizeof(int), 
                    numberOfChildrenAdresses
                );
            }
            else
            {
                this.ChildrenAddresses = new LazyChildrenAddresses();
            }
        }

        internal static LazyNode<T> FromBytes(int degree, byte[] bytes)
        {
            return new LazyNode<T>(degree, bytes);
        }

        internal byte[] ToBytes()
        {
            var bytesAsList = new List<byte[]>();
            //add nr of child addresses
            bytesAsList.Add(
                new BufferWriter(new byte[sizeof(Int32)])
                .WriteInt32((Int32)this.ChildrenAddresses.Count)
                    .GetTrimmedBuffer()
            );

            //write the children addresses
            if (this.ChildrenAddresses.Count > 0)
            {
                bytesAsList.Add(
                    new BufferWriter(new byte[sizeof(Int32)])
                    .WriteBytes(this.ChildrenAddresses.ToBytes())
                    .GetTrimmedBuffer()
                );                    
            }

            var byteCount = bytesAsList.Select(byteFragment => byteFragment.Length).Sum();
            var bytes = new byte[byteCount];
            var writer = new BufferWriter(bytes);
            foreach(var byteFragment in bytesAsList)
            {
                writer.WriteBytes(byteFragment);
            }
            return writer.GetTrimmedBuffer();
        }
    }
}

