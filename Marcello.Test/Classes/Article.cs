using System;

namespace Marcello.Test
{
    public class Article
    {
        public Article ()
        {
        }

        public string Name{ get; set;}       

        public static Article ToiletPaper{
            get{ return new Article (){ Name = "Toilet Paper" }; }  
        }

        public static Article SpinalTapDvd{
            get{ return new Article (){ Name = "DVD: This is Spinal Tap" }; }  
        }

        public static Article BarbieDoll{
            get{ return new Article (){ Name = "Barbie Doll" }; }  
        }
    }
}

