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
    public partial class IndexDefinition<T>
    {
        internal List<IndexedValue> IndexedValues { get; set; }

        internal bool Building { get; set; }

        public IndexDefinition()
        {
            this.IndexedValues = new List<IndexedValue>();
        }

        internal void Initialize()
        {
            this.Building = true;
            this.BuildIndexedValues();
            this.Building = false;
        }

        internal void SetContext(Collection collection,
            Session session,
            RecordManager recordManager,
            IObjectSerializer<T> serializer)
        {
            foreach (var indexedValue in this.IndexedValues)
            {
                ((dynamic)indexedValue).SetContext(
                    collection, session, recordManager, serializer, indexedValue.PropertyName);
            }
        }

        protected void BuildIndexedValues()
        {
            foreach (var prop in this.GetType().GetRuntimeProperties())
            {
                if(prop.DeclaringType == this.GetType()){
                    var propertyType = prop.PropertyType;
                    this.IndexedValues.Add(BuildIndexedValue(prop));
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

        #region protected but part of public api
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

        protected UniqueIndexedValue<T, TAttribute> UniqueIndexedValue<TAttribute>
        (Func<T, TAttribute> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new UniqueIndexedValue<T, TAttribute>(valueFunc);
            }
            else
            {
                return (UniqueIndexedValue<T, TAttribute>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }

        protected IndexedList<T, TAttribute> IndexedList<TAttribute>
            (Func<T, IEnumerable<TAttribute>> valueFunc, [CallerMemberName] string callerMember = "")
        {
            if (this.Building)
            {
                return new IndexedList<T, TAttribute>(valueFunc);
            }
            else
            {
                return (IndexedList<T, TAttribute>)IndexedValues.First(v => v.PropertyName == callerMember);
            }
        }


        #endregion
    }
}

