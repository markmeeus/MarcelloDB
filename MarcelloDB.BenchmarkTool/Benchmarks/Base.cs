using System;
using System.Diagnostics;
using System.Collections.Generic;
using MarcelloDB.BenchmarkTool.DataClasses;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class Base
    {
    
        protected Marcello Session{get;set;}
        protected MarcelloDB.Collections.Collection<Person> Collection {get; set;}

        public Base()
        {

        }

        protected virtual void OnRun()
        {
        }

        public TimeSpan Run(){
            var w = Stopwatch.StartNew();

            EnsureFolder("data");
            using(var fileStreamProvider =  new FileStorageStreamProvider("./data/"))
            {
                this.Session = new Marcello(fileStreamProvider);
                this.Collection = this.Session.Collection<Person>();

                OnRun();  
            }
            
            w.Stop();
            return w.Elapsed;
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

