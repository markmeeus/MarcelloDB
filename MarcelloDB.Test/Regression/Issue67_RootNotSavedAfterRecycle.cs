using MarcelloDB.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://github.com/markmeeus/MarcelloDB/issues/67
namespace MarcelloDB.Test.Regression.Issue67
{
    public class Product
    {
        public int Id { get; set; }
        public byte [] Payload { get; set; }
    }

    [TestFixture]
    public class Issue67_RootNotSavedAfterRecycle
    {

        [SetUp]
        public void Setup ()
        {           
        }

        [Test]
        public void Persist_With_Id_Property ()
        {
            var platform = new TestPlatform ();
            Session session = new Session (platform, "/");
            var collectionFile = session ["articles"];
            var products = collectionFile.Collection<Product, int> ("products", p => p.Id);

            Product product;
            for (int i = 0; i < MarcelloDB.Index.RecordIndex.BTREE_DEGREE * 3; i ++){
                product = new Product ();
                product.Id = i;
                products.Persist (product);    
            }

            // Destroy will cause the record to be recycled.
            // a new node will be added to the empty record index, cause head to move
            // Issue67 caused this head not be updated in the transaction
            session.Transaction (() => {
                for (int i = 0; i < MarcelloDB.Index.RecordIndex.BTREE_DEGREE * 3; i++) {
                    products.Destroy (i);
                }
            });

            // close the session
            session.Dispose();

            // create a new session
            session = new Session (platform, "/");
            collectionFile = session ["articles"];
            products = collectionFile.Collection<Product, int> ("products", p => p.Id);

            // Add large products
            // These will not reuse the previously deleted records, cause an overwrite 
            // of the empty record index nodes form previous transaction
            for (int i = 0; i < MarcelloDB.Index.RecordIndex.BTREE_DEGREE * 3; i++) {
                product = new Product ();
                product.Id = i;
                product.Payload = new byte [1024];
                products.Persist (product);
            }

            // Destroy all these objects will try to deplete the entire empty record index.
            // Which is currupted by the previous destroying transaction
            // This causes unpredictable errors because the data is corrupted
            for (int i = 0; i < MarcelloDB.Index.RecordIndex.BTREE_DEGREE * 3; i++) {
                products.Destroy (i);
            }
        }
    }
}
