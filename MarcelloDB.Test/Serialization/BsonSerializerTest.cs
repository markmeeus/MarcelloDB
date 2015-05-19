using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Test.Classes;
using System.Collections.Generic;
using System.Linq;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class BsonSerializerTest
    {
        BsonSerializer _serializer;

        [SetUp]
        public void Initialize()
        {
            _serializer = new BsonSerializer();
        }

        [Test]
        public void Serializes_And_Deserializes_Simple_Object()
        {
            var article = Article.SpinalTapDvd;
            var bytes = _serializer.Serialize(article);
            var deserializedArticle = (Article)_serializer.Deserialize(bytes);
            Assert.AreEqual(article.ID, deserializedArticle.ID);
            Assert.AreEqual(article.Name, deserializedArticle.Name);
        }

        [Test]
        public void Serializes_Subclasses_In_list()
        {
            var list = new List<Article>{ Article.BarbieDoll, Food.Bread };
            var bytes = _serializer.Serialize(list);
            var deserializedList = (List<Article>)_serializer.Deserialize(bytes);
            Assert.AreEqual(list[0].ID, deserializedList[0].ID);
            Assert.AreEqual(list[0].Name, deserializedList[0].Name);
            Assert.AreEqual(list[1].ID, deserializedList[1].ID);
            Assert.AreEqual(list[1].Name, deserializedList[1].Name);
            Assert.AreEqual(((Food)list[1]).Expires.ToString(), ((Food)deserializedList[1]).Expires.ToString());
        }
    }
}

