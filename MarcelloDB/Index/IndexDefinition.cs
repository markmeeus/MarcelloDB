using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;

namespace MarcelloDB.Index
{
    internal class IndexDefinition{
        internal static List<IndexedFieldDescriptor> GetIndexedFieldDescriptors<TDef>() where TDef: new()
        {
            var descriptors = new List<IndexedFieldDescriptor>();
            var definitionType = typeof(TDef);

            foreach (var prop in definitionType.GetRuntimeProperties()
                .Where(m => m.DeclaringType == definitionType))
            {
                descriptors.Add(new IndexedFieldDescriptor(){ Name = prop.Name });
            }

            foreach (var method in definitionType.GetRuntimeMethods()
                .Where(m => m.DeclaringType == definitionType && !m.IsSpecialName))
            {
                descriptors.Add(new IndexedFieldDescriptor(){
                    Name = method.Name,
                    ValueFunc = (o) => method.Invoke(new TDef(), new object[]{o})
                });
            }
            return descriptors;
        }
    }
}

