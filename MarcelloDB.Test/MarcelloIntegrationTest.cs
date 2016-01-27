using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

using MarcelloDB;
using MarcelloDB.Test.Classes;
using MarcelloDB.Collections;
using MarcelloDB.Storage;
using MarcelloDB.Exceptions;
using MarcelloDB.Transactions;
using MarcelloDB.Index;

namespace MarcelloDB.Test
{
    [TestFixture]
    public class MarcelloIntegrationTest
    {
        Session _session;
        CollectionFile _collectionFile;
        Collection<Article> _articles;
        TestPlatform _platform;
        InMemoryStreamProvider _provider;

        [SetUp]
        public void Setup()
        {
            _platform = new TestPlatform();
            _session = new Session(_platform, "/");
            _provider = (InMemoryStreamProvider)_session.StreamProvider;
            _collectionFile = _session["articles"];
            _articles = _collectionFile.Collection<Article>("articles");
        }

        [Test]
        public void Indexer_Returns_CollectionFile()
        {
            Assert.NotNull(_collectionFile, "CollectionFile should not be null");
        }

        [Test]
        public void Collection_Returns_A_Collection()
        {
            var collection = _articles;
            Assert.NotNull(collection, "Collection should not be null");
        }

        [Test]
        public void Collections_Are_Reused_Per_File()
        {
            Assert.AreSame(_session["data"].Collection<Article>("articles"), _session["data"].Collection<Article>("articles"));
        }

        [Test]
        public void Collections_Are_Not_Reused_Over_Different_Files()
        {
            Assert.AreNotSame(_session["data"].Collection<Article>("articles"), _session["data_copy"].Collection<Article>("articles"));
        }

        [Test]
        public void Insert_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            _articles.Persist(toiletPaper);
            var article = _articles.Find(toiletPaper.ID);

            Assert.AreEqual(toiletPaper.Name, article.Name, "First article");
        }

        class ArticleIndexes : IndexDefinition<Article>
        {
            public IndexedValue<Article, string> Name { get; set;}
            public IndexedValue<Article, string> Description { get; set; }

            public IndexedValue<Article, string> NameAndDescription {
                get {
                    return IndexedValue((Article article) =>
                    {
                        return String.Format("{0}-{1}", article.Name, article.Description);
                    });
                }
            }
        }

        [Test]
        public void Insert_Object_Updates_Property_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            var articles = _session["indexed_articles"].Collection<Article, ArticleIndexes>("articles");

            articles.Persist(toiletPaper);
            var papers = articles.Indexes.Name.Find(toiletPaper.Name);

