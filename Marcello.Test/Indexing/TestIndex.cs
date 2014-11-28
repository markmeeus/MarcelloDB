using System;
using NUnit.Framework;
using Marcello.Indexing;
using Marcello.Storage;
using Marcello.Test.Classes;
using Marcello.Records;
using Marcello.Collections;

namespace Marcello.Test.Indexing
{   
    [TestFixture]
	public class TestIndex
	{
        Marcello _session;
        Collection<Article> _articles;
        Index<Article> _index;

        [SetUp]
        public void Initialize()
        {
            _session = new Marcello(new InMemoryStreamProvider());
            _articles = _session.Collection<Article>();
            _index = new Index<Article>(_articles);
        }

        [Test]
        public void Registers_Single_Address()
        {
            var barbieDoll = Article.BarbieDoll;
            _index.RegisterAddress(barbieDoll, 1000);
            Assert.AreEqual(1000, _index.GetAddressOf(barbieDoll));
        }
	}
}

