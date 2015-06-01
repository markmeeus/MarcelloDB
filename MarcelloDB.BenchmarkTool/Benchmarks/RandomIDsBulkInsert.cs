using System;
using MarcelloDB.BenchmarkTool.DataClasses;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class RandomIDsBulkInsert : Base
    {
        int _objectCount;

        public RandomIDsBulkInsert(int objectCount)
        {
            _objectCount = objectCount;
        }

        protected override void OnRun()
        {
            var r = new Random();
            for (int i = 1; i < _objectCount; i++)
            {
                var person = Person.BuildRandom();
                person.ID = r.Next();
                this.Collection.Persist(person);
            }
        }
    }
}

