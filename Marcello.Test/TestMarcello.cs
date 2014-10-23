using NUnit.Framework;
using System;
using System.Linq;

using Marcello;

namespace Marcello.Test
{
    [TestFixture ()]
    public class Test
    {
        Marcello _marcello;

        [SetUp]
        public void Setup(){
            _marcello = new Marcello (new InMemoryStreamProvider());
        }

        [Test ()]
        public void TestGetCollectionReturnsACollection ()
        {
            var collection = _marcello.GetCollection<Article> ();
            Assert.NotNull (collection, "Collection should not be null");
        }

        [Test ()]
        public void TestInsertObjectShouldFindObject(){
            var toiletPaper = Article.ToiletPaper;
            _marcello.GetCollection<Article>().Persist (toiletPaper);
            var article = _marcello.GetCollection<Article> ().All.FirstOrDefault();
            Assert.AreEqual (toiletPaper.Name, article.Name, "First article");
        }
    }
}

