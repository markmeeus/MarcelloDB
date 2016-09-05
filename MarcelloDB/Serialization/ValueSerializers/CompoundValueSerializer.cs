
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using MarcelloDB.Index;

namespace MarcelloDB.Serialization.ValueSerializers
{
    static class CompoundValueSerializer
    {
    	public static ConstructorInfo GetGenericConstructorWithTypes(Type[] valueTypes)
        {
        	switch(valueTypes.Length){
        	case 1:
	            return typeof(CompoundValueSerializer<>)
	                .GetTypeInfo()
	                .MakeGenericType(valueTypes)
	                .GetTypeInfo()
	                .DeclaredConstructors.First();
        	case 2:
	            return typeof(CompoundValueSerializer<,>)
	                .GetTypeInfo()
	                .MakeGenericType(valueTypes)
	                .GetTypeInfo()
	                .DeclaredConstructors.First();
        	case 3:
	            return typeof(CompoundValueSerializer<,,>)
	                .GetTypeInfo()
	                .MakeGenericType(valueTypes)
	                .GetTypeInfo()
	                .DeclaredConstructors.First();
        	case 4:
	            return typeof(CompoundValueSerializer<,,,>)
	                .GetTypeInfo()
	                .MakeGenericType(valueTypes)
	                .GetTypeInfo()
	                .DeclaredConstructors.First();
        	case 5:
	            return typeof(CompoundValueSerializer<,,,,>)
	                .GetTypeInfo()
	                .MakeGenericType(valueTypes)
	                .GetTypeInfo()
	                .DeclaredConstructors.First();
        	case 6:
	            return typeof(CompoundValueSerializer<,,,,,>)
	                .GetTypeInfo()
	                .MakeGenericType(valueTypes)
	                .GetTypeInfo()
	                .DeclaredConstructors.First();
        	case 7:
	            return typeof(CompoundValueSerializer<,,,,,,>)
	                .GetTypeInfo()
	                .MakeGenericType(valueTypes)
	                .GetTypeInfo()
	                .DeclaredConstructors.First();
        	case 8:
	            return typeof(CompoundValueSerializer<,,,,,,,>)
	                .GetTypeInfo()
	                .MakeGenericType(valueTypes)
	                .GetTypeInfo()
	                .DeclaredConstructors.First();
			default:
				throw new InvalidOperationException("Compound values support upto 8 values");
			}
		}
    }

    internal class CompoundValueSerializer<T1> : ValueSerializer<CompoundValue<T1>>
    {
        List<ValueSerializer> Serializers { get; set; }

        public CompoundValueSerializer(IEnumerable<ValueSerializer> serializers)
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
            return new CompoundValue<T1>(
            	((ValueSerializer<T1>)this.Serializers[0]).ReadValue(formatter)
            );
        }

