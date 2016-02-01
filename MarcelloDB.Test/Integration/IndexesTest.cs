using System;
using NUnit.Framework;
using MarcelloDB.Collections;
using MarcelloDB.Test.Classes;
using System.Linq;

namespace MarcelloDB.Test.Integration
{
    [TestFixture]
    public class IndexesTest
    {
        Session _session;
        CollectionFile _collectionFile;
        Collection<Article, ArticleIndexes> _articles;
        TestPlatform _platform;

        [SetUp]
        public void Setup()
        {
            _platform = new TestPlatform();
            _session = new Session(_platform, "/");
            _collectionFile = _session["articles"];
            _articles = _collectionFile.Collection<Article, ArticleIndexes>("articles");
        }

        [Test]
        public void Insert_Object_Updates_Property_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);
            var papers = _articles.Indexes.Name.Find(toiletPaper.Name);

            Assert.AreEqual(Article.ToiletPaper.ID, papers.First().ID);
        }

        [Test]
        public void Insert_Object_Updates_Custom_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);

            var papers = _articles.Indexes.NameAndDescription
                .Find(String.Format("{0}-{1}", Article.ToiletPaper.Name, Article.ToiletPaper.Description));

            Assert.AreEqual(Article.ToiletPaper.ID, papers.First().ID);
        }

        [Test]
        public void Update_Object_Removes_From_Property_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);
            var originalName = toiletPaper.Name;
            toiletPaper.Name = "Papier de toilette";
            _articles.Persist(toiletPaper);

            var papers = _articles.Indexes.Name.Find(originalName);
            Assert.IsEmpty(papers);
        }

        [Test]
        public void Update_Object_Removes_From_Custom_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);
            var originalName = toiletPaper.Name;
            toiletPaper.Name = "Papier de toilette";
            _articles.Persist(toiletPaper);

            var papers = _articles.Indexes.NameAndDescription
                .Find(String.Format("{0}-{1}", originalName, Article.ToiletPaper.Description));

            Assert.IsEmpty(papers);
        }

        [Test]
        public void Large_Update_Object_Adjusts_Property_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);
            var originalName = toiletPaper.Name;
            toiletPaper.Name = new string('a', 10000);
            _articles.Persist(toiletPaper);

            var papers = _articles.Indexes.Name.Find(toiletPaper.Name);
            Assert.AreEqual(Article.ToiletPaper.ID, papers.First().ID);
        }

        [Test]
        public void Large_Update_Object_Adjusts_Custom_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);
            var originalName = toiletPaper.Name;
            toiletPaper.Name = new string('a', 10000);
            _articles.Persist(toiletPaper);

            var papers = _articles.Indexes.NameAndDescription
                .Find(String.Format("{0}-{1}", toiletPaper.Name,toiletPaper.Description));

            Assert.AreEqual(Article.ToiletPaper.ID, papers.First().ID);
        }

        [Test]
        public void Destroy_Object_Removes_From_Property_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);
            _articles.Destroy(toiletPaper.ID);

            var papers = _articles.Indexes.Name.Find(toiletPaper.Name);
            Assert.IsEmpty(papers);
        }

        [Test]
        public void Destroy_Object_Removes_From_Custom_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);
            _articles.Destroy(toiletPaper.ID);

            var papers = _articles.Indexes.NameAndDescription
                .Find(String.Format("{0}-{1}", Article.ToiletPaper.Name, Article.ToiletPaper.Description));
            Assert.IsEmpty(papers);
        }


    }
}
