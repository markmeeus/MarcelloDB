using System;
using System.Collections.Generic;
using MarcelloDB.BenchmarkTool.DataClasses;
using System.Linq;
using MarcelloDB.BenchmarkTool.Extensions;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class RandomDestroy : Base
    {
        int _objectCount ;

        List<Person> _toDestroy;

        public RandomDestroy(int objectCount)
        {
            _objectCount = objectCount;
        }

        protected override void OnSetup()
        {
            for (int i = 0; i < _objectCount; i++)
            {
                var person = Person.BuildRandom();
                person.ID = i;
                this.Collection.Persist(person);
            }

            _toDestroy = this.Collection.All.ToList();
            _toDestroy.Shuffle();

            base.OnSetup();
        }

        protected override void OnRun()
        {

            foreach (var p in _toDestroy)
            {
                this.Collection.Destroy(p.ID);
            }
        }
    }
}

