using System;
using NUnit.Framework;
using MarcelloDB.Index;
using MarcelloDB.Test.Classes;
using System.Collections.Generic;
using MarcelloDB.Collections;

namespace MarcelloDB.Test.Index
{
    [TestFixture]
    public class IndexDefinitionValidatorTest
    {
        class DefinitionWithWrongProperty : IndexDefinition<Article>
        {
            public string Description { get; set; }
        }

        [Test]
        public void Throws_When_Property_Is_No_IndexedValue()
        {
            Assert.Throws<InvalidOperationException>(()=>{
                IndexDefinitionValidator.Validate<Article, DefinitionWithWrongProperty>();
            });
        }

        [Test]
        public void Throws_With_Correct_Message_When_Property_Is_No_IndexedValue()
        {
            var message = string.Empty;
            try{
                IndexDefinitionValidator.Validate<Article, DefinitionWithWrongProperty>();
            }catch(Exception e){
                message = e.Message;
            }
            Assert.AreEqual("Unexpected property Description of type String. Properties of an IndexDefinition should be of type IndexedValue<,>.", message);
        }

        class DefinitionWithWrongGenericProperty : IndexDefinition<Article>
        {
            public List<string> Description { get; set; }
        }

        [Test]
        public void Throws_When_Generic_Property_Is_No_IndexedValue()
        {
            Assert.Throws<InvalidOperationException>(()=>{
                IndexDefinitionValidator.Validate<Article, DefinitionWithWrongGenericProperty>();
            });
        }

        [Test]
        public void Throws_With_Correct_Message_When_GenericProperty_Is_No_IndexedValue()
        {
            var message = string.Empty;
            try{
                IndexDefinitionValidator.Validate<Article, DefinitionWithWrongGenericProperty>();
            }catch(Exception e){
                message = e.Message;
            }
            Assert.AreEqual("Unexpected property Description of type List`1. Properties of an IndexDefinition should be of type IndexedValue<,>."
                , message);
        }

        class DefinitionWithWrongAttributePropertyType : IndexDefinition<Article>
        {
            public IndexedValue<Article, int> Description { get; set; }
        }

        [Test]
        public void Throws_When_Property_Has_Wrong_Attribute_Type()
        {
            Assert.Throws<InvalidOperationException>(()=>{
                IndexDefinitionValidator.Validate<Article, DefinitionWithWrongAttributePropertyType>();
            });
        }

        [Test]
        public void Throws_With_Correct_Message_When_Property_Has_Wrong_Attribute_Type()
        {
            var message = string.Empty;
            try{
                IndexDefinitionValidator.Validate<Article, DefinitionWithWrongAttributePropertyType>();
            }catch(Exception e){
                message = e.Message;
            }
            Assert.AreEqual("Property Description cannot be indexed as a String because it is of type Int32." +
                "\nDid you mean:" +
                "\npublic IndexedValue<Article, Int32> Description {get; set;}"
                , message);
        }

        class DefinitionWithNonExistingProperty : IndexDefinition<Article>
        {
            public IndexedValue<Article, string> NoSuchProperty { get; set; }
        }

        [Test]
        public void Throws_When_Property_Does_Not_Exists_On_Target()
        {
            Assert.Throws<InvalidOperationException>(()=>{
                IndexDefinitionValidator.Validate<Article, DefinitionWithNonExistingProperty>();
            });
        }

        [Test]
        public void Throws_With_Correct_Message_When_Property_Does_Not_Exists_On_Target()
        {
            var message = string.Empty;
            try{
                IndexDefinitionValidator.Validate<Article, DefinitionWithNonExistingProperty>();
            }catch(Exception e){
                message = e.Message;
            }
            Assert.AreEqual("NoSuchProperty cannot be indexed because NoSuchProperty does not exist on Article."
                , message);
        }

        class DefinitionWithNoSetter : IndexDefinition<Article>
        {
            public IndexedValue<Article, string> Description { get { return null; } }
        }

        [Test]
        public void Throws_When_Null_Returning_Property_Has_No_Setter()
        {
            Assert.Throws<InvalidOperationException>(()=>{
                IndexDefinitionValidator.Validate<Article, DefinitionWithNoSetter>();
            });
        }

        [Test]
        public void Throws_With_Correct_Message_When_Null_Returning_Property_Has_No_Setter()
        {
            var message = string.Empty;
            try{
                IndexDefinitionValidator.Validate<Article, DefinitionWithNoSetter>();
            }catch(Exception e){
                message = e.Message;
            }
            Assert.AreEqual("Description does not have a setter. It looks like a default IndexedValue (because it returns null by default). Either add a setter or return base.IndexedValue()"
                , message);
        }

        class DefinitionWithBrokenSetter : IndexDefinition<Article>
        {
            public IndexedValue<Article, string> Description { get{ return null;} set { }}
        }

        [Test]
        public void Throws_When_Null_Returning_Property_Has_Broken_Setter()
        {
            Assert.Throws<InvalidOperationException>(()=>{
                IndexDefinitionValidator.Validate<Article, DefinitionWithBrokenSetter>();
            });
        }

        [Test]
        public void Throws_With_Correct_Message_When_Null_Returning_Property_Has_Broken_Setter()
        {
            var message = string.Empty;
            try{
                IndexDefinitionValidator.Validate<Article, DefinitionWithBrokenSetter>();
            }catch(Exception e){
                message = e.Message;
            }
            Assert.AreEqual("Description has a broken set method. It looks like a default IndexedValue (because it returns null by default), however, get is not returning what was set."
                , message);
        }

    }
}

