using System;
using NUnit.Framework;
using MarcelloDB.Serialization;
using MarcelloDB.Test.Classes;
using System.Collections.Generic;
using System.Linq;
using MarcelloDB.Buffers;

namespace MarcelloDB.Test.Serialization
{
    [TestFixture]
    public class BsonSerializerTest
    {        
        [Test]
        public void Serializes_And_Deserializes_Simple_Object()
        {
            var byteBufferManager = new ByteBufferManager();
            var serializer = new BsonSerializer<Article>();
            var article = Article.SpinalTapDvd;
            var deserializedArticle = (Article)serializer.Deserialize(
                byteBufferManager.FromBytes(serializer.Serialize(article))
            );
            Assert.AreEqual(article.ID, deserializedArticle.ID);
            Assert.AreEqual(article.Name, deserializedArticle.Name);
        }

        [Test]
        public void Serializes_Subclasses_In_list()
        {
            var byteBufferManager = new ByteBufferManager();
            var serializer = new BsonSerializer<List<Article>>();
            var list = new List<Article>{ Article.BarbieDoll, Food.Bread };
            var deserializedList = (List<Article>)serializer.Deserialize(
                byteBufferManager.FromBytes(serializer.Serialize(list))
            );
            Assert.AreEqual(list[0].ID, deserializedList[0].ID);
            Assert.AreEqual(list[0].Name, deserializedList[0].Name);
            Assert.AreEqual(list[1].ID, deserializedList[1].ID);
            Assert.AreEqual(list[1].Name, deserializedList[1].Name);
            Assert.AreEqual(((Food)list[1]).Expires.ToString(), ((Food)deserializedList[1]).Expires.ToString());
        }
    }
}

