using System;
using System.Collections.Generic;

namespace MarcelloDB.BenchmarkTool.DataClasses
{
    public class Person: Base
    {
        static Random _rand = new Random();

        public int ID { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set; }
        public List<Address> Addresses { get; set; }

        public static Person BuildRandom()
        {
            var person = new Person()
            {
                FirstName = "First " + _rand.Next().ToString(),
                LastName =  "Last  " + _rand.Next().ToString(),
                Addresses = new List<Address>()
            };

            var addressCount = _rand.Next() % 4;
            for (int i = 0; i < addressCount; i++)
            {
                person.Addresses.Add(Address.BuildRandom());
            }

            return person;
        }
    }
}

