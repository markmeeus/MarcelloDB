using System;
using NUnit.Framework;
using Marcello.Indexing;
using Marcello.Storage;
using Marcello.Test.Classes;


namespace Marcello.Test.Indexing
{
    [TestFixture]
	public class TestIndex
	{
        Marcello _session;
        Index _index;
        StorageEngine<Article> _storageEngine;

        [SetUp]
        public void Initialize()
        {
            _session = new Marcello(new InMemoryStreamProvider());
            _storageEngine = new StorageEngine<Article>(_session);
            _index = new Index(100, _session, _storageEngine);
        }

        [Test]
        public void Add_Address_To_Empty_Index_Adds_Block()
        {
            _index.Add(123, 456);
            _storageEngine.Read();
        }
	}
}

