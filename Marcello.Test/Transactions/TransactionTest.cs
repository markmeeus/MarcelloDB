using System;
using NUnit.Framework;
using Marcello.Test.Classes;
using Marcello.Collections;
using System.Linq;

namespace Marcello.Test.Transactions
{
    [TestFixture]
    public class TransactionTest
    {
        Marcello _marcello;
        Collection<Article> _articles;

        [SetUp]
        public void Setup()
        {
            _marcello = new Marcello(new InMemoryStreamProvider());
            _articles = _marcello.Collection<Article>();
        }

        [Test]
        public void Insert()
        {
            _marcello.Transaction(() => {
                _articles.Persist(Article.BarbieDoll);
            });

            Assert.AreEqual(Article.BarbieDoll.ID, _articles.All.First().ID);
        }

        [Test]
        public void Insert_Visibility(){
            _marcello.Transaction(() => {
                _articles.Persist(Article.BarbieDoll);
                Assert.AreEqual(Article.BarbieDoll.ID, _articles.All.First().ID);
            });                
        }

        [Test]
        public void Update()
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
        public void Update_Visibility()
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
        public void Destroy()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);

            _marcello.Transaction(() => {
                _articles.Destroy(Article.BarbieDoll);
            });

            Assert.AreEqual(0, _articles.All.Count());
        }

        [Test]
        public void Destroy_Visibility()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);

            _marcello.Transaction(() => {
                _articles.Destroy(Article.BarbieDoll);
                Assert.AreEqual(0, _articles.All.Count());
            });
        }

        [Test]
        public void Insert_Rollback()
        {
            try{
                _marcello.Transaction (() => {
                    _articles.Persist(Article.BarbieDoll);
                    throw new Exception ("Rollback");
                });
            }catch(Exception){
            }
            Assert.AreEqual(0, _articles.All.Count());
        }

        [Test]
        public void Update_Rollback()
        {
            var article = Article.BarbieDoll;
            _articles.Persist(article);

            try{
                _marcello.Transaction (() => {
                    article.Name = "UPDATED";
                    _articles.Persist(article);
                    throw new Exception ("Rollback");
                });
            }catch(Exception){}

            Assert.AreEqual(Article.BarbieDoll.Name, _articles.All.First().Name, "Update should be rolled back");
        }

        [Test]
        public void Destroy_Rollback()
        {           
            var article = Article.BarbieDoll;
            _articles.Persist(article);

            try{
                _marcello.Transaction (() => {
                    _articles.Destroy(article);
                    throw new Exception ("Rollback");
                });
            }catch(Exception){}

            Assert.AreEqual(Article.BarbieDoll.Name, _articles.All.First().Name, "Destroy should be rolled back");
           
        }
    }
}

