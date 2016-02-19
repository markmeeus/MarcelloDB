using System;
using System.Reflection;
using MarcelloDB.Collections;
using System.Linq;

namespace MarcelloDB.Index
{
    public class IndexDefinitionValidator
    {
        public IndexDefinitionValidator()
        {
        }

        public static void Validate<T, TIndexDef>() where TIndexDef: IndexDefinition<T>, new()
        {
            var definition = new TIndexDef();
            definition.Building = true;
            var definitionType = typeof(TIndexDef);
            foreach (var prop in definitionType.GetRuntimeProperties())
            {
                ValidatePropertyType(definition, typeof(T), prop, definitionType);
            }
        }

        static void ValidatePropertyType<T>(IndexDefinition<T> definition, Type targetType, PropertyInfo prop, Type definitionType)
        {
            if (prop.DeclaringType == definitionType)
            {
                var propertyType = prop.PropertyType;
                if (propertyType.IsConstructedGenericType)
                {
                    if (propertyType.GetGenericTypeDefinition() != typeof(IndexedValue<, >))
                    {
                        UnexpectedPropertyTypeError(definitionType.Name,prop.Name, prop.PropertyType.Name);
                    }
                    ValidateAttribute(definition, definitionType, targetType, prop, propertyType.GenericTypeArguments[1]);
                }
                else
                {
                    UnexpectedPropertyTypeError(definitionType.Name, prop.Name, prop.PropertyType.Name);
                }
            }
        }

        static void ValidateAttribute<T>(IndexDefinition<T> definition, Type definitionType, Type targetType, PropertyInfo property, Type expectedType)
        {
            if (!IsCustomIndexedValue(definition, property))
            {
                var targetProperty = targetType.GetRuntimeProperties()
                    .FirstOrDefault((prop) => prop.Name == property.Name);

                if (targetProperty != null)
                {
                    if (targetProperty.PropertyType != expectedType)
                    {
                        PropertyTypeMismatchError(
                            definitionType.Name, targetType.Name, property.Name, targetProperty.PropertyType.Name, expectedType.Name);
                    }
                    ValidateGetterAndSetter(definition, definitionType, property);
                }
                else
                {
                    NoTargetProperty(definitionType.Name, targetType.Name, property.Name);
                }
            }
        }

        static void ValidateGetterAndSetter<T>(IndexDefinition<T> definition, Type definitionType, PropertyInfo property)
        {
            if (property.SetMethod == null)
            {
                NoSetMethodForDefaultIndexedValue(definitionType.Name, property.Name);
            }

            var buildMethod = property.PropertyType.GetRuntimeMethods().First(m => m.Name == "Build");
            var indexedValue = buildMethod.Invoke(null, new object[0]);

            property.SetValue(definition, indexedValue);

            if (property.GetValue(definition) != indexedValue)
            {
                BrokenSetMethodForDefaultIndexedValue(definitionType.Name, property.Name);
            }
        }

        static bool IsCustomIndexedValue<T>(IndexDefinition<T> definition, PropertyInfo property)
        {
            //Default properties should return null, custom properties return an IndexedValue
            return property.GetMethod.Invoke(definition, new object[0]) != null;
        }

        static void UnexpectedPropertyTypeError(string indexDefinitionName, string propertyName, string typeName)
        {
            throw new InvalidOperationException(
                string.Format("{0}: Unexpected property {1} of type {2}. " +
                    "Properties of an IndexDefinition should be of type IndexedValue<,>.",
                    indexDefinitionName, propertyName, typeName)
            );
        }

        static void PropertyTypeMismatchError(
            string indexDefinitionName,
            string typeName,
            string propertyName,
            string indexedValueTypeName,
            string propertyTypeName)
        {
            throw new InvalidOperationException(
                string.Format("{0}: Property {1} cannot be indexed as a {2} because it is of type {3}." +
                    "\nDid you mean:" +
                    "\npublic IndexedValue<{4}, {3}> {1} {{get; set;}}",
                    indexDefinitionName, propertyName, indexedValueTypeName, propertyTypeName, typeName)
            );
        }

        static void NoTargetProperty(string indexDefinitionName, string targetName, string propertyName)
        {
            throw new InvalidOperationException(
                string.Format("{0}: {1} cannot be indexed because {1} does not exist on {2}.",
                    indexDefinitionName, propertyName, targetName)
            );
        }

        static void NoSetMethodForDefaultIndexedValue(string indexDefinitionName, string propertyName)
        {
            throw new InvalidOperationException(
                string.Format("{0}: {1} does not have a setter. It looks like a default IndexedValue (because it returns null by default). Either add a setter or return base.IndexedValue()"
                    , indexDefinitionName, propertyName)
            );
        }

        static void BrokenSetMethodForDefaultIndexedValue(string indexDefinitionName, string propertyName)
        {
            throw new InvalidOperationException(
                string.Format("{0}: {1} has a broken set method. It looks like a default IndexedValue (because it returns null by default), however, get is not returning what was set."
                    , indexDefinitionName, propertyName)
            );
        }
    }
}

