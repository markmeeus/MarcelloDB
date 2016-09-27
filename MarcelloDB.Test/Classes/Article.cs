using System;

namespace MarcelloDB.Test.Classes
{
    public class Article
    {
        public Article ()
        {
            this.Reference = Guid.NewGuid().ToString();
        }

        public string Name{ get; set;}

        public string Description { get; set; }

        public int ID { get; set;}

        public string Code { get; set; }

        public string Category { get; set; }

        public string Reference { get; set; }

        public static Article ToiletPaper
        {
            get
            {
                return new Article(){
                    ID = 1,
                    Code = "001",
                    Name = "Toilet Paper",
                    Description = "The finest paper money can buy",
                    Category = "Hygiene",
                    Reference = "TLT001"
                };
            }
        }

        public static Article SpinalTapDvd
        {
            get
            {
                return new Article(){
                    ID = 2,
                    Code = "002",
                    Name = "DVD: This is Spinal Tap",
                    Description = "Best dvd ever",
                    Category = "Entertainment",
                    Reference = "DVD002"
                };
            }
        }

        public static Article BarbieDoll
        {
            get
            {
                return new Article(){
                    ID = 3,
                    Code = "003",
                    Name = "Barbie Doll",
                    Description = "Some doll",
                    Category = "Toys",
                    Reference = "TOY003"
                };
            }
        }
    }

    public class Food : Article
    {
        public DateTime Expires {get;set;}


        public static Food Bread
        {
            get
            {
                return new Food{
                    ID = 4,
                    Code = "004",
                    Expires = DateTime.Now.AddDays(2),
                    Name = "Bread",
                    Description = "White Bread",
                    Category = "Bread"
                };
            }
        }
    }
}

