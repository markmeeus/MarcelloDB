using System;
using NUnit.Framework;
using MarcelloDB.Collections;
using System.Collections.Generic;
using MarcelloDB.Index;
using System.Linq;

namespace MarcelloDB.Test.Integration
{
    public class Indexable
    {
        public int     ID    { get; set; }
        public List<string> Tags { get; set; }

        public static Indexable Create(int id)
        {
            return new Indexable
            {
                ID    = id,
                Tags = Enumerable.Range(1, id).Select((i)=>String.Format("Tag{0}", i)).ToList()
            };
        }
    }

    [TestFixture]
    public class IndexListTest
    {
        Session _session;
        CollectionFile _collectionFile;

        TestPlatform _platform;

        [SetUp]
        public void Setup()
        {
            _platform = new TestPlatform();
            _session = new Session(_platform, "/");
            _collectionFile = _session["indexables"];
        }

        class TestIndexDefinition : IndexDefinition<Indexable>
        {
            public IndexedList<Indexable, string> Tags
            {
                get {
                    return IndexedList((o) => o.Tags);
                }
            }
        }

        [Test]
        public void Contains_Find_Single_Item()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, TestIndexDefinition>("indexables", i => i.ID);
            indexables.Persist(Indexable.Create(1));
            var tagged1 = indexables.Indexes.Tags.Contains("Tag1").ToList();
            Assert.AreEqual(1, tagged1[0].ID);
        }

        [Test]
        public void Contains_Find_Multiple_Items()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, TestIndexDefinition>("indexables", i => i.ID);
            indexables.Persist(Indexable.Create(1));
            indexables.Persist(Indexable.Create(2));

            var tagged1 = indexables.Indexes.Tags.Contains("Tag1").ToList();

            Assert.IsTrue(tagged1.Select((t) => t.ID).Contains(1));
            Assert.IsTrue(tagged1.Select((t) => t.ID).Contains(2));
        }

        [Test]
        public void Contains_Returns_Only_Distinct_Objects()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, TestIndexDefinition>("indexables", i => i.ID);

            var indexable = new Indexable
                {
                    ID = 1,
                    Tags = new List<string>{"Tag1", "Tag1"}
                };

            indexables.Persist(indexable);

            var tagged1 = indexables.Indexes.Tags.Contains("Tag1").ToList();

            Assert.AreEqual(1, tagged1.Count);
            Assert.AreEqual(1, tagged1[0].ID);
        }

        [Test]
        public void ContainsAnyOf_Returns_Objects()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, TestIndexDefinition>("indexables", i => i.ID);

            indexables.Persist(new Indexable
                {
                    ID = 1,
                    Tags = new List<string>{ "Tag1", "anyother" }
                });
            indexables.Persist(new Indexable
                {
                    ID = 2,
                    Tags = new List<string>{ "Tag2", "anyother" }
                });
            indexables.Persist(new Indexable
                {
                    ID = 3,
                    Tags = new List<string>{ "Tag3", "anyother" }
                });

            var result = indexables.Indexes.Tags.ContainsAny(new string[]{"Tag2", "Tag3"}).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(new int[]{2,3}, result.Select((a)=>a.ID));
        }

        [Test]
        public void ContainsAnyOf_Returns_Distinct_Objects()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, TestIndexDefinition>("indexables", i => i.ID);

            indexables.Persist(new Indexable
                {
                    ID = 1,
                    Tags = new List<string>{ "Tag1", "anyother" }
                });
            indexables.Persist(new Indexable
                {
                    ID = 2,
                    Tags = new List<string>{ "Tag2", "Tag3" }
                });
            indexables.Persist(new Indexable
                {
                    ID = 3,
                    Tags = new List<string>{ "Tag3", "anyother" }
                });

            var result = indexables.Indexes.Tags.ContainsAny(new string[]{"Tag2", "Tag3"}).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(new int[]{2,3}, result.Select((a)=>a.ID));
        }
    }
}

