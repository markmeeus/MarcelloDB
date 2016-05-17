using MarcelloDB.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcelloDB.Test.Regression.Issue32
{
    public class Product
    {
        public Guid Id { get; set; }
    }

    [TestFixture]
    public class Issue32_OutOfMemoryOnSecondPersist
    {
        TestPlatform _platform;

        [SetUp]
        public void Setup()
        {
            _platform = new TestPlatform();
        }

        [Test]
        public void Should_Not_Throw_On_Second_Persist()
        {
            SaveProduct();
            Assert.DoesNotThrow(SaveProduct);
        }

        void SaveProduct()
        {
            using(var session = new Session(_platform, "/"))
            {
                var collectionFile = session["products"];
                var products = collectionFile.Collection<Product, Guid>("products", p => p.Id);

                var product = new Product();
                product.Id = Guid.NewGuid();
                products.Persist(product);
            }
        }
    }
}
