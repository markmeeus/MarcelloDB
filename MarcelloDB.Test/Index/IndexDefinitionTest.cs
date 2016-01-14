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
        public void Get_IndexedFieldDescriptors_Returns_Descriptor_For_Property()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinitionWithProp>();
            Assert.AreEqual("Name", descriptors[0].Name);
            Assert.AreEqual(Article.BarbieDoll.Name, descriptors[0].GetValueFor(Article.BarbieDoll));
        }

        [Test]
        public void Get_IndexedFieldDescriptors_Returns_Descriptor_For_Method()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinitionWithMethod>();
            Assert.AreEqual("Description", descriptors[0].Name);
            Assert.AreEqual("Custom" + Article.BarbieDoll.Description, descriptors[0].GetValueFor(Article.BarbieDoll));
        }

        [Test]
        public void Get_IndexedFieldDescriptors_Returns_Descriptor_For_Props_And_Methods()
        {
            var descriptors = IndexDefinition.GetIndexedFieldDescriptors<TestDefinition>();
            Assert.AreEqual("Name", descriptors[0].Name);
            Assert.AreEqual("Description", descriptors[1].Name);
        }
    }
}

