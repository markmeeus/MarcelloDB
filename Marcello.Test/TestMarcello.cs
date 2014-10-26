using NUnit.Framework;
using System;
using System.Linq;

using Marcello;
using System.Collections.Generic;

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

        [Test()]
        public void Test2Objects(){
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            _marcello.GetCollection<Article>().Persist (toiletPaper);
            _marcello.GetCollection<Article>().Persist (spinalTapDvd);
            var articleNames = _marcello.GetCollection<Article> ().All.Select(a => a.Name).ToList();
            Assert.AreEqual (new List<string>{toiletPaper.Name, spinalTapDvd.Name}, articleNames, "Multiple Articles");
        }

        [Test()]
        public void TestMultipleObjects(){
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            _marcello.GetCollection<Article>().Persist (toiletPaper);
            _marcello.GetCollection<Article>().Persist (spinalTapDvd);
            _marcello.GetCollection<Article>().Persist (barbieDoll);
            var articleNames = _marcello.GetCollection<Article> ().All.Select(a => a.Name).ToList();
            Assert.AreEqual (new List<string>{toiletPaper.Name, spinalTapDvd.Name, barbieDoll.Name}, articleNames, "Multiple Articles");
        }
    }
}

