using System;
using NUnit.Framework;
using MarcelloDB.Helpers;
using System.Collections.Generic;

namespace MarcelloDB.Test
{
    [TestFixture]
    public class ChangeTrackingListTest
    {
        ChangeTrackingList<int> _list;

        [SetUp]
        public void Initialize()
        {
            _list = new ChangeTrackingList<int>();
        }

        [Test]
        public void Test_New_List_Is_Not_Dirty()
        {
            Assert.IsFalse(new ChangeTrackingList<int>().Dirty);
        }

        [Test]
        public void Add_Adds()
        {
            _list.Add(123);
            Assert.IsTrue(_list.IndexOf(123) > -1);
        }

        [Test]
        public void Add_Sets_Dirty()
        {
            _list.Add(123);
            Assert.IsTrue(_list.Dirty);
        }

        [Test]
        public void Add_Tracks_Changes()
        {
            _list.Add(1);
            _list.Add(2);
            Assert.AreEqual(new List<int>{ 1, 2 }, _list.Added);
        }

        [Test]
        public void Removing_Dirty_Flag_Clears_Added()
        {
            _list.Add(1);
            _list.ClearChanges();
            Assert.IsEmpty(_list.Added);
        }

        [Test]
        public void Removing_Dirty_Flag_Clears_Removed()
        {
            _list.Add(1);
            _list.ClearChanges();
            _list.RemoveAt(0);
            _list.ClearChanges();
            Assert.IsEmpty(_list.Removed);
        }

        [Test]
        public void Insert_Inserts()
        {
            _list.Add(10);
            _list.Add(20);
            _list.Insert(1, 15);
            Assert.AreEqual(_list[0], 10);
            Assert.AreEqual(_list[1], 15);
            Assert.AreEqual(_list[2], 20);
        }

        [Test]
        public void Insert_Sets_Dirty()
        {
            _list.Add(10);
            _list.Add(20);
            _list.ClearChanges();
            _list.Insert(1, 15);
            Assert.IsTrue(_list.Dirty);
        }

        [Test]
        public void Insert_Tracks_Changes()
        {
            _list.Add(10);
            _list.Add(20);
            _list.ClearChanges();
            _list.Insert(1, 11);
            _list.Insert(1, 12);
            Assert.AreEqual(new List<int>{ 11, 12 }, _list.Added);
        }

        [Test]
        public void Indexer_Sets()
        {
            _list.Add(10);
            _list.Add(20);
            _list[1] = 30;
            Assert.AreEqual(30, _list[1]);
        }

        [Test]
        public void Indexer_Sets_Dirty()
        {
            _list.Add(10);
            _list.Add(20);
            _list.ClearChanges();
            _list[1] = 30;
            Assert.IsTrue(_list.Dirty);
        }

        [Test]
        public void Indexer_Tracks_Changes()
        {
            _list.Add(10);
            _list.Add(20);
            _list.ClearChanges();
            _list[1] = 30;
            Assert.AreEqual(new List<int>{ 30 }, _list.Added);
            Assert.AreEqual(new List<int>{ 20 }, _list.Removed);
        }

        [Test]
        public void Indexer_Tracks_Nothing_When_Value_Is_Same()
        {
            _list.Add(10);
            _list.Add(20);
            _list.ClearChanges();
            _list[1] = 20;
            Assert.AreEqual(new List<int>{  }, _list.Added);
            Assert.AreEqual(new List<int>{  }, _list.Removed);
            Assert.IsFalse(_list.Dirty);
        }

        [Test]
        public void AddRange_Adds_Range()
        {
            _list.Add(10);
            _list.AddRange(new List<int>{ 20, 30 });
            Assert.AreEqual(20, _list[1]);
            Assert.AreEqual(30, _list[2]);
        }

        [Test]
        public void AddRange_Sets_Dirty()
        {
            _list.AddRange(new List<int>{ 20, 30 });
            Assert.IsTrue(_list.Dirty);
        }

        [Test]
        public void AddRange_Tracks_Changes()
        {
            _list.AddRange(new List<int>{ 20, 30 });
            Assert.AreEqual(new List<int>{ 20, 30 }, _list.Added);
        }


        [Test]
        public void RemoveRange_Removes()
        {
            _list.Add(10);
            _list.Add(20);
            _list.Add(30);
            _list.RemoveRange(1, 2);
            Assert.AreEqual(1, _list.Count);
            Assert.AreEqual(10, _list.First());
        }

        [Test]
        public void RemoveRange_Sets_Dirty()
        {
            _list.Add(10);
            _list.Add(20);
            _list.Add(30);
            _list.ClearChanges();
            _list.RemoveRange(1, 2);
            Assert.IsTrue(_list.Dirty);
        }

        [Test]
        public void RemoveRange_Tracks_Changes()
        {
            _list.Add(10);
            _list.Add(20);
            _list.Add(30);
            _list.ClearChanges();
            _list.RemoveRange(1, 2);
            Assert.AreEqual(new List<int>{ 20, 30 }, _list.Removed);
        }

        [Test]
        public void RemoveAt_Removes()
        {
            _list.Add(10);
            _list.Add(20);
            _list.Add(30);
            _list.RemoveAt(1);
            Assert.AreEqual(2, _list.Count);
            Assert.AreEqual(10, _list[0]);
            Assert.AreEqual(30, _list[1]);
        }

        [Test]
        public void RemoveAt_Sets_Dirty()
        {
            _list.Add(10);
            _list.Add(20);
            _list.Add(30);
            _list.ClearChanges();
            _list.RemoveAt(1);
            Assert.IsTrue(_list.Dirty);
        }

        [Test]
        public void RemoveAt_Tracks_Changes()
        {
            _list.Add(10);
            _list.Add(20);
            _list.Add(30);
            _list.RemoveAt(1);
            Assert.AreEqual(new List<int>{ 20 }, _list.Removed);
        }
    }
}

