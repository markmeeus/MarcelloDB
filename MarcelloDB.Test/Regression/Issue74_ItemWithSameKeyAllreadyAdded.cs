using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarcelloDB.Collections;
using MarcelloDB.Index;
using NUnit.Framework;

namespace MarcelloDB.Test.Regression
{
    public class Product
    {
        public Guid Id { get; set; }
        public bool IsGood { get; set; }
    }

    [TestFixture]
    public class Issue74_ItemWithSameKeyAllreadyAdded
    {
        Session _session;
        CollectionFile _collectionFile;
        TestPlatform _platform;

        [SetUp]
        public void Setup ()
        {
            _platform = new TestPlatform ();
            _session = new Session (_platform, "/");
            _collectionFile = _session ["articles"];
        }

        [Test]
        public void ParallelCreateCollectionFile()
        {
            var tasks = new List<Task> ();
            tasks.Add(Task.Run (() => {
                var collection = _collectionFile.Collection<Product, Guid> ("products", p => p.Id);
            }));
            tasks.Add (Task.Run (() => {
                var collection = _collectionFile.Collection<Product, Guid> ("products", p => p.Id);
            }));


            Task.WaitAll (tasks.ToArray ());

        }
    }
}
