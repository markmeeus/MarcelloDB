using System;
using System.Collections.Generic;
using MarcelloDB.Index;
using MarcelloDB.Collections;

namespace MarcelloDB.BenchmarkTool.DataClasses
{
    public class PersonIndexes : IndexDefinition<Person>
    {
        public  IndexedValue<Person, string> FirstName { get; set; }
        public  IndexedValue<Person, string> LastName { get; set; }
        public IndexedValue<Person, string> FullName
        {
            get {
                return base.IndexedValue(p => string.Format("{0} {1}", p.FirstName, p.LastName));
            }
        }
    }

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

