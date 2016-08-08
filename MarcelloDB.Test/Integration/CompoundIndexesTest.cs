using System;
using NUnit.Framework;
using MarcelloDB.Collections;
using MarcelloDB.Index;
using System.Linq;

namespace MarcelloDB.Test.Integration.CompoundIndexesTest
{
    public class Indexable
    {
        public int     ID    { get; set; }
        public int     Prop1 { get; set; }
        public long    Prop2 { get; set; }
        public double  Prop3 { get; set; }
        public string  Prop4 { get; set; }
        public int     Prop5 { get; set; }
        public long    Prop6 { get; set; }
        public double  Prop7 { get; set; }
        public string  Prop8 { get; set; }

        public static Indexable CreateIndexable(int i)
        {
            return new Indexable
            {
                ID    = i,
                Prop1 = i,
                Prop2 = i,
                Prop3 = i,
                Prop4 = i.ToString(),
                Prop5 = i,
                Prop6 = i,
                Prop7 = i,
                Prop8 = i.ToString()
            };
        }
    }

    [TestFixture]
    public class CompoundIndexesTest
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

        class IndexableIndexDefinition2 : IndexDefinition<Indexable>
        {
            public CompoundIndexedValue<Indexable, int, long> Compound1And2
            {
                get{
                    return CompoundIndexedValue((indexable) =>
                        {
                            return CompoundValue.Build(indexable.Prop1, indexable.Prop2);
                        });
                }
            }
        }

        [Test]
        public void TestInsertCompoundOf2()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, IndexableIndexDefinition2>("indexables2", i => i.ID);
            indexables.Persist(Indexable.CreateIndexable(1));
            indexables.Persist(Indexable.CreateIndexable(2));
            indexables.Persist(Indexable.CreateIndexable(3));

            var item2 = indexables.Indexes.Compound1And2.Find(2, 2).First();
            Assert.AreEqual(2, item2.ID);
        }

        class IndexableIndexDefinition3 : IndexDefinition<Indexable>
        {
            public CompoundIndexedValue<Indexable, int, long, double> Compound1And2And3
            {
                get{
                    return CompoundIndexedValue((indexable) =>
                        {
                            return CompoundValue.Build(indexable.Prop1, indexable.Prop2, indexable.Prop3);
                        });
                }
            }
        }
        [Test]
        public void TestInsertCompoundOf3()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, IndexableIndexDefinition3>("indexables3", i => i.ID);
            indexables.Persist(Indexable.CreateIndexable(1));
            indexables.Persist(Indexable.CreateIndexable(2));
            indexables.Persist(Indexable.CreateIndexable(3));

            var item2 = indexables.Indexes.Compound1And2And3.Find(2, 2, 2).First();
            Assert.AreEqual(2, item2.ID);
        }

        class IndexableIndexDefinition4 : IndexDefinition<Indexable>
        {
            public CompoundIndexedValue<Indexable, int, long, double, string> Compound1And2And3And4
            {
                get{
                    return CompoundIndexedValue((indexable) =>
                        {
                            return CompoundValue.Build(
                                indexable.Prop1,
                                indexable.Prop2,
                                indexable.Prop3,
                                indexable.Prop4
                            );
                        });
                }
            }
        }

        [Test]
        public void TestInsertCompoundOf4()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, IndexableIndexDefinition4>("indexables3", i => i.ID);
            indexables.Persist(Indexable.CreateIndexable(1));
            indexables.Persist(Indexable.CreateIndexable(2));
            indexables.Persist(Indexable.CreateIndexable(3));

            var item2 = indexables.Indexes.Compound1And2And3And4.Find(2, 2, 2, "2").First();
            Assert.AreEqual(2, item2.ID);
        }

        [Test]
        public void TestAll()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, IndexableIndexDefinition4>("indexables4", i => i.ID);

            indexables.Persist(Indexable.CreateIndexable(3));
            indexables.Persist(Indexable.CreateIndexable(2));
            indexables.Persist(Indexable.CreateIndexable(1));

            var items = indexables.Indexes.Compound1And2And3And4.All.ToList();
                Assert.AreEqual(3, items.Count);

            Assert.AreEqual(1, items[0].ID);
            Assert.AreEqual(2, items[1].ID);
            Assert.AreEqual(3, items[2].ID);
        }

        [Test]
        public void TestGreaterThan3Compound_3Params()
        {
            var indexables =
                _collectionFile.Collection<Indexable, int, IndexableIndexDefinition3>("indexables3", i => i.ID);
            indexables.Persist(Indexable.CreateIndexable(1));
            indexables.Persist(Indexable.CreateIndexable(2));
            indexables.Persist(Indexable.CreateIndexable(3));

            var items = indexables.Indexes.Compound1And2And3.GreaterThan(2, 2, 1).ToList();

            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(2, items[0].ID);
            Assert.AreEqual(3, items[1].ID);
        }
    }
}