            Assert.AreEqual(Article.ToiletPaper.ID, papers.First().ID);
        }

        [Test]
        public void Insert_Object_Updates_Method_Index()
        {
            var toiletPaper = Article.ToiletPaper;

            var articles = _session["indexed_articles"].Collection<Article, ArticleIndexes>("articles");

            articles.Persist(toiletPaper);

            var papers = articles.Indexes.NameAndDescription
                .Find(String.Format("{0}-{1}", Article.ToiletPaper.Name, Article.ToiletPaper.Description));

            Assert.AreEqual(Article.ToiletPaper.ID, papers.First().ID);
        }

        [Test]
        public void Insert_2_Objects()
        {
            var toiletPaper = Article.ToiletPaper;

            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist (spinalTapDvd);

            var articleNames = _articles.All.Select(a => a.Name).ToList();

            Assert.AreEqual(new List<string>{toiletPaper.Name, spinalTapDvd.Name}, articleNames, "Should return 2 article names");
        }

        [Test()]
        public void Insert_Multiple_Objects()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(barbieDoll);

            var articleNames = _articles.All.Select(a => a.Name).ToList();
            Assert.AreEqual(new List<string>{toiletPaper.Name, spinalTapDvd.Name, barbieDoll.Name}, articleNames, "Should return multiple article names");
        }

        [Test]
        public void Update()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            toiletPaper.Name += "Extra Strong ToiletPaper";

            _articles.Persist(toiletPaper);

            var reloadedArticle = _articles.All.Where (a => a.ID == Article.ToiletPaper.ID).FirstOrDefault ();

            Assert.AreEqual(toiletPaper.Name, reloadedArticle.Name, "Should return updated article name");
        }

        [Test]
        public void Small_Update()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            toiletPaper.Name = "Short";
            _articles.Persist(toiletPaper);

            var articleNames = _articles.All.Select(a => a.Name).ToList();
            articleNames.Sort();

            Assert.AreEqual(new List<string>{spinalTapDvd.Name, toiletPaper.Name}, articleNames, "Should return updated article names");
        }

        [Test]
        public void Large_Update()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            toiletPaper.Name += "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus in lorem porta, mollis odio sit amet, tincidunt leo. Aliquam suscipit sapien nec orci fermentum imperdiet. Sed id est ante. Aliquam nec nibh id purus fermentum lobortis. Morbi posuere ullamcorper diam, in tincidunt mi pulvinar ut. Nam imperdiet mi a viverra congue. Proin eros metus, vehicula tempus eros vitae, pulvinar posuere nisi. Sed volutpat laoreet tortor. Sed sagittis nunc sed dui sollicitudin porta. Donec non neque ut erat commodo convallis vel ac dolor. Quisque eu lectus dapibus, varius sem non, semper dolor. Morbi at venenatis tellus. Integer efficitur neque ornare, lobortis nisi suscipit, consequat purus. Aliquam erat volutpat.";

            _articles.Persist(toiletPaper);

            var articleNames = _articles.All.Select(a => a.Name).ToList();
            articleNames.Sort();

            Assert.AreEqual(new List<string>{spinalTapDvd.Name, toiletPaper.Name}, articleNames, "Should return updated article names");
        }

        [Test]
        public void Large_Update_For_Last_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            spinalTapDvd.Name += "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus in lorem porta, mollis odio sit amet, tincidunt leo. Aliquam suscipit sapien nec orci fermentum imperdiet. Sed id est ante. Aliquam nec nibh id purus fermentum lobortis. Morbi posuere ullamcorper diam, in tincidunt mi pulvinar ut. Nam imperdiet mi a viverra congue. Proin eros metus, vehicula tempus eros vitae, pulvinar posuere nisi. Sed volutpat laoreet tortor. Sed sagittis nunc sed dui sollicitudin porta. Donec non neque ut erat commodo convallis vel ac dolor. Quisque eu lectus dapibus, varius sem non, semper dolor. Morbi at venenatis tellus. Integer efficitur neque ornare, lobortis nisi suscipit, consequat purus. Aliquam erat volutpat.";

            _articles.Persist(spinalTapDvd);

            var articleNames = _articles.All.Select(a => a.Name).ToList();
            articleNames.Sort();

            Assert.AreEqual(new List<string>{spinalTapDvd.Name, toiletPaper.Name}, articleNames, "Should return updated article names");
        }

        [Test]
        public void Destroy_Only_Object()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);

            _articles.Destroy(toiletPaper.ID);

            Assert.AreEqual (0, _articles.All.Count());
        }

        [Test]
        public void Destroy_First_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            _articles.Destroy(toiletPaper.ID);

            Assert.AreEqual(1, _articles.All.Count());
            Assert.AreEqual(spinalTapDvd.ID, _articles.All.First().ID);
        }

        [Test]
        public void Destroy_Middle_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(barbieDoll);

            _articles.Destroy(spinalTapDvd.ID);

            Assert.AreEqual(2, _articles.All.Count());
            Assert.AreEqual(toiletPaper.ID, _articles.All.First().ID);
            Assert.AreEqual(barbieDoll.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Destroy_Last_Object()
        {

            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(barbieDoll);

            _articles.Destroy(barbieDoll.ID);

            Assert.AreEqual(2, _articles.All.Count());
            Assert.AreEqual(toiletPaper.ID, _articles.All.First().ID);
            Assert.AreEqual(spinalTapDvd.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Destroy_All_Same_Size()
        {
            foreach (var a in new Article[]{
                new Article{ Name = "Article", ID = 1 },
                new Article{ Name = "Article", ID = 2 },
                new Article{ Name = "Article", ID = 3 }
            })
            {
                _articles.Persist(a);
            }

            foreach (var a in _articles.All.ToArray())
            {
                _articles.Destroy(a.ID);
            }

            Assert.AreEqual(0, _articles.All.Count());
        }

        [Test]
        public void Destroy_Last_Insert_New()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Destroy(spinalTapDvd.ID);

            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(barbieDoll);

            Assert.AreEqual(2, _articles.All.Count());
            Assert.AreEqual(toiletPaper.ID, _articles.All.First().ID);
            Assert.AreEqual(barbieDoll.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Destroy_Only_Insert_New()
        {
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(spinalTapDvd);
            _articles.Destroy(spinalTapDvd.ID);

            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(barbieDoll);

            Assert.AreEqual(1, _articles.All.Count());
            Assert.AreEqual(barbieDoll.ID, _articles.All.First().ID);
            Assert.AreEqual(barbieDoll.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Destroy_And_Insert_Reuse_Storage_Space()
        {
            var barbieDoll = Article.BarbieDoll;
            var spinalTapDvd = Article.SpinalTapDvd;

            //create and destroy a few records to allow db to initialize
            _articles.Persist(barbieDoll);
            _articles.Persist(spinalTapDvd);
            _articles.Destroy(spinalTapDvd.ID);
            _articles.Persist(spinalTapDvd);

            _session.Journal.Apply (); //make sure the journal is applied to the backing stream

            var storageSize = ((InMemoryStream)_provider.GetStream("articles")).BackingStream.Length;
            _articles.Destroy(barbieDoll.ID);
            _articles.Persist(barbieDoll);

            _session.Journal.Apply (); //make sure the journal is applied to the backing stream

            var newStorageSize = ((InMemoryStream)_provider.GetStream("articles")).BackingStream.Length;
            Assert.AreEqual(storageSize, newStorageSize);
        }

        [Test]
        public void Multiple_Delete_And_Insert_Reuse_Storage_Space()
        {
            var barbieDoll = Article.BarbieDoll;
            var spinalTapDvd = Article.SpinalTapDvd;
            var toiletPaper = Article.ToiletPaper;

            //a few inserts and delete to allow db to initialize
            _articles.Persist(barbieDoll);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(toiletPaper);
            _articles.Destroy(toiletPaper.ID);
            _articles.Persist(toiletPaper);

            _session.Journal.Apply (); //make sure the journal is applied to the backing stream

            var storageSize = ((InMemoryStream)_provider.GetStream("articles")).BackingStream.Length;
            _articles.Destroy(barbieDoll.ID);
            _articles.Destroy(toiletPaper.ID);

            _articles.Persist(barbieDoll);
            _articles.Persist(toiletPaper);
            _session.Journal.Apply (); //make sure the journal is applied to the backing stream

            var newStorageSize = ((InMemoryStream)_provider.GetStream("articles")).BackingStream.Length;
            Assert.AreEqual(storageSize, newStorageSize);
        }

        [Test]
        public void Can_Handle_Subclasses()
        {
            var bread = Food.Bread;
            _articles.Persist(bread);
            var breadFromDB = (Food) _articles.All.First();
            Assert.NotNull(breadFromDB);
            Assert.AreEqual(bread.Expires.ToString(), breadFromDB.Expires.ToString());
            Assert.AreEqual(bread.Name, breadFromDB.Name );
        }

        [Test]
        public void Save_To_File_Stream()
        {
            EnsureFolder("data");
            var platform = new MarcelloDB.netfx.Platform();

            using(var session = new Session(platform, "./data/"))
            {
                var articles = session["data"].Collection<Article>("articles");

                var toiletPaper = Article.ToiletPaper;
                var spinalTapDvd = Article.SpinalTapDvd;
                var barbieDoll = Article.BarbieDoll;

                articles.Persist(toiletPaper);
                articles.Persist(spinalTapDvd);
                articles.Persist(barbieDoll);

                var articleNames = articles.All.Select(a => a.Name).ToList();

                Assert.AreEqual(new List<string> { toiletPaper.Name, spinalTapDvd.Name, barbieDoll.Name }, articleNames);
            }
        }

        [Test]
        public void Add_1000_ToFile()
        {
            EnsureFolder("data");
            var platform =  new MarcelloDB.netfx.Platform();
            using (var session = new Session(platform, "./data/"))
            {
                var articles = session["data"].Collection<Article>("articles");
                var ids = new List<int>();
                for (int i = 1; i < 1000; i++)
                {
                    ids.Add(i);
                    var a = new Article { ID = i, Name = "Article " + i.ToString() };
                    articles.Persist(a);
                }

                for (int i = 1; i < 1000; i++)
                {
                    var a = articles.Find(i);
                    Assert.NotNull(a, "Article with ID: " + i.ToString() + " Should have been found");
                    Assert.AreEqual(i, a.ID, "Article " + i.ToString() + " Should have been found.");
                }

                var savedIds = articles.All.Select(a => a.ID);
                Assert.AreEqual(ids, savedIds);
            }
        }

        [Test]
        public void Add_And_Remove_1000()
        {

            var articles = _session["data"].Collection<Article>("articles");

            for (int i = 1; i < 1000; i++)
            {
                var a = new Article { ID = i, Name = "Article " + i.ToString() };
                articles.Persist(a);
            }

            for (int i = 1; i < 1000; i++)
            {
                var a = articles.Find(i);
                articles.Destroy(a.ID);
            }

            Assert.AreEqual(0, articles.All.Count());

            for (int i = 1; i < 1000; i++)
            {
                var a = new Article { ID = i, Name = "Article " + i.ToString() };
                articles.Persist(a);
            }

            Assert.AreEqual(999, articles.All.Count());

            for (int i = 1; i < 1000; i++)
            {
                var a = new Article { ID = i, Name = "Article " + i.ToString() };
                articles.Destroy(a.ID);
            }
                Assert.AreEqual(0, articles.All.Count());
        }

        [Test]
        public void Add_100_And_Remove_Backwards()
        {

            var articles = _session["data"].Collection<Article>("articles");

            for (int i = 1; i < 1000; i++)
            {
                var a = new Article { ID = i, Name = "Article " + i.ToString() };
                articles.Persist(a);
            }

            for (int i = 999; i > 0; i--)
            {
                var a = articles.Find(i);
                articles.Destroy(a.ID);
            }

            Assert.AreEqual(0, articles.All.Count());

            for (int i = 1; i < 1000; i++)
            {
                var a = new Article { ID = i, Name = "Article " + i.ToString() };
                articles.Persist(a);
            }

            Assert.AreEqual(999, articles.All.Count());

            for (int i = 999; i > 0; i--)
            {
                var a = new Article { ID = i, Name = "Article " + i.ToString() };
                articles.Destroy(a.ID);
            }
            Assert.AreEqual(0, articles.All.Count());
        }

        [Test]
        public void Add_And_Remove_Random_1000()
        {
            var rand = new Random();
            var articles = _session["data"].Collection<Article>("articles");
            var articleIDs = new List<int>();
            for (int i = 1; i < 1000; i++)
            {
                var a = new Article { ID = i, Name = "Article " + i.ToString() };
                articles.Persist(a);
                articleIDs.Add(a.ID);
            }


            while(articleIDs.Count > 0){
                var toDelete = articleIDs [rand.Next() % articleIDs.Count];
                articles.Destroy(toDelete);
                articleIDs.Remove(toDelete);
            }

        }

        [Test]
        public void Can_Use_Multiple_Collections()
        {
            var locations = _collectionFile.Collection<Location>("locations");
            _articles.Persist(Article.SpinalTapDvd);
            locations.Persist(Location.Harrods);
            _articles.Persist(Article.BarbieDoll);
            locations.Persist(Location.MandS);

            Assert.AreEqual(Article.SpinalTapDvd.Name, _articles.All.First().Name);
            Assert.AreEqual(Article.BarbieDoll.Name, _articles.All.Last().Name);

            Assert.AreEqual(Location.Harrods.Name, locations.All.First().Name);
            Assert.AreEqual(Location.MandS.Name, locations.All.Last().Name);

            _articles.Destroy(Article.SpinalTapDvd.ID);
            locations.Destroy(Location.Harrods.ID);

            Assert.AreEqual(1, _articles.All.Count());
            Assert.AreEqual(1, locations.All.Count());
            Assert.AreEqual(Article.BarbieDoll.Name, _articles.All.Last().Name);
            Assert.AreEqual(Location.MandS.Name, locations.All.Last().Name);
        }

        [Test]
        public void Can_Use_Multiple_Collections_Of_Same_Type()
        {
            var _articles1 = _collectionFile.Collection<Article>("articles1");
            var _articles2 = _collectionFile.Collection<Article>("articles2");
            _articles1.Persist(Article.BarbieDoll);
            _articles1.Persist(Article.ToiletPaper);
            _articles2.Persist(Article.SpinalTapDvd);

            Assert.AreEqual(2, _articles1.All.Count());
            Assert.AreEqual(1, _articles2.All.Count());

            Assert.NotNull(_articles1.All.FirstOrDefault(a => a.ID == Article.BarbieDoll.ID));
            Assert.NotNull(_articles1.Find(Article.BarbieDoll.ID));
            Assert.NotNull(_articles1.All.FirstOrDefault(a => a.ID == Article.ToiletPaper.ID));
            Assert.NotNull(_articles1.Find(Article.ToiletPaper.ID));
            Assert.Null(_articles1.All.FirstOrDefault(a => a.ID == Article.SpinalTapDvd.ID));
            Assert.Null(_articles1.Find(Article.SpinalTapDvd.ID));

            Assert.NotNull(_articles2.All.FirstOrDefault(a => a.ID == Article.SpinalTapDvd.ID));
            Assert.NotNull(_articles2.Find(Article.SpinalTapDvd.ID));
            Assert.Null(_articles2.All.FirstOrDefault(a => a.ID == Article.BarbieDoll.ID));
            Assert.Null(_articles2.Find(Article.BarbieDoll.ID));
            Assert.Null(_articles2.All.FirstOrDefault(a => a.ID == Article.ToiletPaper.ID));
            Assert.Null(_articles2.Find(Article.ToiletPaper.ID));
        }

        [Test]
        public void Throw_IDMissingException_When_Object_Has_No_ID_Property()
        {
            Assert.Throws(typeof(IDMissingException), () =>
                {
                    _session["data"].Collection<object>("objects").Persist(new {Name = "Object Without ID"});
                });
        }

        [Test]
        public void Throw_ObjectNotFoundException_When_Deleting_Non_Existing_Object()
        {
            Assert.Throws(typeof(ObjectNotFoundException), () =>
                {
                    _session["data"].Collection<object>("objects").Destroy("123");
                });
        }


        [Test]
        public void Thows_ArgumentException_When_Trying_To_Open_Journal_File()
        {
            Assert.Throws(typeof(ArgumentException), () =>
            {
                _session[Journal.JOURNAL_COLLECTION_NAME].Collection<object>("objects");
            });

            Assert.Throws(typeof(ArgumentException), () =>
            {
                _session[Journal.JOURNAL_COLLECTION_NAME.ToLower()].Collection<object>("objects");
            });

            Assert.Throws(typeof(ArgumentException), () =>
            {
                _session[Journal.JOURNAL_COLLECTION_NAME.ToUpper()].Collection<object>("objects");
            });
        }

        [Test]
        public void Throwd_InvalidOperation_When_Interacting_With_Collection_While_Enumerating()
        {
            _articles.Persist(Article.BarbieDoll);
            _articles.Persist(Article.SpinalTapDvd);
            _articles.Persist(Article.ToiletPaper);
            Assert.Throws(typeof(InvalidOperationException), () =>
                {
                    foreach(var a in _articles.All){
                        _articles.Persist(a);
                    }
                });
            Assert.Throws(typeof(InvalidOperationException), () =>
                {
                    foreach(var a in _articles.All){
                        _articles.Destroy(a.ID);
                    }
                });
        }

        [Test]
        public void Throws_InvalidOperation_When_A_Collection_Exists_For_Another_Type()
        {
            _collectionFile.Collection<Article>("articles");
            Assert.Throws(typeof(InvalidOperationException), () =>
                {
                    _collectionFile.Collection<object>("articles");
                });
        }

        [Test]
        public void Recovers_After_Exception_Inside_Enumeration()
        {
            _articles.Persist(Article.BarbieDoll);
            _articles.Persist(Article.SpinalTapDvd);
            _articles.Persist(Article.ToiletPaper);
            try{
                foreach(var a in _articles.All){
                    _articles.Persist(a);
                }
            }catch(Exception){}

            Assert.DoesNotThrow(()=>{
                _articles.Persist(Article.BarbieDoll);
            });
        }

        [Test]
        public void Throw_ObjectDisposedException_When_Enumerating_In_Disposed_Session()
        {
            _session.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _articles.All.ToList());
        }

        [Test]
        public void Throw_ObjectDisposedException_When_Persisting_In_Disposed_Session()
        {
            _session.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _articles.Persist(Article.BarbieDoll));
        }

        [Test]
        public void Throw_ObjectDisposedException_When_Finding_In_Disposed_Session()
        {
            _articles.Persist(Article.BarbieDoll);
            _session.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _articles.Find(Article.BarbieDoll.ID));
        }

        [Test]
        public void Throw_ObjectDisposedException_When_Destroying_In_Disposed_Session()
        {
            _articles.Persist(Article.BarbieDoll);
            _session.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _articles.Destroy(Article.BarbieDoll));
        }

        private void EnsureFolder(string path)
        {
            if(System.IO.Directory.Exists("data")){
                System.IO.Directory.Delete("data", true);
            }
            System.IO.Directory.CreateDirectory("data");
        }
    }
}

