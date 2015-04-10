using System;
using MarcelloDB.BenchmarkTool.Benchmarks;

namespace MarcelloDB.BenchmarkTool
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var elapsed = new SequentialIDsBulkInsert(1000).Run();
            Console.WriteLine(string.Format("SequentialIDsBulkInsert took: {0} ms", elapsed.TotalMilliseconds));
        }
    }
}
