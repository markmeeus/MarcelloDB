using System;
using NUnit.Framework;
using MarcelloDB.Collections;
using MarcelloDB.Test.Classes;
using System.Linq;
using System.Collections.Generic;
using MarcelloDB.Index;

namespace MarcelloDB.Test.Integration
{
    [TestFixture]
    public class IndexesTest
    {
        Session _session;
        CollectionFile _collectionFile;
        Collection<Article, int, ArticleIndexes> _articles;
        TestPlatform _platform;

        [SetUp]
        public void Setup()
        {
            _platform = new TestPlatform();
            _session = new Session(_platform, "/");
            _collectionFile = _session["articles"];
            _articles = _collectionFile.Collection<Article, int, ArticleIndexes>("articles", a => a.ID);
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

            var papers = _articles.Indexes.FullDescription
                .Find(String.Format("{0}-{1}", Article.ToiletPaper.Name, Article.ToiletPaper.Description));

            Assert.AreEqual(Article.ToiletPaper.ID, papers.First().ID);
        }

        [Test]
        public void Insert_Object_Updates_Custom_Compound_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(Article.BarbieDoll);
            _articles.Persist(Article.SpinalTapDvd);
            _articles.Persist(Article.ToiletPaper);

            var papers = _articles.Indexes.CodeAndName
                .Find(Article.ToiletPaper.Code, Article.ToiletPaper.Name);

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

            var papers = _articles.Indexes.FullDescription
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

            var papers = _articles.Indexes.FullDescription
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

            var papers = _articles.Indexes.FullDescription
                .Find(String.Format("{0}-{1}", Article.ToiletPaper.Name, Article.ToiletPaper.Description));
            Assert.IsEmpty(papers);
        }

        [Test]
        public void All_Enumerates_All()
        {
            var article1 = new Article{ID = 1, Name = "Article1", Description = "Description 1"};
            var article2 = new Article{ID = 2, Name = "Artcile2", Description = "Description 2"};

            _articles.Persist(article1);
            _articles.Persist(article2);

            var articles = _articles.Indexes.Description.All.ToList();

            Assert.AreEqual(
                new List<int>{1, 2},
                articles.Select(a => a.ID));
        }

        [Test]
        public void All_Descending_Enumerates_All_Descending()
        {
            var article1 = new Article{ID = 1, Name = "Article1", Description = "Description 1"};
            var article2 = new Article{ID = 2, Name = "Artcile2", Description = "Description 2"};

            _articles.Persist(article1);
            _articles.Persist(article2);

            var articles = _articles.Indexes.Description.All.Descending.ToList();

            Assert.AreEqual(
                new List<int>{2, 1},
                articles.Select(a => a.ID));
        }

        [Test]
        public void All_Keys_Enumerates_All_Keys()
        {
            var article1 = new Article{ID = 1, Name = "Article1", Description = "Description 1"};
            var article2 = new Article{ID = 2, Name = "Artcile2", Description = "Description 2"};

            _articles.Persist(article1);
            _articles.Persist(article2);

            var articleDescriptions = _articles.Indexes.Description.All.Keys.ToList();

            Assert.AreEqual(new List<string>{article1.Description, article2.Description}, articleDescriptions.ToArray());
        }

        [Test]
        public void All_Keys_Skips_Duplicates()
        {
            var article1 = new Article{ID = 1, Name = "Article1", Description = "Description 1"};
            var article2 = new Article{ID = 2, Name = "Artcile2", Description = "Description 2"};
            var article3 = new Article{ID = 3, Name = "Artcile3", Description = "Description 2"};

            _articles.Persist(article1);
            _articles.Persist(article2);
            _articles.Persist(article3);

            var articleDescriptions = _articles.Indexes.Description.All.Keys.ToList();

            Assert.AreEqual(new List<string>{article1.Description, article2.Description}, articleDescriptions.ToArray());
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
        public void Find_Finds_Null_Values()
        {
            var article1 = new Article{ID = 1, Name = "Article1", Description = null};
            var article2 = new Article{ID = 2, Name = "Artcile2", Description = null};

            _articles.Persist(article1);
            _articles.Persist(article2);

            var articles = _articles.Indexes.Category.Find(null).ToList();

            Assert.AreEqual(
                new List<int>{1, 2},
                articles.Select(a => a.ID).OrderBy( id => id));
        }

        [Test]
        public void Between()
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
        public void Between_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.Between("Cat1").And("Cat4").Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat2", "Cat3"}, found);
        }

