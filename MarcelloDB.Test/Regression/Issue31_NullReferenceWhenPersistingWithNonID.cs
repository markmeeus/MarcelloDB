using MarcelloDB.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://github.com/markmeeus/MarcelloDB/issues/31
namespace MarcelloDB.Test.Regression.Issue31
{
    public class Product
    {
        public Guid Id { get; set; }
    }

    [TestFixture]
    public class Issue31_NullReferenceWhenPersistingWithNonID
    {

        Session _session;
        CollectionFile _collectionFile;
        Collection<Product> _products;
        TestPlatform _platform;

        [SetUp]
        public void Setup()
        {
            _platform = new TestPlatform();
            _session = new Session(_platform, "/");
            _collectionFile = _session["articles"];
            _products = _collectionFile.Collection<Product>("products");
        }

        [Test]
        public void Persist_With_Id_Property()
        {
            var entitiy = new Product();
            entitiy.Id = Guid.NewGuid();
            _products.Persist(entitiy);
            var foundEntity = _products.Find(entitiy.Id);
            Assert.AreEqual(entitiy.Id, foundEntity.Id);
        }
    }
}
