using System;
using NUnit.Framework;
using Marcello.Storage;
using Marcello.Test.Classes;

namespace Marcello.Test.Storage
{
    [TestFixture]
    public class TestStorageEngine
    {
        StorageEngine<Article> _engine;
        Marcello _session;

        [SetUp]
        public void Initialize()
        {
            _engine = new StorageEngine<Article>(_session);
        }

        [Test]
        public void Writes_To_chain()
        {
        }
    }
}

