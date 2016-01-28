using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;
using MarcelloDB.Serialization;
using MarcelloDB.Records;
using MarcelloDB.Collections;
using System.Runtime.CompilerServices;

namespace MarcelloDB.Index
{
    public abstract class IndexDefinition
    {
        /// <summary>
        /// _customIndexedValues keeps a reference to every instantiated IndexedValue
        /// This is needed to populate it with the necessary references to access the relevant index
        /// </summary>
        protected Dictionary<string, object> _customIndexedValues = new Dictionary<string, object>();

        internal List<IndexedFieldDescriptor> Descriptors { get; set; }

        internal List<IndexedValue> IndexedValues { get; set; }

        internal IndexDefinition()
        {
            this.IndexedValues = new List<IndexedValue>();
            this.BuildIndexedFieldDescriptors();
        }

        internal static TIndexDef Build<TObj, TIndexDef>(
            string collectionName,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<TObj> serializer)
            where TIndexDef:IndexDefinition<TObj>, new()
        {
            var indexDefinition = new TIndexDef();

            foreach (var prop in typeof(TIndexDef).GetRuntimeProperties())
            {
                var propertyType = prop.PropertyType;
                if (propertyType.IsConstructedGenericType)
                {
                    if(propertyType.GetGenericTypeDefinition() == typeof(IndexedValue<,>)){
                        var typeArgs = propertyType.GenericTypeArguments;
                        var indexValue = (dynamic)prop.GetValue(indexDefinition);
                        ((dynamic)indexValue).SetContext(
                            collectionName, session, recordManager, serializer, prop.Name);
                    }
                }
            }
            return indexDefinition;
        }

        protected abstract void BuildIndexedFieldDescriptors();

    }

    public class IndexDefinition<T> : IndexDefinition
    {
        public IndexDefinition()
        {
            this.IndexedValues.Add(new IndexedIDValue()
                {
                    IDValueFunction = (o) => new ObjectProxy<T>((T)o).ID
                });

            foreach (var prop in this.GetType().GetRuntimeProperties())
            {
                var propertyType = prop.PropertyType;
                if (propertyType.IsConstructedGenericType)
                {
                    if(propertyType.GetGenericTypeDefinition() == typeof(IndexedValue<,>)){
                        var typeArgs = propertyType.GenericTypeArguments;
                        var indexedValue = (dynamic)prop.GetValue(this);
                        if (indexedValue == null)
                        {
                            //build and assign the IndexedValue<,>
                            var buildMethod = propertyType.GetRuntimeMethods().First(m => m.Name == "Build");
                            indexedValue = buildMethod.Invoke(null, new object[]{});
                            //Set the value in the property
                            prop.SetValue(this, indexedValue);
                        }
                        indexedValue.PropertyName = prop.Name;
                        this.IndexedValues.Add(indexedValue);
                    }
                }
            }
        }
        /// <summary>
        /// Creates (or reuses from cache) and IndexedValue
        /// IndexedValues are cached per CallerName
        /// </summary>
        protected IndexedValue<T, TAttribute> IndexedValue<TAttribute>
            (Func<T, TAttribute> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (!_customIndexedValues.ContainsKey(callerMember))
            {
                _customIndexedValues[callerMember] = new IndexedValue<T, TAttribute>(valueFunc);
            }

            return (IndexedValue<T, TAttribute>)_customIndexedValues[callerMember];
        }

        protected override void BuildIndexedFieldDescriptors()
        {
            var definitionType = this.GetType();
            this.Descriptors = new List<IndexedFieldDescriptor>();

            this.Descriptors.Add(new IndexedFieldDescriptor(){
                Name = "ID",
                IsID = true,
                ValueFunc = (o) => new ObjectProxy<T>((T)o).ID
            });

            foreach (var prop in definitionType.GetRuntimeProperties()
                .Where(m => m.DeclaringType == definitionType))
            {
                //if the get method returns an IndexedValue, it is a custom index field
                Func<object,object> valueFunc;

                object propertyValue = prop.GetValue(this);
                if (propertyValue == null)
                {
                    valueFunc = (o) => typeof(T)
                        .GetRuntimeProperty(prop.Name).GetMethod.Invoke(o, new object[0]);
                }
                else
                {
                    valueFunc = (o) =>
                        {
                            return (((dynamic)(dynamic)propertyValue).ValueFunction).Invoke((T)o);
                        };
                }
                this.Descriptors.Add(new IndexedFieldDescriptor(){
                    Name = prop.Name,
                    IsID = false,
                    ValueFunc = valueFunc
                });
            }
        }

    }
}

