using System;
using MarcelloDB.BenchmarkTool.DataClasses;
using System.Linq;
using System.Collections.Generic;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class SequentialDestroy : Base
    {
        int _objectCount ;

        List<Person> _toDestroy;

        public SequentialDestroy(int objectCount)
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

