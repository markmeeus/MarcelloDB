using System;
using MarcelloDB.Index;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MarcelloDB.Serialization.ValueSerializers
{    


    internal class _CompoundValueSerializer<T1> : ValueSerializer<CompoundValue<T1>>
    {
        List<ValueSerializer> Serializers { get; set; }

        public _CompoundValueSerializer(IEnumerable<ValueSerializer> serializers)
        {
            this.Serializers = serializers.ToList();
        }

        #region implemented abstract members of ValueSerializer

        internal override void WriteValue(BinaryFormatter formatter, CompoundValue<T1> value)
        {
            ((ValueSerializer<T1>)Serializers[0]).WriteValue(formatter, value.P1);
        }

        internal override CompoundValue<T1> ReadValue(BinaryFormatter formatter)
        {
            return CompoundValue.Build(
                ((ValueSerializer<T1>)this.Serializers[0]).ReadValue(formatter)
            );
        }            

        #endregion
    }


}

