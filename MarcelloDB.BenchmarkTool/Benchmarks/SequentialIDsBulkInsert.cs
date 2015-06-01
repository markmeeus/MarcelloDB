using System;
using System.Diagnostics;
using MarcelloDB.BenchmarkTool.DataClasses;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class SequentialIDsBulkInsert : Base
    {
       
        int _objectCount;

        public SequentialIDsBulkInsert(int objectCount)
        {
            _objectCount = objectCount;
        }
            
        protected override void OnRun()
        {
            for (int i = 1; i < _objectCount; i++)
            {
                var person = Person.BuildRandom();
                person.ID = i;
                this.Collection.Persist(person);
            }
        }               
    }        
}




