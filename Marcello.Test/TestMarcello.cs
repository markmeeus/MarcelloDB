using NUnit.Framework;
using System;
using Marcello;

namespace Marcello.Test
{
	[TestFixture ()]
	public class Test
	{
		Marcello _marcello;

		[SetUp]
		public void Setup(){
			_marcello = new Marcello ();
		}

		[Test ()]
		public void TestGetCollectionReturnsACollection ()
		{
			var collection = _marcello.GetCollection<Article> ();
			Assert.NotNull (collection, "Collection should not be null");
		}
	}
}

