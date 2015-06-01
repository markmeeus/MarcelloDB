using System;
using MarcelloDB.BenchmarkTool.Benchmarks;

namespace MarcelloDB.BenchmarkTool
{
    class MainClass
    {
        public static void Main(string[] args)
        {              
            TimeSpan elapsed;
            foreach (var objectCount in new int[]{500, 1000, 2000, 4000, 8000})
            {
                elapsed = new SequentialIDsBulkInsert(objectCount).Run();
                Console.WriteLine(string.Format("SequentialIDsBulkInsert {0} took: {1} ms", objectCount, elapsed.TotalMilliseconds));
            }

            foreach (var objectCount in new int[]{500, 1000, 2000, 4000, 8000})
            {
                elapsed = new RandomIDsBulkInsert(objectCount).Run();
                Console.WriteLine(string.Format("RandomIDsBulkInsert {0} took: {1} ms", objectCount, elapsed.TotalMilliseconds));
            }

            foreach (var objectCount in new int[]{500, 1000, 2000, 4000, 8000})
            {
                elapsed = new RandomRead(objectCount).Run();
                Console.WriteLine(string.Format("RandomRead {0} took: {1} ms", objectCount, elapsed.TotalMilliseconds));
            }

            foreach (var objectCount in new int[]{500, 1000, 2000, 4000, 8000})
            {
                elapsed = new EnumerateAll(objectCount).Run();
                Console.WriteLine(string.Format("EnumerateAll {0} took: {1} ms", objectCount, elapsed.TotalMilliseconds));
            }

            int large = 100 * 1000;
            elapsed = new SequentialIDsBulkInsert(large).Run();
            Console.WriteLine(string.Format("SequentialIDsBulkInsert {0} took: {1} ms", large, elapsed.TotalMilliseconds));

            elapsed = new RandomIDsBulkInsert(large).Run();
            Console.WriteLine(string.Format("RandomIDsBulkInsert {0} took: {1} ms", large, elapsed.TotalMilliseconds));

            elapsed = new RandomRead(large).Run();
            Console.WriteLine(string.Format("RandomRead {0} took: {1} ms", large, elapsed.TotalMilliseconds));

            elapsed = new EnumerateAll(large).Run();
            Console.WriteLine(string.Format("EnumerateAll {0} took: {1} ms", large, elapsed.TotalMilliseconds));

        }
    }
}
