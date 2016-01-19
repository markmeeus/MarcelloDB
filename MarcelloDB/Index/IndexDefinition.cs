using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;
using MarcelloDB.Serialization;

namespace MarcelloDB.Index
{
    internal class EmptyIndexDef
    {

    }

    internal class IndexDefinition{
        internal static List<IndexedFieldDescriptor> GetIndexedFieldDescriptors<TDef, T>() where TDef: new()
        {
            var descriptors = new List<IndexedFieldDescriptor>();
            var definitionType = typeof(TDef);

            descriptors.Add(new IndexedFieldDescriptor(){
                Name = "ID",
                IsID = true,
                ValueFunc = (o) => new ObjectProxy<T>((T)o).ID
            });

            foreach (var prop in definitionType.GetRuntimeProperties()
                .Where(m => m.DeclaringType == definitionType))
            {
                descriptors.Add(new IndexedFieldDescriptor(){
                    Name = prop.Name,
                    IsID = false,
                    ValueFunc = (o) => typeof(T)
                        .GetRuntimeProperty(prop.Name).GetMethod.Invoke(o, new object[0])
                });
            }

            foreach (var method in definitionType.GetRuntimeMethods()
                .Where(m => m.DeclaringType == definitionType && !m.IsSpecialName))
            {
                descriptors.Add(new IndexedFieldDescriptor(){
                    Name = method.Name,
                    IsID = false,
                    ValueFunc = (o) => method.Invoke(new TDef(), new object[]{o})
                });
            }
            return descriptors;
        }
    }
}

