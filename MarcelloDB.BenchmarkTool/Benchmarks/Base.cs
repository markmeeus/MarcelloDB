using System;
using System.Diagnostics;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class Base
    {
        public class TestClass
        {
            public int ID { get; set; }
            public string Name { get; set;}
        }

        protected Marcello Session{get;set;}
        protected MarcelloDB.Collections.Collection<TestClass> Collection {get; set;}

        public Base()
        {

        }

        protected virtual void OnRun()
        {
        }

        public TimeSpan Run(){
            var w = Stopwatch.StartNew();

            EnsureFolder("data");
            var fileStreamProvider =  new FileStorageStreamProvider("./data/");
            this.Session = new Marcello(fileStreamProvider);
            this.Collection = this.Session.Collection<TestClass>();

            OnRun();

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

