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
    }
}

