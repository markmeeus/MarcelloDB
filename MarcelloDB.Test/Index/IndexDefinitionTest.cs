    using System;
using NUnit.Framework;
using MarcelloDB.Test.Classes;
using MarcelloDB.Index;
using System.Collections.Generic;
using MarcelloDB.Collections;
using System.Linq;

namespace MarcelloDB.Test.Index
{
    class TestDefinition : IndexDefinition<Article>
    {
        public IndexedValue<Article, string> Name{ get; set; }

        public IndexedValue<Article, string> CustomDescription
        {
            get
            {
                return IndexedValue((Article article)=>{
                    return "Custom" + article.Description;
                });
            }
        }
    }

    [TestFixture]
    public class IndexDefinitionTest
    {
        [Test]
        public void IndexedValues_Returns_ID_IndexedValue()
        {
            var indexedValues = new TestDefinition().IndexedValues;
            var idIndexedValue = indexedValues.First(v => v is IndexedIDValue<Article>);
            Assert.AreEqual(Article.BarbieDoll.ID, idIndexedValue.GetValue(Article.BarbieDoll));
        }

        [Test]
        public void IndexedValues_Contains_IndexedValue_For_Empty_Property()
        {
            var indexValues = new TestDefinition().IndexedValues;
            Assert.NotNull(indexValues.FirstOrDefault(v => v.PropertyName == "Name"));
        }

        [Test]
        public void IndexedValues_Contains_Indexed_Value_For_Implemented_Property()
        {
            var indexValues = new TestDefinition().IndexedValues;
            Assert.NotNull(indexValues.FirstOrDefault(v => v.PropertyName == "CustomDescription"));
        }

        [Test]
        public void IndexedField_GetValue_Returns_Correct_Value_For_Empty_Property()
        {
            var indexValues = new TestDefinition().IndexedValues;
            var indexValue = indexValues.FirstOrDefault(v => v.PropertyName == "Name");
            Assert.AreEqual(Article.BarbieDoll.Name, indexValue.GetValue(Article.BarbieDoll));
        }

        [Test]
        public void IndexedField_GetValue_Returns_Correct_Value_For_Implemented_Property()
        {
            var indexValues = new TestDefinition().IndexedValues;
            var indexValue = indexValues.FirstOrDefault(v => v.PropertyName == "CustomDescription");
            Assert.AreEqual("Custom" + Article.BarbieDoll.Description, indexValue.GetValue(Article.BarbieDoll));
        }
    }
}

