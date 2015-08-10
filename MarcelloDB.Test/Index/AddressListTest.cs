using System;
using NUnit.Framework;
using MarcelloDB.Index;
using System.Collections.Generic;

namespace MarcelloDB.Test.Index
{
    [TestFixture]
    public class AddressListTest
    {
        AddressList _addressList;

        [SetUp]
        public void Initialize()
        {
            _addressList = new AddressList();
        }

        [Test]
        public void Test_New_Address_List_Is_Not_Dirty()
        {
            Assert.IsFalse(new AddressList().Dirty);
        }

        [Test]
        public void Add_Adds()
        {
            _addressList.Add(123);
            Assert.IsTrue(_addressList.IndexOf(123) > -1);
        }

        [Test]
        public void Add_Sets_Dirty()
        {
            _addressList.Add(123);
            Assert.IsTrue(_addressList.Dirty);
        }

        [Test]
        public void Add_Tracks_Changes()
        {
            _addressList.Add(1);
            _addressList.Add(2);
            Assert.AreEqual(new List<Int64>{ 1, 2 }, _addressList.Added);
        }

        [Test]
        public void Removing_Dirty_Flag_Clears_Added()
        {
            _addressList.Add(1);
            _addressList.ClearChanges();
            Assert.IsEmpty(_addressList.Added);
        }

        [Test]
        public void Removing_Dirty_Flag_Clears_Removed()
        {
            _addressList.Add(1);
            _addressList.ClearChanges();
            _addressList.RemoveAt(0);
            _addressList.ClearChanges();
            Assert.IsEmpty(_addressList.Removed);
        }

        [Test]
        public void Insert_Inserts()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.Insert(1, 15);
            Assert.AreEqual(_addressList[0], 10);
            Assert.AreEqual(_addressList[1], 15);
            Assert.AreEqual(_addressList[2], 20);
        }

        [Test]
        public void Insert_Sets_Dirty()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.ClearChanges();
            _addressList.Insert(1, 15);
            Assert.IsTrue(_addressList.Dirty);
        }

        [Test]
        public void Insert_Tracks_Changes()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.ClearChanges();
            _addressList.Insert(1, 11);
            _addressList.Insert(1, 12);
            Assert.AreEqual(new List<Int64>{ 11, 12 }, _addressList.Added);
        }

        [Test]
        public void Indexer_Sets()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList[1] = 30;
            Assert.AreEqual(30, _addressList[1]);
        }

        [Test]
        public void Indexer_Sets_Dirty()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.ClearChanges();
            _addressList[1] = 30;
            Assert.IsTrue(_addressList.Dirty);
        }

        [Test]
        public void Indexer_Tracks_Changes()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.ClearChanges();
            _addressList[1] = 30;
            Assert.AreEqual(new List<Int64>{ 30 }, _addressList.Added);
            Assert.AreEqual(new List<Int64>{ 20 }, _addressList.Removed);
        }

        [Test]
        public void Indexer_Tracks_Nothing_When_Value_Is_Same()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.ClearChanges();
            _addressList[1] = 20;
            Assert.AreEqual(new List<Int64>{  }, _addressList.Added);
            Assert.AreEqual(new List<Int64>{  }, _addressList.Removed);
            Assert.IsFalse(_addressList.Dirty);
        }

        [Test]
        public void AddRange_Adds_Range()
        {
            _addressList.Add(10);
            _addressList.AddRange(new List<Int64>{ 20, 30 });
            Assert.AreEqual(20, _addressList[1]);
            Assert.AreEqual(30, _addressList[2]);
        }

        [Test]
        public void AddRange_Sets_Dirty()
        {
            _addressList.AddRange(new List<Int64>{ 20, 30 });
            Assert.IsTrue(_addressList.Dirty);
        }

        [Test]
        public void AddRange_Tracks_Changes()
        {
            _addressList.AddRange(new List<Int64>{ 20, 30 });
            Assert.AreEqual(new List<Int64>{ 20, 30 }, _addressList.Added);
        }


        [Test]
        public void RemoveRange_Removes()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.Add(30);
            _addressList.RemoveRange(1, 2);
            Assert.AreEqual(1, _addressList.Count);
            Assert.AreEqual(10, _addressList.First());
        }

        [Test]
        public void RemoveRange_Sets_Dirty()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.Add(30);
            _addressList.ClearChanges();
            _addressList.RemoveRange(1, 2);
            Assert.IsTrue(_addressList.Dirty);
        }

        [Test]
        public void RemoveRange_Tracks_Changes()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.Add(30);
            _addressList.ClearChanges();
            _addressList.RemoveRange(1, 2);
            Assert.AreEqual(new List<Int64>{ 20, 30 }, _addressList.Removed);
        }

        [Test]
        public void RemoveAt_Removes()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.Add(30);
            _addressList.RemoveAt(1);
            Assert.AreEqual(2, _addressList.Count);
            Assert.AreEqual(10, _addressList[0]);
            Assert.AreEqual(30, _addressList[1]);
        }

        [Test]
        public void RemoveAt_Sets_Dirty()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.Add(30);
            _addressList.ClearChanges();
            _addressList.RemoveAt(1);
            Assert.IsTrue(_addressList.Dirty);
        }

        [Test]
        public void RemoveAt_Tracks_Changes()
        {
            _addressList.Add(10);
            _addressList.Add(20);
            _addressList.Add(30);
            _addressList.RemoveAt(1);
            Assert.AreEqual(new List<Int64>{ 20 }, _addressList.Removed);
        }
    }
}

