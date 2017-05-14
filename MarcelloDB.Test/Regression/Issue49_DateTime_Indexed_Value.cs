using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarcelloDB.Collections;
using MarcelloDB.Index;
using MarcelloDB.Platform;
using NUnit.Framework;

namespace MarcelloDB.Test.Regression.Issue49
{
    public class Product
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public Product ()
        {
            this.Id = Guid.NewGuid();
            this.Date = DateTime.Now;
        }
    }

    public class ProductIndexDefinition : IndexDefinition<Product>
    {
    	public IndexedValue<Product, DateTime> Date { get; set; }
    }

    [TestFixture]
    public class DateTime_Indexed_Value
    {
        Session _session;
        Collection<Product, Guid, ProductIndexDefinition> _collection;
        IPlatform _platform;
        [SetUp]
        public void Setup ()
        {
            _platform = new TestPlatform();
        	_session = new Session (_platform, "");
            _collection = _session["moves.dat"].Collection<Product, Guid, ProductIndexDefinition>
                ("products", p => p.Id);
        }

        [Test]
        public void Second_Persist_Should_Not_Crash ()
        {
            //comparing now
            var item = new Product();
            var id = item.Id;
            _collection.Persist(item);

            //lLoad from DB and persist againg
            var productFromDB = _collection.Find(id);

            //Date property now has a value with ticks rounded to milliseconds
            //The index has microsecond precision
            //after persisting this record again, there are 2 entries for this record
            //One with microsecond precision, and one with millisecond precision
            _collection.Persist (productFromDB); 

            //Persiting will first load the original to unregister index entries
            //The millisecond precision one will be removed
            //Registering will use the higher precision one, which is still in de index and causes the error
            _collection.Persist(item);
        }

    }
}
