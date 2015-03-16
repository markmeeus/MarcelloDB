using System;

namespace MarcelloDB.Test.Classes
{
    public class Article
    {
        public Article ()
        {
        }

        public string Name{ get; set;}       
        public int ID{ get; set;}       

        public static Article ToiletPaper
        {
            get{ return new Article(){ ID=1, Name = "Toilet Paper" }; }  
        }

        public static Article SpinalTapDvd
        {
            get{ return new Article(){ ID=2, Name = "DVD: This is Spinal Tap" }; }  
        }

        public static Article BarbieDoll
        {
            get{ return new Article(){ ID=3, Name = "Barbie Doll" }; }  
        }
    }
}

