    using System;
using NUnit.Framework;
using MarcelloDB.Test.Classes;
using MarcelloDB.Index;
using System.Collections.Generic;
using MarcelloDB.Collections;
using System.Linq;
using MarcelloDB.Records;

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
            var indexedValues = new TestDefinition().IndexedValues;
            Assert.NotNull(indexedValues.FirstOrDefault(v => v.PropertyName == "Name"));
        }

        [Test]
        public void IndexedValues_Contains_Indexed_Value_For_Implemented_Property()
        {
            var indexedValues = new TestDefinition().IndexedValues;
            Assert.NotNull(indexedValues.FirstOrDefault(v => v.PropertyName == "CustomDescription"));
        }

        [Test]
        public void IndexedField_GetValue_Returns_Correct_Value_For_Empty_Property()
        {
            var indexedValues = new TestDefinition().IndexedValues;
            var indexValue = indexedValues.FirstOrDefault(v => v.PropertyName == "Name");
            Assert.AreEqual(Article.BarbieDoll.Name, indexValue.GetValue(Article.BarbieDoll));
        }

        [Test]
        public void IndexedField_GetIndexKey_Returns_IDValue_For_IDIndexedValue()
        {
            var indexedValues = new TestDefinition().IndexedValues;
            var indexKey = indexedValues.First(v => v is IndexedIDValue<Article>)
                .GetKey(Article.BarbieDoll, 123);
            Assert.AreEqual(Article.BarbieDoll.ID, indexKey);
        }

        [Test]
        public void IndexedField_GetIndexKey_Returns_Correct_Key_For_Empty_Property()
        {
            var indexedValues = new TestDefinition().IndexedValues;
            var indexKey = (ValueWithAddressIndexKey) indexedValues.FirstOrDefault(v => v.PropertyName == "Name")
                .GetKey(Article.BarbieDoll, 123);
            Assert.AreEqual(Article.BarbieDoll.Name, indexKey.V);
            Assert.AreEqual(123, indexKey.A);
        }

        [Test]
        public void IndexedField_GetIndexKey_Returns_Correct_Key_For_Custom_Property()
        {
            var indexedValues = new TestDefinition().IndexedValues;
            var indexKey = (ValueWithAddressIndexKey) indexedValues.FirstOrDefault(v => v.PropertyName == "CustomDescription")
                .GetKey(Article.BarbieDoll, 123);
            Assert.AreEqual("Custom" + Article.BarbieDoll.Description, indexKey.V);
            Assert.AreEqual(123, indexKey.A);
        }

        [Test]
        public void IndexedField_GetValue_Returns_Correct_Value_For_Implemented_Property()
        {
            var indexedValues = new TestDefinition().IndexedValues;
            var indexValue = indexedValues.FirstOrDefault(v => v.PropertyName == "CustomDescription");
            Assert.AreEqual("Custom" + Article.BarbieDoll.Description, indexValue.GetValue(Article.BarbieDoll));
        }
    }
}

