using System;
using NUnit.Framework;
using MarcelloDB.Test.Classes;
using MarcelloDB.Collections;
using System.Linq;

namespace MarcelloDB.Test.Transactions
{
    [TestFixture]
    public class TransactionTest
    {
        Session _session;
        Collection<Article> _articles;
        Collection<Location> _locations;

        [SetUp]
        public void Setup()
        {
            _session = new Session(new TestPlatform(), "/");
            _articles = _session["articles_data"].Collection<Article>("articles");
            _locations = _session["locations_data"].Collection<Location>("locations");
        }

        [Test]
        public void Insert()
        {
            _session.Transaction(() => {
                _articles.Persist(Article.BarbieDoll);
                _locations.Persist(Location.Harrods);
            });

            Assert.AreEqual(Article.BarbieDoll.ID, _articles.All.First().ID);
            Assert.AreEqual(Location.Harrods.ID, _locations.All.First().ID);
        }

        [Test]
        public void Insert_Visibility(){
            _session.Transaction(() => {
                _articles.Persist(Article.BarbieDoll);
                Assert.AreEqual(Article.BarbieDoll.ID, _articles.All.First().ID);
                _locations.Persist(Location.Harrods);
                Assert.AreEqual(Location.Harrods.ID, _locations.All.First().ID);
            });
        }

        [Test]
        public void Update()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);
            var harrods = Location.Harrods;
            _locations.Persist(harrods);

            _session.Transaction(() => {
                barbieDoll.Name += "UPDATED";
                harrods.Name += "UPDATED";
                _articles.Persist(barbieDoll);
                _locations.Persist(harrods);
            });

            Assert.AreEqual(barbieDoll.Name, _articles.All.First().Name);
            Assert.AreEqual(harrods.Name, _locations.All.First().Name);
        }

        [Test]
        public void Update_Visibility()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);
            var harrods = Location.Harrods;
            _locations.Persist(harrods);

            _session.Transaction(() => {
                barbieDoll.Name = "UPDATED";
                harrods.Name += "UPDATED";
                _articles.Persist(barbieDoll);
                Assert.AreEqual(barbieDoll.Name, _articles.All.First().Name);
                _locations.Persist(harrods);
                Assert.AreEqual(harrods.Name, _locations.All.First().Name);
            });
        }

        [Test]
        public void Destroy()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);
            var harrods = Location.Harrods;
            _locations.Persist(harrods);

            _session.Transaction(() => {
                _articles.Destroy(barbieDoll.ID);
                _locations.Destroy(harrods.ID);
            });

            Assert.AreEqual(0, _articles.All.Count());
            Assert.AreEqual(0, _locations.All.Count());
        }

        [Test]
        public void Destroy_Visibility()
        {
            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(Article.BarbieDoll);
            var harrods = Location.Harrods;
            _locations.Persist(harrods);

            _session.Transaction(() => {
                _articles.Destroy(barbieDoll.ID);
                Assert.AreEqual(0, _articles.All.Count());
                _locations.Destroy(harrods.ID);
                Assert.AreEqual(0, _locations.All.Count());
            });
        }

        [Test]
        public void Insert_Rollback()
        {
            try{
                _session.Transaction (() => {
                    _articles.Persist(Article.BarbieDoll);
                    _locations.Persist(Location.Harrods);
                    throw new Exception ("Rollback");
                });
            }catch(Exception){
            }
            Assert.AreEqual(0, _articles.All.Count());
            Assert.AreEqual(0, _locations.All.Count());
        }

        [Test]
        public void Update_Rollback()
        {
            var article = Article.BarbieDoll;
            _articles.Persist(article);
            var harrods = Location.Harrods;
            _locations.Persist(harrods);

            try{
                _session.Transaction (() => {
                    article.Name += "UPDATED";
                    _articles.Persist(article);
                    harrods.Name += "UPDATED";
                    _locations.Persist(harrods);
                    throw new Exception ("Rollback");
                });
            }catch(Exception){}

            Assert.AreEqual(Article.BarbieDoll.Name, _articles.All.First().Name, "Update should be rolled back");
            Assert.AreEqual(Location.Harrods.Name, _locations.All.First().Name, "Update should be rolled back");
        }

        [Test]
        public void Destroy_Rollback()
        {
            var article = Article.BarbieDoll;
            _articles.Persist(article);

            var harrods = Location.Harrods;
            _locations.Persist(harrods);

            try{
                _session.Transaction (() => {
                    _articles.Destroy(article.ID);
                    _locations.Destroy(harrods.ID);
                    throw new Exception ("Rollback");
                });
            }catch(Exception){}

            Assert.AreEqual(Article.BarbieDoll.Name, _articles.All.First().Name, "Destroy should be rolled back");
            Assert.AreEqual(Location.Harrods.Name, _locations.All.First().Name, "Destroy should be rolled back");
        }
    }
}

