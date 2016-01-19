using System;
using NUnit.Framework;
using MarcelloDB.Test.Classes;
using MarcelloDB.Index;
using System.Collections.Generic;

namespace MarcelloDB.Test.Index
{
    class TestDefinitionWithProp
    {
        public string Name {get; set;}
    }

    class TestDefinitionWithMethod
    {
        public string Description(Article a){
            return "Custom" + a.Description;
        }
    }

    class TestDefinition
    {
        public string Name{ get; }

        public string Description(Article a){
            return "Custom" + a.Description;
        }
    }

    [TestFixture]
    public class IndexDefinitionTest
    {

        [Test]
        public void GetIndexedFieldDescriptors_Returns_ID_Descriptor()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinitionWithProp, Article>();
            Assert.AreEqual("ID", descriptors[0].Name);
        }

        [Test]
        public void GetIndexedFieldDescriptors_Returns_ID_Descriptor_Value()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinitionWithProp, Article>();
            Assert.AreEqual(Article.BarbieDoll.ID, descriptors[0].ValueFunc(Article.BarbieDoll));
        }

        [Test]
        public void GetIndexedFieldDescriptors_Returns_Descriptor_For_Property()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinitionWithProp, Article>();
            Assert.AreEqual("Name", descriptors[1].Name);
        }

        [Test]
        public void GetIndexedFieldDescriptors_Returns_Descriptor_For_Method()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinitionWithMethod, Article>();
            Assert.AreEqual("Description", descriptors[1].Name);
        }

        [Test]
        public void GetIndexedFieldDescriptors_Returns_Descriptor_For_Props_And_Methods()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinition, Article>();
            Assert.AreEqual("Name", descriptors[1].Name);
            Assert.AreEqual("Description", descriptors[2].Name);
        }

        [Test]
        public void GetIndexedFieldDescriptors_Sets_ValueFunc_For_Property()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinitionWithProp, Article>();
            Assert.AreEqual(Article.BarbieDoll.Name, descriptors[1].ValueFunc(Article.BarbieDoll));
        }

        [Test]
        public void GetIndexedFieldDescriptors_Sets_ValueFunc_For_Method()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinitionWithMethod, Article>();
            Assert.AreEqual("Custom" + Article.BarbieDoll.Description, descriptors[1].ValueFunc(Article.BarbieDoll));
        }
    }
}