        #endregion
    }

    internal class CompoundValueSerializer<T1, T2> : ValueSerializer<CompoundValue<T1, T2>>
    {
        List<ValueSerializer> Serializers { get; set; }

        public CompoundValueSerializer(IEnumerable<ValueSerializer> serializers)
        {
            this.Serializers = serializers.ToList();
        }

        #region implemented abstract members of ValueSerializer

        internal override void WriteValue(BinaryFormatter formatter, CompoundValue<T1, T2> value)
        {
            ((ValueSerializer<T1>)Serializers[0]).WriteValue(formatter, value.P1);
			((ValueSerializer<T2>)Serializers[1]).WriteValue(formatter, value.P2);
        }

        internal override CompoundValue<T1, T2> ReadValue(BinaryFormatter formatter)
        {
            return new CompoundValue<T1, T2>(
            	((ValueSerializer<T1>)this.Serializers[0]).ReadValue(formatter),
				((ValueSerializer<T2>)this.Serializers[1]).ReadValue(formatter)
            );
        }

        #endregion
    }

    internal class CompoundValueSerializer<T1, T2, T3> : ValueSerializer<CompoundValue<T1, T2, T3>>
    {
        List<ValueSerializer> Serializers { get; set; }

        public CompoundValueSerializer(IEnumerable<ValueSerializer> serializers)
        {
            this.Serializers = serializers.ToList();
        }

        #region implemented abstract members of ValueSerializer

        internal override void WriteValue(BinaryFormatter formatter, CompoundValue<T1, T2, T3> value)
        {
            ((ValueSerializer<T1>)Serializers[0]).WriteValue(formatter, value.P1);
			((ValueSerializer<T2>)Serializers[1]).WriteValue(formatter, value.P2);
			((ValueSerializer<T3>)Serializers[2]).WriteValue(formatter, value.P3);
        }

        internal override CompoundValue<T1, T2, T3> ReadValue(BinaryFormatter formatter)
        {
            return new CompoundValue<T1, T2, T3>(
            	((ValueSerializer<T1>)this.Serializers[0]).ReadValue(formatter),
				((ValueSerializer<T2>)this.Serializers[1]).ReadValue(formatter),
				((ValueSerializer<T3>)this.Serializers[2]).ReadValue(formatter)
            );
        }

        #endregion
    }

    internal class CompoundValueSerializer<T1, T2, T3, T4> : ValueSerializer<CompoundValue<T1, T2, T3, T4>>
    {
        List<ValueSerializer> Serializers { get; set; }

        public CompoundValueSerializer(IEnumerable<ValueSerializer> serializers)
        {
            this.Serializers = serializers.ToList();
        }

        #region implemented abstract members of ValueSerializer

        internal override void WriteValue(BinaryFormatter formatter, CompoundValue<T1, T2, T3, T4> value)
        {
            ((ValueSerializer<T1>)Serializers[0]).WriteValue(formatter, value.P1);
			((ValueSerializer<T2>)Serializers[1]).WriteValue(formatter, value.P2);
			((ValueSerializer<T3>)Serializers[2]).WriteValue(formatter, value.P3);
			((ValueSerializer<T4>)Serializers[3]).WriteValue(formatter, value.P4);
        }

        internal override CompoundValue<T1, T2, T3, T4> ReadValue(BinaryFormatter formatter)
        {
            return new CompoundValue<T1, T2, T3, T4>(
            	((ValueSerializer<T1>)this.Serializers[0]).ReadValue(formatter),
				((ValueSerializer<T2>)this.Serializers[1]).ReadValue(formatter),
				((ValueSerializer<T3>)this.Serializers[2]).ReadValue(formatter),
				((ValueSerializer<T4>)this.Serializers[3]).ReadValue(formatter)
            );
        }

        #endregion
    }

    internal class CompoundValueSerializer<T1, T2, T3, T4, T5> : ValueSerializer<CompoundValue<T1, T2, T3, T4, T5>>
    {
        List<ValueSerializer> Serializers { get; set; }

        public CompoundValueSerializer(IEnumerable<ValueSerializer> serializers)
        {
            this.Serializers = serializers.ToList();
        }

        #region implemented abstract members of ValueSerializer

        internal override void WriteValue(BinaryFormatter formatter, CompoundValue<T1, T2, T3, T4, T5> value)
        {
            ((ValueSerializer<T1>)Serializers[0]).WriteValue(formatter, value.P1);
			((ValueSerializer<T2>)Serializers[1]).WriteValue(formatter, value.P2);
			((ValueSerializer<T3>)Serializers[2]).WriteValue(formatter, value.P3);
			((ValueSerializer<T4>)Serializers[3]).WriteValue(formatter, value.P4);
			((ValueSerializer<T5>)Serializers[4]).WriteValue(formatter, value.P5);
        }

        internal override CompoundValue<T1, T2, T3, T4, T5> ReadValue(BinaryFormatter formatter)
        {
            return new CompoundValue<T1, T2, T3, T4, T5>(
            	((ValueSerializer<T1>)this.Serializers[0]).ReadValue(formatter),
				((ValueSerializer<T2>)this.Serializers[1]).ReadValue(formatter),
				((ValueSerializer<T3>)this.Serializers[2]).ReadValue(formatter),
				((ValueSerializer<T4>)this.Serializers[3]).ReadValue(formatter),
				((ValueSerializer<T5>)this.Serializers[4]).ReadValue(formatter)
            );
        }

        #endregion
    }

    internal class CompoundValueSerializer<T1, T2, T3, T4, T5, T6> : ValueSerializer<CompoundValue<T1, T2, T3, T4, T5, T6>>
    {
        List<ValueSerializer> Serializers { get; set; }

        public CompoundValueSerializer(IEnumerable<ValueSerializer> serializers)
        {
            this.Serializers = serializers.ToList();
        }

        #region implemented abstract members of ValueSerializer

        internal override void WriteValue(BinaryFormatter formatter, CompoundValue<T1, T2, T3, T4, T5, T6> value)
        {
            ((ValueSerializer<T1>)Serializers[0]).WriteValue(formatter, value.P1);
			((ValueSerializer<T2>)Serializers[1]).WriteValue(formatter, value.P2);
			((ValueSerializer<T3>)Serializers[2]).WriteValue(formatter, value.P3);
			((ValueSerializer<T4>)Serializers[3]).WriteValue(formatter, value.P4);
			((ValueSerializer<T5>)Serializers[4]).WriteValue(formatter, value.P5);
			((ValueSerializer<T6>)Serializers[5]).WriteValue(formatter, value.P6);
        }

        internal override CompoundValue<T1, T2, T3, T4, T5, T6> ReadValue(BinaryFormatter formatter)
        {
            return new CompoundValue<T1, T2, T3, T4, T5, T6>(
            	((ValueSerializer<T1>)this.Serializers[0]).ReadValue(formatter),
				((ValueSerializer<T2>)this.Serializers[1]).ReadValue(formatter),
				((ValueSerializer<T3>)this.Serializers[2]).ReadValue(formatter),
				((ValueSerializer<T4>)this.Serializers[3]).ReadValue(formatter),
				((ValueSerializer<T5>)this.Serializers[4]).ReadValue(formatter),
				((ValueSerializer<T6>)this.Serializers[5]).ReadValue(formatter)
            );
        }

        #endregion
    }

    internal class CompoundValueSerializer<T1, T2, T3, T4, T5, T6, T7> : ValueSerializer<CompoundValue<T1, T2, T3, T4, T5, T6, T7>>
    {
        List<ValueSerializer> Serializers { get; set; }

        public CompoundValueSerializer(IEnumerable<ValueSerializer> serializers)
        {
            this.Serializers = serializers.ToList();
        }

        #region implemented abstract members of ValueSerializer

        internal override void WriteValue(BinaryFormatter formatter, CompoundValue<T1, T2, T3, T4, T5, T6, T7> value)
        {
            ((ValueSerializer<T1>)Serializers[0]).WriteValue(formatter, value.P1);
			((ValueSerializer<T2>)Serializers[1]).WriteValue(formatter, value.P2);
			((ValueSerializer<T3>)Serializers[2]).WriteValue(formatter, value.P3);
			((ValueSerializer<T4>)Serializers[3]).WriteValue(formatter, value.P4);
			((ValueSerializer<T5>)Serializers[4]).WriteValue(formatter, value.P5);
			((ValueSerializer<T6>)Serializers[5]).WriteValue(formatter, value.P6);
			((ValueSerializer<T7>)Serializers[6]).WriteValue(formatter, value.P7);
        }

        internal override CompoundValue<T1, T2, T3, T4, T5, T6, T7> ReadValue(BinaryFormatter formatter)
        {
            return new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(
            	((ValueSerializer<T1>)this.Serializers[0]).ReadValue(formatter),
				((ValueSerializer<T2>)this.Serializers[1]).ReadValue(formatter),
				((ValueSerializer<T3>)this.Serializers[2]).ReadValue(formatter),
				((ValueSerializer<T4>)this.Serializers[3]).ReadValue(formatter),
				((ValueSerializer<T5>)this.Serializers[4]).ReadValue(formatter),
				((ValueSerializer<T6>)this.Serializers[5]).ReadValue(formatter),
				((ValueSerializer<T7>)this.Serializers[6]).ReadValue(formatter)
            );
        }

        #endregion
    }

    internal class CompoundValueSerializer<T1, T2, T3, T4, T5, T6, T7, T8> : ValueSerializer<CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>>
    {
        List<ValueSerializer> Serializers { get; set; }

        public CompoundValueSerializer(IEnumerable<ValueSerializer> serializers)
        {
            this.Serializers = serializers.ToList();
        }

        #region implemented abstract members of ValueSerializer

        internal override void WriteValue(BinaryFormatter formatter, CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8> value)
        {
            ((ValueSerializer<T1>)Serializers[0]).WriteValue(formatter, value.P1);
			((ValueSerializer<T2>)Serializers[1]).WriteValue(formatter, value.P2);
			((ValueSerializer<T3>)Serializers[2]).WriteValue(formatter, value.P3);
			((ValueSerializer<T4>)Serializers[3]).WriteValue(formatter, value.P4);
			((ValueSerializer<T5>)Serializers[4]).WriteValue(formatter, value.P5);
			((ValueSerializer<T6>)Serializers[5]).WriteValue(formatter, value.P6);
			((ValueSerializer<T7>)Serializers[6]).WriteValue(formatter, value.P7);
			((ValueSerializer<T8>)Serializers[7]).WriteValue(formatter, value.P8);
        }

        internal override CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8> ReadValue(BinaryFormatter formatter)
        {
            return new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(
            	((ValueSerializer<T1>)this.Serializers[0]).ReadValue(formatter),
				((ValueSerializer<T2>)this.Serializers[1]).ReadValue(formatter),
				((ValueSerializer<T3>)this.Serializers[2]).ReadValue(formatter),
				((ValueSerializer<T4>)this.Serializers[3]).ReadValue(formatter),
				((ValueSerializer<T5>)this.Serializers[4]).ReadValue(formatter),
				((ValueSerializer<T6>)this.Serializers[5]).ReadValue(formatter),
				((ValueSerializer<T7>)this.Serializers[6]).ReadValue(formatter),
				((ValueSerializer<T8>)this.Serializers[7]).ReadValue(formatter)
            );
        }

        #endregion
    }


}