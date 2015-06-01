using System;
using MarcelloDB.BenchmarkTool.DataClasses;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class EnumerateAll : Base
    {
        int _objectCount ;

        public EnumerateAll(int objectCount)
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
            var currentId = 0;
            foreach (var p in this.Collection.All)
            {
                if (currentId != p.ID)
                {
                    throw new InvalidOperationException(
                        string.Format("Expecting Id to be :{0} instead of {1}",currentId, p.ID));
                }
                currentId ++;
            }
        }
    }
}

