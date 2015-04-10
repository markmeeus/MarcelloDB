using System;

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
                var a = new TestClass{ID = r.Next(), Name = "Object " + i.ToString()};
                this.Collection.Persist(a);
            }
        }
    }
}

