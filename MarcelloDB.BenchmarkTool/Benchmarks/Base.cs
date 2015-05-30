using System;
using System.Diagnostics;
using System.Collections.Generic;
using MarcelloDB.BenchmarkTool.DataClasses;

namespace MarcelloDB.BenchmarkTool.Benchmarks
{
    public class Base
    {
    
        protected Session Session{get;set;}
        protected MarcelloDB.Collections.Collection<Person> Collection {get; set;}

        public Base()
        {

        }

        protected virtual void OnSetup(){}

        protected virtual void OnRun()
        {
        }            

        public TimeSpan Run()
        {           
            Stopwatch w;

            EnsureFolder("data");
            var platform = new MarcelloDB.netfx.Platform();
            using(platform)
            {
                this.Session = new Session(platform, "./data/");
                this.Collection = this.Session["persons"].Collection<Person>();
                OnSetup();

                w = Stopwatch.StartNew();
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

