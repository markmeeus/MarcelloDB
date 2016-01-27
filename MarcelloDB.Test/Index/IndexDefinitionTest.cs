    using System;
using NUnit.Framework;
using MarcelloDB.Test.Classes;
using MarcelloDB.Index;
using System.Collections.Generic;
using MarcelloDB.Collections;
using System.Linq;

namespace MarcelloDB.Test.Index
{
    class TestDefinitionWithProp : IndexDefinition<Article>
    {
        public IndexedValue<Article, string> Name {get; set;}
    }

    class TestDefinitionWithCustomProp : IndexDefinition<Article>
    {
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

    class TestDefinition : IndexDefinition<Article>
    {
        public IndexedValue<Article, string> Name{ get; set; }

        public IndexedValue<Article, string> CustomDescription
        {
            get
            {
                return new IndexedValue<Article, string>((article)=>{
                    return "Custom" + article.Description;
                });
            }
        }
    }

    [TestFixture]
    public class IndexDefinitionTest
    {

        [Test]
        public void GetIndexedFieldDescriptors_Returns_ID_Descriptor()
        {
            var descriptors = new TestDefinitionWithProp().Descriptors;
            Assert.AreEqual("ID", descriptors[0].Name);
        }

        [Test]
        public void GetIndexedFieldDescriptors_Returns_ID_Descriptor_Value()
        {
            var descriptors = new TestDefinitionWithProp().Descriptors;
            Assert.AreEqual(Article.BarbieDoll.ID, descriptors[0].ValueFunc(Article.BarbieDoll));
        }

        [Test]
        public void GetIndexedFieldDescriptors_Returns_Descriptor_For_Default_Property()
        {
            var descriptors = new TestDefinitionWithProp().Descriptors;
            Assert.AreEqual("Name", descriptors[1].Name);
        }

        [Test]
        public void GetIndexedFieldDescriptors_Returns_Descriptor_For_Custom_Property()
        {
            var descriptors = new TestDefinitionWithProp().Descriptors;
            Assert.IsNotNull(descriptors.Where((d)=>d.Name=="Description"));
        }

        [Test]
        public void GetIndexedFieldDescriptors_Returns_Descriptor_For_Default_And_Custom()
        {
            var descriptors = new TestDefinition().Descriptors;
            Assert.IsNotNull(descriptors.Where((d)=>d.Name=="Name"));
            Assert.IsNotNull(descriptors.Where((d)=>d.Name=="Description"));
        }

        [Test]
        public void GetIndexedFieldDescriptors_Sets_ValueFunc_For_Default_Prop()
        {
            var descriptors = new TestDefinitionWithProp().Descriptors;
            Assert.AreEqual(Article.BarbieDoll.Name, descriptors[1].ValueFunc(Article.BarbieDoll));
        }

        [Test]
        public void GetIndexedFieldDescriptors_Sets_ValueFunc_For_Custom_Prop()
        {
            var descriptors = new TestDefinitionWithCustomProp().Descriptors;
            Assert.AreEqual("Custom" + Article.BarbieDoll.Description, descriptors[1].ValueFunc(Article.BarbieDoll));
        }
    }
}

