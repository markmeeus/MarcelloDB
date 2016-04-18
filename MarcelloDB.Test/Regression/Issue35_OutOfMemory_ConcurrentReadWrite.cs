using System;
using NUnit.Framework;
using System.Threading.Tasks;
using MarcelloDB.Collections;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using MarcelloDB.Index;

namespace MarcelloDB.Test.Regression.Issue35
{
    public class ArticleIndexes : IndexDefinition<Article>
    {
        public IndexedValue<Article, string> Name { get; set; }
    }

    public class Article
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Writer
    {
        bool Running { get; set; }

        Collection<Article> Articles { get; set; }

        int CurrentID { get; set; }

        public Writer(Collection<Article> articles, int startID)
        {
            this.Articles = articles;
            this.CurrentID = startID;
        }

        public void Start()
        {
            this.Running = true;
            new Thread(() =>
                {
                    while(this.Running)
                    {
                        this.CurrentID++;
                        this.Articles.Persist(new Article{
                            ID = this.CurrentID,
                            Name = String.Format("article {0}", this.CurrentID)
                        });
                    }
                }).Start();
        }
        public void Stop()
        {
            this.Running = false;
        }
    }

    [TestFixture]
    public class Issue35_Crash_On_Concurrent_ReadWrite
    {
        TestPlatform _platform;

        [SetUp]
        public void Setup()
        {
            _platform = new TestPlatform();
        }

        [Test]
        public void ShouldNotThrow_When_Multiple_Threads_Are_Writing_While_Reading_ID_Index()
        {
            var session = new Session(_platform, "/");
            var articles = session["data"].Collection<Article, ArticleIndexes>("articles");

            var writer1 = new Writer(articles, 1);
            var writer2 = new Writer(articles, int.MaxValue / 2);
            writer1.Start();
            writer2.Start();
            Assert.DoesNotThrow(() =>
                {
                    foreach(var i in Enumerable.Range(0, 20)){
                        articles.All.Take(100).ToList();
                        Thread.Sleep(10);
                    }
                });

            writer1.Stop();
            writer2.Stop();
        }

        [Test]
        public void ShouldNotThrow_When_Multiple_Threads_Are_Writing_While_Reading_Custom_Index()
        {
            var session = new Session(_platform, "/");
            var articles = session["data"].Collection<Article, ArticleIndexes>("articles");

            var writer1 = new Writer(articles, 1);
            var writer2 = new Writer(articles, int.MaxValue / 2);
            writer1.Start();
            writer2.Start();
            Assert.DoesNotThrow(() =>
                {
                    foreach(var i in Enumerable.Range(0, 20)){
                        articles.Indexes.Name.All.Take(100).ToList();
                        Thread.Sleep(10);
                    }
                });

            writer1.Stop();
            writer2.Stop();
        }

        [Test]
        public void ShouldNotThrow_When_Multiple_Threads_Are_Writing_While_Reading_Custom_Index_Keys()
        {
            var session = new Session(_platform, "/");
            var articles = session["data"].Collection<Article, ArticleIndexes>("articles");

            var writer1 = new Writer(articles, 1);
            var writer2 = new Writer(articles, int.MaxValue / 2);
            writer1.Start();
            writer2.Start();
            Assert.DoesNotThrow(() =>
                {
                    foreach(var i in Enumerable.Range(0, 20)){
                        articles.Indexes.Name.All.Keys.Take(100).ToList();
                        Thread.Sleep(10);
                    }
                });

            writer1.Stop();
            writer2.Stop();
        }

        [Test]
        public void ShouldNotThrow_When_Enumerable_Changed_Due_To_Rolled_Back_Transaction()
        {
            var session = new Session(_platform, "/");
            var articles = session["data"].Collection<Article>("articles");

            Assert.DoesNotThrow(() =>
                {
                    IEnumerable<Article> enumerable = null;
                    // Add first articles
                    articles.Persist(new Article{
                        ID = 1,
                        Name = String.Format("article {0}", 1),
                        Description = String.Format("article {0}", 1)
                    });

                    articles.Persist(new Article{
                        ID = 2,
                        Name = String.Format("article {0}", 2),
                        Description = String.Format("article {0}", 2)
                    });
                    try{
                        session.Transaction(()=>{

                            //add second product;
                            articles.Persist(new Article{
                                ID = 3,
                                Name = String.Format("article {0}", 3),
                                Description = String.Format("article {0}", 1)
                            });
                            //add second product;
                            articles.Persist(new Article{
                                ID = 4,
                                Name = String.Format("article {0}", 4),
                                Description = String.Format("article {0}", 4)
                            });
                            // Get enumerator
                            enumerable = articles.All;
                            // make enumerable load it's data
                            enumerable.ToList();

                            throw(new Exception("rollback"));
                        });
                    }catch(Exception){}

                    // insert bigger object

                    articles.Persist(new Article{
                        ID = 3,
                        Name = "A bigger article, a bigger article, a bigger article, A bigger article, a bigger article, a bigger article, A bigger article, a bigger article, a bigger article, A bigger article, a bigger article, a bigger article",
                        Description = String.Format("article {0}", 3)
                    });

                    enumerable.ToList();

                });
        }
    }
}

