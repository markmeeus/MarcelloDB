using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

using Marcello;
using Marcello.Test.Classes;
using Marcello.Collections;

namespace Marcello.Test
{
    [TestFixture ()]
    public class MarcelloIntegrationTest
    {
        Marcello _marcello;
        Collection<Article> _articles;
        InMemoryStreamProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new InMemoryStreamProvider();
            _marcello = new Marcello(_provider);
            _articles = _marcello.Collection<Article>();
        }
            
        [Test]
        public void Collection_Returns_A_Collection()
        {
            var collection = _articles;
            Assert.NotNull(collection, "Collection should not be null");
        }

        [Test]
        public void Collections_Are_Reused_Per_Session()
        {
            Assert.AreSame(_marcello.Collection<Article>(), _marcello.Collection<Article>());
        }

        [Test]
        public void Insert_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            _articles.Persist(toiletPaper);
            var article = _articles.All.FirstOrDefault();

            Assert.AreEqual(toiletPaper.Name, article.Name, "First article");
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
        public void Delete_Only_Object()
        {
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(toiletPaper);

            _articles.Destroy(toiletPaper);
        
            Assert.AreEqual (0, _articles.All.Count());
        }

        [Test]
        public void Delete_First_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);

            _articles.Destroy(toiletPaper);

            Assert.AreEqual(1, _articles.All.Count());
            Assert.AreEqual(spinalTapDvd.ID, _articles.All.First().ID);
        }

        [Test]
        public void Delete_Middle_Object()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(barbieDoll);

            _articles.Destroy(spinalTapDvd);

            Assert.AreEqual(2, _articles.All.Count());
            Assert.AreEqual(toiletPaper.ID, _articles.All.First().ID);
            Assert.AreEqual(barbieDoll.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Delete_Last_Object()
        {

            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(barbieDoll);

            _articles.Destroy(barbieDoll);

            Assert.AreEqual(2, _articles.All.Count());
            Assert.AreEqual(toiletPaper.ID, _articles.All.First().ID);
            Assert.AreEqual(spinalTapDvd.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Delete_Last_Insert_New()
        {
            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(toiletPaper);
            _articles.Persist(spinalTapDvd);
            _articles.Destroy(spinalTapDvd);

            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(barbieDoll);

            Assert.AreEqual(2, _articles.All.Count());
            Assert.AreEqual(toiletPaper.ID, _articles.All.First().ID);
            Assert.AreEqual(barbieDoll.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Delete_Only_Insert_New()
        {        
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(spinalTapDvd);
            _articles.Destroy(spinalTapDvd);

            var barbieDoll = Article.BarbieDoll;
            _articles.Persist(barbieDoll);

            Assert.AreEqual(1, _articles.All.Count());
            Assert.AreEqual(barbieDoll.ID, _articles.All.First().ID);
            Assert.AreEqual(barbieDoll.ID, _articles.All.Last().ID);
        }

        [Test]
        public void Delete_And_Insert_Reuse_Storage_Space()
        {
            var barbieDoll = Article.BarbieDoll;
            var spinalTapDvd = Article.SpinalTapDvd;

            _articles.Persist(barbieDoll);
            _articles.Persist(spinalTapDvd);

            _marcello.Journal.Apply (); //make sure the journal is applied to the backing stream

            var storageSize = ((InMemoryStream)_provider.GetStream("Article")).BackingStream.Length;
            _articles.Destroy(barbieDoll);
            _articles.Persist(barbieDoll);

            _marcello.Journal.Apply (); //make sure the journal is applied to the backing stream

            var newStorageSize = ((InMemoryStream)_provider.GetStream("Article")).BackingStream.Length;
            Assert.AreEqual(storageSize, newStorageSize);
        }

        [Test]
        public void Multiple_Delete_And_Insert_Reuse_Storage_Space()
        {
            var barbieDoll = Article.BarbieDoll;
            var spinalTapDvd = Article.SpinalTapDvd;
            var toiletPaper = Article.ToiletPaper;

            _articles.Persist(barbieDoll);
            _articles.Persist(spinalTapDvd);
            _articles.Persist(toiletPaper);

            _marcello.Journal.Apply (); //make sure the journal is applied to the backing stream

            var storageSize = ((InMemoryStream)_provider.GetStream("Article")).BackingStream.Length;
            _articles.Destroy(barbieDoll);
            _articles.Destroy(toiletPaper);

            _articles.Persist(barbieDoll);
            _articles.Persist(toiletPaper);
            _marcello.Journal.Apply (); //make sure the journal is applied to the backing stream

            var newStorageSize = ((InMemoryStream)_provider.GetStream("Article")).BackingStream.Length;
            Assert.AreEqual(storageSize, newStorageSize);
        }

        [Test]
        public void Save_To_File_Stream()
        {
            var fileStreamProvider =  new FileStorageStreamProvider("//Users/markmeeus/documents/tmp/marcello");
            var marcello = new Marcello (fileStreamProvider);

            var toiletPaper = Article.ToiletPaper;
            var spinalTapDvd = Article.SpinalTapDvd;
            var barbieDoll = Article.BarbieDoll;

            var articles = marcello.Collection<Article>();

            articles.Persist(toiletPaper);
            articles.Persist(spinalTapDvd);
            articles.Persist(barbieDoll);

            var articleNames = articles.All.Select(a => a.Name).ToList();

            Assert.AreEqual(new List<string>{toiletPaper.Name, spinalTapDvd.Name, barbieDoll.Name }, articleNames);
        }
    }
}

