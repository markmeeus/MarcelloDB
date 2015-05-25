using System;
using MarcelloDB.BenchmarkTool.DataClasses;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class RandomRead : Base
    {
        int _objectCount ;

        public RandomRead(int objectCount)
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
            base.OnSetup();
        }

        protected override void OnRun()
        {
            var r = new Random();
            for (int i = 0; i < _objectCount; i++)
            {
                var o = this.Collection.Find(r.Next(_objectCount));
                if(o == null){
                    throw new Exception("Object with Id " + i.ToString() + " not found");
                }
            }
        }
    }
}

