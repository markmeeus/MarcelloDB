using System;
using NUnit.Framework;
using Marcello.Test.Classes;
using Marcello.Collections;
using System.Linq;

namespace Marcello.Test.Transactions
{
    [TestFixture ()]
    public class TransactionTest
    {
        Marcello _marcello;
        Collection<Article> _articles;

        [SetUp]
        public void Setup()
        {
            _marcello = new Marcello(new InMemoryStreamProvider());
            _articles = _marcello.GetCollection<Article>();;
        }

        [Test]
        public void TestInsertWithinTransactionSucceeds()
        {
            _marcello.Transaction(() => {
                _articles.Persist(Article.BarbieDoll);
            });

            Assert.AreEqual(Article.BarbieDoll.ID, _articles.All.First().ID);
        }

        [Test]
        public void TestInsertIsVisibleWithinTransaction(){
            _marcello.Transaction(() => {
                _articles.Persist(Article.BarbieDoll);
                Assert.AreEqual(Article.BarbieDoll.ID, _articles.All.First().ID);
            });                
        }

        [Test]
        public void TestUpdateWithinTransactionSucceeds()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);

            _marcello.Transaction(() => {
                barbieDoll.Name = "UPDATED";
                _articles.Persist(Article.BarbieDoll);
            });

            Assert.AreEqual(Article.BarbieDoll.Name, _articles.All.First().Name);
        }

        [Test]
        public void TestUpdateIsVisibleWithinTransaction()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);

            _marcello.Transaction(() => {
                barbieDoll.Name = "UPDATED";
                _articles.Persist(Article.BarbieDoll);
                Assert.AreEqual(Article.BarbieDoll.Name, _articles.All.First().Name);
            });                
        }

        [Test]
        public void TestDestroyWithinTransactionSucceeds()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);

            _marcello.Transaction(() => {
                _articles.Destroy(Article.BarbieDoll);
            });

            Assert.AreEqual(0, _articles.All.Count());
        }

        [Test]
        public void TestDestroyIsVisibleWithinTransaction()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);

            _marcello.Transaction(() => {
                _articles.Destroy(Article.BarbieDoll);
                Assert.AreEqual(0, _articles.All.Count());
            });
        }
    }
}

