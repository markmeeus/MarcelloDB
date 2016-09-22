
using System.Linq;
using System.Collections.Generic;

namespace MarcelloDB.Index
{
    public class CompoundValue<T1> : CompoundValue{

        public int ConstructedWithCount { get; set; }

        public T1 P1{ get; set; }

        internal CompoundValue(){
            this.ConstructedWithCount = 0;
        }

        internal CompoundValue(T1 p1){
            this.P1 = p1;
            this.ConstructedWithCount = 1;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1 }.Take(this.ConstructedWithCount);
        }
    }

    public class CompoundValue<T1, T2> : CompoundValue{

        public int ConstructedWithCount { get; set; }

        public T1 P1{ get; set; } public T2 P2{ get; set; }

        internal CompoundValue(){
            this.ConstructedWithCount = 0;
        }

        internal CompoundValue(T1 p1){
            this.P1 = p1;
            this.ConstructedWithCount = 1;
        }

        internal CompoundValue(T1 p1, T2 p2){
            this.P1 = p1; this.P2 = p2;
            this.ConstructedWithCount = 2;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2 }.Take(this.ConstructedWithCount);
        }
    }

    public class CompoundValue<T1, T2, T3> : CompoundValue{

        public int ConstructedWithCount { get; set; }

        public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; }

        internal CompoundValue(){
            this.ConstructedWithCount = 0;
        }

        internal CompoundValue(T1 p1){
            this.P1 = p1;
            this.ConstructedWithCount = 1;
        }

        internal CompoundValue(T1 p1, T2 p2){
            this.P1 = p1; this.P2 = p2;
            this.ConstructedWithCount = 2;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3){
            this.P1 = p1; this.P2 = p2; this.P3 = p3;
            this.ConstructedWithCount = 3;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3 }.Take(this.ConstructedWithCount);
        }
    }

    public class CompoundValue<T1, T2, T3, T4> : CompoundValue{

        public int ConstructedWithCount { get; set; }

        public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; }

        internal CompoundValue(){
            this.ConstructedWithCount = 0;
        }

        internal CompoundValue(T1 p1){
            this.P1 = p1;
            this.ConstructedWithCount = 1;
        }

        internal CompoundValue(T1 p1, T2 p2){
            this.P1 = p1; this.P2 = p2;
            this.ConstructedWithCount = 2;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3){
            this.P1 = p1; this.P2 = p2; this.P3 = p3;
            this.ConstructedWithCount = 3;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4;
            this.ConstructedWithCount = 4;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4 }.Take(this.ConstructedWithCount);
        }
    }

    public class CompoundValue<T1, T2, T3, T4, T5> : CompoundValue{

        public int ConstructedWithCount { get; set; }

        public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; } public T5 P5{ get; set; }

        internal CompoundValue(){
            this.ConstructedWithCount = 0;
        }

        internal CompoundValue(T1 p1){
            this.P1 = p1;
            this.ConstructedWithCount = 1;
        }

        internal CompoundValue(T1 p1, T2 p2){
            this.P1 = p1; this.P2 = p2;
            this.ConstructedWithCount = 2;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3){
            this.P1 = p1; this.P2 = p2; this.P3 = p3;
            this.ConstructedWithCount = 3;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4;
            this.ConstructedWithCount = 4;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5;
            this.ConstructedWithCount = 5;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4, this.P5 }.Take(this.ConstructedWithCount);
        }
    }

    public class CompoundValue<T1, T2, T3, T4, T5, T6> : CompoundValue{

        public int ConstructedWithCount { get; set; }

        public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; } public T5 P5{ get; set; } public T6 P6{ get; set; }

        internal CompoundValue(){
            this.ConstructedWithCount = 0;
        }

        internal CompoundValue(T1 p1){
            this.P1 = p1;
            this.ConstructedWithCount = 1;
        }

        internal CompoundValue(T1 p1, T2 p2){
            this.P1 = p1; this.P2 = p2;
            this.ConstructedWithCount = 2;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3){
            this.P1 = p1; this.P2 = p2; this.P3 = p3;
            this.ConstructedWithCount = 3;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4;
            this.ConstructedWithCount = 4;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5;
            this.ConstructedWithCount = 5;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5; this.P6 = p6;
            this.ConstructedWithCount = 6;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4, this.P5, this.P6 }.Take(this.ConstructedWithCount);
        }
    }

    public class CompoundValue<T1, T2, T3, T4, T5, T6, T7> : CompoundValue{

        public int ConstructedWithCount { get; set; }

        public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; } public T5 P5{ get; set; } public T6 P6{ get; set; } public T7 P7{ get; set; }

        internal CompoundValue(){
            this.ConstructedWithCount = 0;
        }

        internal CompoundValue(T1 p1){
            this.P1 = p1;
            this.ConstructedWithCount = 1;
        }

        internal CompoundValue(T1 p1, T2 p2){
            this.P1 = p1; this.P2 = p2;
            this.ConstructedWithCount = 2;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3){
            this.P1 = p1; this.P2 = p2; this.P3 = p3;
            this.ConstructedWithCount = 3;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4;
            this.ConstructedWithCount = 4;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5;
            this.ConstructedWithCount = 5;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5; this.P6 = p6;
            this.ConstructedWithCount = 6;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5; this.P6 = p6; this.P7 = p7;
            this.ConstructedWithCount = 7;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4, this.P5, this.P6, this.P7 }.Take(this.ConstructedWithCount);
        }
    }

    public class CompoundValue<T1, T2, T3, T4, T5, T6, T7, T8> : CompoundValue{

        public int ConstructedWithCount { get; set; }

        public T1 P1{ get; set; } public T2 P2{ get; set; } public T3 P3{ get; set; } public T4 P4{ get; set; } public T5 P5{ get; set; } public T6 P6{ get; set; } public T7 P7{ get; set; } public T8 P8{ get; set; }

        internal CompoundValue(){
            this.ConstructedWithCount = 0;
        }

        internal CompoundValue(T1 p1){
            this.P1 = p1;
            this.ConstructedWithCount = 1;
        }

        internal CompoundValue(T1 p1, T2 p2){
            this.P1 = p1; this.P2 = p2;
            this.ConstructedWithCount = 2;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3){
            this.P1 = p1; this.P2 = p2; this.P3 = p3;
            this.ConstructedWithCount = 3;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4;
            this.ConstructedWithCount = 4;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5;
            this.ConstructedWithCount = 5;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5; this.P6 = p6;
            this.ConstructedWithCount = 6;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5; this.P6 = p6; this.P7 = p7;
            this.ConstructedWithCount = 7;
        }

        internal CompoundValue(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8){
            this.P1 = p1; this.P2 = p2; this.P3 = p3; this.P4 = p4; this.P5 = p5; this.P6 = p6; this.P7 = p7; this.P8 = p8;
            this.ConstructedWithCount = 8;
        }

        internal override IEnumerable<object> GetValues()
        {
            return new object[]{ this.P1, this.P2, this.P3, this.P4, this.P5, this.P6, this.P7, this.P8 }.Take(this.ConstructedWithCount);
        }
    }

}