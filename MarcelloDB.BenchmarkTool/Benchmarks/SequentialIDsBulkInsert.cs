using System;
using System.Diagnostics;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class SequentialIDsBulkInsert
    {
        public class TestClass
        {
            public int ID { get; set; }
            public string Name { get; set;}
        }

        int _objectCount;

        public SequentialIDsBulkInsert(int objectCount)
        {
            _objectCount = objectCount;
        }

        public TimeSpan Run(){
            var w = Stopwatch.StartNew();

            EnsureFolder("data");
            var fileStreamProvider =  new FileStorageStreamProvider("./data/");
            var marcello = new Marcello(fileStreamProvider);
            var articles = marcello.Collection<TestClass>();

            for (int i = 1; i < _objectCount; i++)
            {
                var a = new TestClass{ID = i, Name = "Object " + i.ToString()};
                articles.Persist(a);
            }

            w.Stop();
            return new TimeSpan(w.ElapsedTicks); 
        }

        private void EnsureFolder(string path)
        {
            if(System.IO.Directory.Exists("data")){
                System.IO.Directory.Delete("data", true);
            }
            System.IO.Directory.CreateDirectory("data");      
        }
    }        
}




