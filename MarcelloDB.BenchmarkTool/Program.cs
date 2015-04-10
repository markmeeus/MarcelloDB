using System;
using MarcelloDB.BenchmarkTool.Benchmarks;

namespace MarcelloDB.BenchmarkTool
{
    class MainClass
    {
        public static void Main(string[] args)
        {   
            foreach (var objectCount in new int[]{500, 1000, 2000, 4000})
            {
                var elapsed = new SequentialIDsBulkInsert(objectCount).Run();
                Console.WriteLine(string.Format("SequentialIDsBulkInsert {0} took: {1} ms", objectCount, elapsed.TotalMilliseconds));

            }
        }
    }
}
