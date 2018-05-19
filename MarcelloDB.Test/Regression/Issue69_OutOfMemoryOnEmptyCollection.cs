using MarcelloDB.Collections;
using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcelloDB.netfx;


//https://github.com/markmeeus/MarcelloDB/issues/69
namespace MarcelloDB.Test.Regression.Issue69
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Payload { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Payload { get; set; }
    }

    [TestFixture]
    public class Issue69_OutOfMemoryOnEmptyCollection
    {
        [Test]
        public void Should_Not_Go_Corrupt_After_Enumerating_Empty_Collections ()
        {
            var platform = new TestPlatform ();
            Session session = new Session (platform, "/");
            var collectionFile = session ["articles"];
            var objects = collectionFile.Collection<object, string> ("objects", p => p.ToString());

            //insert something so that the root exists:
            objects.Persist (1);

            var products = collectionFile.Collection<Product, Guid> ("products", p => p.Id);
            var customers = collectionFile.Collection<Customer, int> ("customers", p => p.Id);

            // Creating an enumerator caused a root node to be inserted, out of any transaction
            // the recordmanager was aware of. 
            // The second enumerator actually runs an empty transaction to make sure any data is flushed,
            // the recordmanager's head however was not updated in the data file.
            products.All.ToList ();
            customers.All.ToList ();

            //the product collection now has a root node for it's id index at 'head' location
            //the customer only has one in the journal
            //Without a transaction in that specific collection file, the head itself was not updated
            session.Dispose ();
            session = new Session (platform, "/");
            collectionFile = session ["articles"];

            objects = collectionFile.Collection<object, string> ("objects", p => p.ToString ());
            products = collectionFile.Collection<Product, Guid> ("products", p => p.Id);
            customers = collectionFile.Collection<Customer, int> ("customers", p => p.Id);

            //add some data after head, it will overwrite the collection's index root nodes
            objects.Persist (2);
            objects.Persist (3);
            objects.Persist (4);

            //inserting something at the head caused the next enumeration to fail
            products.All.ToList ();
            customers.All.ToList ();
      }
    }
}
