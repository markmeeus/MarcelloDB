using System;

namespace MarcelloDB.BenchmarkTool.DataClasses
{
    public class Address
    {
        static Random _random = new Random();

        public string Street { get; set; }
        public string Number { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public static Address BuildRandom() 
        {
            return new Address{
                Street = "Street " + _random.Next().ToString(),
                Number = "Number " + _random.Next().ToString(),
                ZipCode = _random.Next() % 10000,
                City = "City " + _random.Next().ToString(),
                Country = "Country " + _random.Next().ToString()
            };
        }
    }
}

