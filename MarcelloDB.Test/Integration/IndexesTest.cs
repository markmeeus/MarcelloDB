using System;
using NUnit.Framework;
using MarcelloDB.Collections;
using MarcelloDB.Test.Classes;
using System.Linq;
using System.Collections.Generic;

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

        [Test]
        public void Find_Finds_All()
        {
            var toiletPaper = Article.ToiletPaper;
            var tootPaste = new Article
            {
                ID = 5,
                Name = "ToothPaste",
                Description = "Tootpaste with Minty flavour",
                Category = toiletPaper.Category
            };

            _articles.Persist(toiletPaper);
            _articles.Persist(tootPaste);
            _articles.Persist(Article.BarbieDoll);
            _articles.Persist(Article.SpinalTapDvd);
            _articles.Persist(Food.Bread);

            var hygienicArticles = _articles.Indexes.Category.Find(toiletPaper.Category).ToList();

            Assert.AreEqual(
                new List<Article>{toiletPaper, tootPaste}.Select(a => a.ID),
                hygienicArticles.Select(a => a.ID));
        }

        [Test]
        public void Finds_Between()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.Between("Cat1").And("Cat4").ToList();
            Assert.AreEqual(
                new List<int>{ 2, 3, 4, 5}, found.Select(a => a.ID));
        }

        [Test]
        public void Finds_Between_Including_Start()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.BetweenIncluding("Cat2").And("Cat4").ToList();
            Assert.AreEqual(
                new List<int>{ 2, 3, 4, 5}, found.Select(a => a.ID));
        }

        [Test]
        public void Finds_Between_Including_End()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.Between("Cat1").AndIncluding("Cat3").ToList();
            Assert.AreEqual(
                new List<int>{ 2, 3, 4, 5}, found.Select(a => a.ID));
        }

        [Test]
        public void Finds_Between_Including_Start_And_End()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.BetweenIncluding("Cat2").AndIncluding("Cat3").ToList();
            Assert.AreEqual(
                new List<int>{ 2, 3, 4, 5}, found.Select(a => a.ID));
        }


        [Test]
        public void Finds_GreaterThan()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.GreaterThan("Cat2").ToList();
            Assert.AreEqual(
                new List<int>{4, 5, 6}, found.Select(a => a.ID));
        }

        [Test]
        public void Finds_SmallerThan()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.SmallerThan("Cat4").ToList();
            Assert.AreEqual(
                new List<int>{1, 2 ,3, 4, 5}, found.Select(a => a.ID));
        }
    }
}
