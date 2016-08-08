
using System.Collections.Generic;

namespace MarcelloDB.Index
{

	public class CompoundValue<T1, T2> : CompoundValue{

		public T1 P1{ get; set; } public T2 P2{ get; set; }

        internal CompoundValue(){}

        internal CompoundValue(T1 p1, T2 p2){
        	this.P1 = p1; this.P2 = p2;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2 };
        }
    }

    public abstract partial class CompoundValue {
		internal static CompoundValue<T1, T2> Build<T1, T2>(T1 p1, T2 p2)
        {
            return new CompoundValue<T1, T2>(p1, p2);
        }
    }
		
	public class CompoundValue<T1, T2, T3> : CompoundValue{

		public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; }

        internal CompoundValue(){}

        internal CompoundValue(T1 p1, T2 p2, T3 p3){
        	this.P1 = p1; this.P2 = p2; this.P3 = p3;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3 };
        }
    }

    public abstract partial class CompoundValue {
		internal static CompoundValue<T1, T2, T3> Build<T1, T2, T3>(T1 p1, T2 p2, T3 p3)
        {
            return new CompoundValue<T1, T2, T3>(p1, p2, p3);
        }
    }
		
	public class CompoundValue<T1, T2, T3, T4> : CompoundValue{

		public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; }

        internal CompoundValue(){}

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4){
        	this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4 };
        }
    }

    public abstract partial class CompoundValue {
		internal static CompoundValue<T1, T2, T3, T4> Build<T1, T2, T3, T4>(T1 p1, T2 p2, T3 p3, T4 p4)
        {
            return new CompoundValue<T1, T2, T3, T4>(p1, p2, p3, p4);
        }
    }
		
	public class CompoundValue<T1, T2, T3, T4, T5> : CompoundValue{

		public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; } public T5 P5{ get; set; }

        internal CompoundValue(){}

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5){
        	this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4, this.P5 };
        }
    }

    public abstract partial class CompoundValue {
		internal static CompoundValue<T1, T2, T3, T4, T5> Build<T1, T2, T3, T4, T5>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            return new CompoundValue<T1, T2, T3, T4, T5>(p1, p2, p3, p4, p5);
        }
    }
		
	public class CompoundValue<T1, T2, T3, T4, T5, T6> : CompoundValue{

		public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; } public T5 P5{ get; set; } public T6 P6{ get; set; }

        internal CompoundValue(){}

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6){
        	this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5; this.P6 = p6;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4, this.P5, this.P6 };
        }
    }

    public abstract partial class CompoundValue {
		internal static CompoundValue<T1, T2, T3, T4, T5, T6> Build<T1, T2, T3, T4, T5, T6>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            return new CompoundValue<T1, T2, T3, T4, T5, T6>(p1, p2, p3, p4, p5, p6);
        }
    }
		
	public class CompoundValue<T1, T2, T3, T4, T5, T6, T7> : CompoundValue{

		public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; } public T5 P5{ get; set; } public T6 P6{ get; set; } public T7 P7{ get; set; }

        internal CompoundValue(){}

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7){
        	this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5; this.P6 = p6; this.P7 = p7;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4, this.P5, this.P6, this.P7 };
        }
    }

    public abstract partial class CompoundValue {
		internal static CompoundValue<T1, T2, T3, T4, T5, T6, T7> Build<T1, T2, T3, T4, T5, T6, T7>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            return new CompoundValue<T1, T2, T3, T4, T5, T6, T7>(p1, p2, p3, p4, p5, p6, p7);
        }
    }
		
	public class CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8> : CompoundValue{

		public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; } public T5 P5{ get; set; } public T6 P6{ get; set; } public T7 P7{ get; set; } public T8 P8{ get; set; }

        internal CompoundValue(){}

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8){
        	this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5; this.P6 = p6; this.P7 = p7; this.P8 = p8;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4, this.P5, this.P6, this.P7, this.P8 };
        }
    }

    public abstract partial class CompoundValue {
		internal static CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8> Build<T1, T2, T3, T4, T5, T6, T7, T8>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            return new CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8>(p1, p2, p3, p4, p5, p6, p7, p8);
        }
    }
		}