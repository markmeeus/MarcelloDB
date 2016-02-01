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
    public class IndexDefinition<T>
    {
        internal List<IndexedValue> IndexedValues { get; set; }

        protected bool Building { get; set; }

        public IndexDefinition()
        {
            this.IndexedValues = new List<IndexedValue>();
            this.Building = true;
            this.BuildIndexedValues();
            this.Building = false;
        }

        internal void SetContext(string collectionName,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<T> serializer)
        {
            foreach (var indexedValue in this.IndexedValues)
            {
                ((dynamic)indexedValue).SetContext(
                    collectionName, session, recordManager, serializer, indexedValue.PropertyName);
            }
        }

        protected IndexedValue<T, TAttribute> IndexedValue<TAttribute>
            (Func<T, TAttribute> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new IndexedValue<T, TAttribute>(valueFunc);
            }
            else
            {
                return (IndexedValue<T, TAttribute>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }

        protected void BuildIndexedValues()
        {
            this.IndexedValues.Add(new IndexedIDValue<T>() {
                IDValueFunction = o =>
                    {
                        return new ObjectProxy<T>((T)o).ID;
                    }
            });
            foreach (var prop in this.GetType().GetRuntimeProperties())
            {
                var propertyType = prop.PropertyType;
                if (propertyType.IsConstructedGenericType)
                {
                    if (propertyType.GetGenericTypeDefinition() == typeof(IndexedValue<, >))
                    {
                        this.IndexedValues.Add(BuildIndexedValue(prop));
                    }
                }
            }
        }

        MarcelloDB.Collections.IndexedValue BuildIndexedValue(PropertyInfo prop)
        {
            var typeArgs = prop.PropertyType.GenericTypeArguments;
            var indexedValue = (dynamic)prop.GetValue(this);
            if (indexedValue == null)
            {
                //build and assign the IndexedValue<,>
                var buildMethod = prop.PropertyType.GetRuntimeMethods().First(m => m.Name == "Build");
                indexedValue = buildMethod.Invoke(null, new object[0]);
                //Set the value in the property
                prop.SetValue(this, indexedValue);
            }
            indexedValue.PropertyName = prop.Name;
            return indexedValue;
        }
    }
}

