using System;

namespace MarcelloDB.Test
{
    public class Location
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public static Location Harrods
        {
            get
            {
                return new Location
                {
                    ID = "1a1a",
                    Name = "Harrods",
                    Address = "87-135 Brompton Road, Knightsbridge SW1X 7XL"
                };
            }
        }

        public static Location MandS
        {
            get
            {
                return new Location
                {
                    ID = "2b2b",
                    Name = "Marks & Spencer",
                    Address = "458 Oxford Street, W1C 1AP"
                };
            }
        }
    }
}

