using System;
using MarcelloDB.Serialization;

namespace MarcelloDB.Index.LazyNode.ConcreteValues
{
    internal class LazyGuidValue : LazyValue<Guid>
    {
        internal LazyGuidValue(Guid value) : base(value)
        {
        }

        internal LazyGuidValue(byte[] bytes, int firstByteIndex) : base(bytes, firstByteIndex)
        {
        }

        protected override Guid LoadValue(int valueIndex)
        {
            BufferReader reader = new BufferReader(this.Bytes);
            reader.MoveTo(valueIndex);
            var bytes = reader.ReadBytes(16);
            return new Guid(bytes);
        }

        internal override byte GetTypeID()
        {
            return TYPEID_GUID;
        }

        internal override byte[] GetValueAsBytes()
        {
            var writer = new BufferWriter(new byte[16]);

            return writer
                .WriteBytes(this.Value.ToByteArray())
                .GetTrimmedBuffer();
        }

        #region implemented abstract members of LazyValue

        protected override int GetValueByteSize()
        {
            return 16;
        }

        #endregion
    }
}