        [Test]
        public void Between_Descending()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.Between("Cat1").And("Cat4").Descending.ToList();
            Assert.AreEqual(
                new List<int>{ 5, 4, 3, 2}, found.Select(a => a.ID));
        }

        [Test]
        public void Between_Descending_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.Between("Cat1").And("Cat4").Descending.Keys.ToList();
            Assert.AreEqual(
                new List<string>{ "Cat3", "Cat2"}, found);
        }

        [Test]
        public void Between_Including_Start()
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
        public void Between_Including_Start_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.BetweenIncluding("Cat2").And("Cat4").Keys.ToList();
            Assert.AreEqual(
                new List<string>{ "Cat2", "Cat3"}, found);
        }

        [Test]
        public void Between_Including_Start_Descending()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.BetweenIncluding("Cat2").And("Cat4").Descending.ToList();
            Assert.AreEqual(
                new List<int>{ 5, 4, 3, 2}, found.Select(a => a.ID));
        }

        [Test]
        public void Between_Including_Start_Descending_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.BetweenIncluding("Cat2").And("Cat4").Descending.Keys.ToList();
            Assert.AreEqual(
                new List<string>{ "Cat3", "Cat2"}, found);
        }

        [Test]
        public void Between_Including_Start_With_Null_Values()
        {
            _articles.Persist(new Article(){ ID = 1, Description = null});
            _articles.Persist(new Article(){ ID = 2, Description = null});
            _articles.Persist(new Article(){ ID = 3, Description = "Desc 3"});
            _articles.Persist(new Article(){ ID = 4, Description = "Desc 4"});
            _articles.Persist(new Article(){ ID = 5, Description = "Desc 5"});
            _articles.Persist(new Article(){ ID = 6, Description = "Desc 6"});

            var found = _articles.Indexes.Description.BetweenIncluding(null).And("Desc 4").ToList();
            Assert.AreEqual(
                new List<int>{ 1, 2, 3}, found.Select(a => a.ID).OrderBy(id => id));
        }

        [Test]
        public void Between_Including_Start_With_Null_Values_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Description = null});
            _articles.Persist(new Article(){ ID = 2, Description = null});
            _articles.Persist(new Article(){ ID = 3, Description = "Desc 3"});
            _articles.Persist(new Article(){ ID = 4, Description = "Desc 4"});
            _articles.Persist(new Article(){ ID = 5, Description = "Desc 5"});
            _articles.Persist(new Article(){ ID = 6, Description = "Desc 6"});

            var found = _articles.Indexes.Description.BetweenIncluding(null).And("Desc 4").Keys.ToList();
            Assert.AreEqual(
                new List<string>{ null, "Desc 3"}, found);
        }

        [Test]
        public void Between_Including_Start_With_Null_Values_Descending()
        {
            _articles.Persist(new Article(){ ID = 1, Description = null});
            _articles.Persist(new Article(){ ID = 2, Description = null});
            _articles.Persist(new Article(){ ID = 3, Description = "Desc 3"});
            _articles.Persist(new Article(){ ID = 4, Description = "Desc 4"});
            _articles.Persist(new Article(){ ID = 5, Description = "Desc 5"});
            _articles.Persist(new Article(){ ID = 6, Description = "Desc 6"});

            var found = _articles.Indexes.Description.BetweenIncluding(null).And("Desc 4").Descending.ToList();
            Assert.AreEqual(
                new List<int>{1, 2, 3}, found.Select(a => a.ID).OrderBy(id => id));
        }

        [Test]
        public void Between_Including_Start_With_Null_Values_Descending_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Description = null});
            _articles.Persist(new Article(){ ID = 2, Description = null});
            _articles.Persist(new Article(){ ID = 3, Description = "Desc 3"});
            _articles.Persist(new Article(){ ID = 4, Description = "Desc 4"});
            _articles.Persist(new Article(){ ID = 5, Description = "Desc 5"});
            _articles.Persist(new Article(){ ID = 6, Description = "Desc 6"});

            var found = _articles.Indexes.Description.BetweenIncluding(null).And("Desc 4").Descending.Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Desc 3", null}, found);
        }

        [Test]
        public void Between_Including_End()
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
        public void Between_Including_End_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.Between("Cat1").AndIncluding("Cat3").Keys.ToList();
            Assert.AreEqual(
                new List<string>{ "Cat2", "Cat3"}, found);
        }

        [Test]
        public void Between_Including_End_Descending()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.Between("Cat1").AndIncluding("Cat3").Descending.ToList();
            Assert.AreEqual(
                new List<int>{ 5, 4, 3, 2}, found.Select(a => a.ID));
        }

        [Test]
        public void Between_Including_End_Descending_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.Between("Cat1").AndIncluding("Cat3").Descending.Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat3", "Cat2"}, found);
        }

        [Test]
        public void Between_Including_Start_And_End()
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
        public void Between_Including_Start_And_End_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.BetweenIncluding("Cat2").AndIncluding("Cat3").Keys.ToList();
            Assert.AreEqual(
                new List<string>{ "Cat2", "Cat3"}, found);
        }

        [Test]
        public void Between_Including_Start_And_End_Descending()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.BetweenIncluding("Cat2").AndIncluding("Cat3").Descending.ToList();
            Assert.AreEqual(
                new List<int>{ 5, 4, 3, 2}, found.Select(a => a.ID));
        }

        [Test]
        public void Between_Including_Start_And_End_Descending_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.BetweenIncluding("Cat2").AndIncluding("Cat3").Descending.Keys.ToList();
            Assert.AreEqual(
                new List<string>{ "Cat3", "Cat2"}, found);
        }

        [Test]
        public void GreaterThan()
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
        public void GreaterThan_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.GreaterThan("Cat2").Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat3", "Cat4"}, found);
        }

        [Test]
        public void GreaterThan_Descending()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.GreaterThan("Cat2").Descending.ToList();
            Assert.AreEqual(
                new List<int>{6, 5, 4}, found.Select(a => a.ID));
        }

        [Test]
        public void GreaterThan_Descending_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.GreaterThan("Cat2").Descending.Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat4", "Cat3"}, found);
        }

        [Test]
        public void GreaterThanOrEql()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.GreaterThanOrEqual("Cat2").ToList();
            Assert.AreEqual(
                new List<int>{2, 3, 4, 5, 6}, found.Select(a => a.ID));
        }

        [Test]
        public void GreaterThanOrEql_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.GreaterThanOrEqual("Cat2").Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat2", "Cat3", "Cat4"}, found);
        }

        [Test]
        public void GreaterThanOrEql_Descending()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.GreaterThanOrEqual("Cat2").Descending.ToList();
            Assert.AreEqual(
                new List<int>{6, 5, 4, 3, 2}, found.Select(a => a.ID));
        }

        [Test]
        public void GreaterThanOrEql_Descending_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.GreaterThanOrEqual("Cat2").Descending.Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat4", "Cat3", "Cat2"}, found);
        }

        [Test]
        public void SmallerThan()
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

        [Test]
        public void SmallerThan_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.SmallerThan("Cat4").Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat1", "Cat2" ,"Cat3"}, found);
        }

        [Test]
        public void SmallerThan_Descending()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.SmallerThan("Cat4").Descending.ToList();
            Assert.AreEqual(
                new List<int>{5, 4, 3, 2, 1}, found.Select(a => a.ID));
        }

        [Test]
        public void SmallerThan_Descending_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.SmallerThan("Cat4").Descending.Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat3", "Cat2" ,"Cat1"}, found);;
        }


        [Test]
        public void SmallerThanOrEqual()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.SmallerThanOrEqual("Cat3").ToList();
            Assert.AreEqual(
                new List<int>{1, 2 ,3, 4, 5}, found.Select(a => a.ID));
        }

        [Test]
        public void SmallerThanOrEqual_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.SmallerThanOrEqual("Cat3").Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat1", "Cat2" ,"Cat3"}, found);
        }

        [Test]
        public void SmallerThanOrEqual_Descending()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.SmallerThanOrEqual("Cat3").Descending.ToList();
            Assert.AreEqual(
                new List<int>{5, 4, 3, 2, 1}, found.Select(a => a.ID));
        }

        [Test]
        public void SmallerThanOrEqual_Descending_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 3, Name = "Item 2b", Description = "Desc 2b", Category = "Cat2" });
            _articles.Persist(new Article(){ ID = 4, Name = "Item 3a", Description = "Desc 3a", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 5, Name = "Item 3b", Description = "Desc 3b", Category = "Cat3" });
            _articles.Persist(new Article(){ ID = 6, Name = "Item 4", Description = "Desc 4", Category = "Cat4" });

            var found = _articles.Indexes.Category.SmallerThanOrEqual("Cat3").Descending.Keys.ToList();
            Assert.AreEqual(
                new List<string>{"Cat3", "Cat2" ,"Cat1"}, found);
        }

        [Test]
        public void Cannot_Update_While_Iterating()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            Assert.Throws<InvalidOperationException>(() =>
                {
                    foreach(var article in _articles.Indexes.Name.GreaterThan(null)){
                        _articles.Persist(Article.SpinalTapDvd);
                    }
                });
        }

        [Test]
        public void Cannot_Update_While_Iterating_Keys()
        {
            _articles.Persist(new Article(){ ID = 1, Name = "Item 1", Description = "Desc 1", Category = "Cat1" });
            _articles.Persist(new Article(){ ID = 2, Name = "Item 2a", Description = "Desc 2a", Category = "Cat2" });
            Assert.Throws<InvalidOperationException>(() =>
                {
                    foreach(var article in _articles.Indexes.Name.GreaterThan(null).Keys){
                        _articles.Persist(Article.SpinalTapDvd);
                    }
                });
        }

        class UnexpectedIndexedAttributeDefinition : IndexDefinition<Article>
        {
            public string Description { get; set; }
        }

        [Test]
        public void Throws_When_IndexDefinition_Contains_Unexpected_Property()
        {
            Assert.Throws<InvalidOperationException>(() =>
                {
                    _collectionFile.Collection<Article, int, UnexpectedIndexedAttributeDefinition>("articles", a => a.ID);
                });
        }
    }
}
