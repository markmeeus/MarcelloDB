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
                        UnexpectedPropertyTypeError(prop.Name, prop.PropertyType.Name);
                    }
                    ValidateAttribute(definition, targetType, prop, propertyType.GenericTypeArguments[1]);
                }
                else
                {
                    UnexpectedPropertyTypeError(prop.Name, prop.PropertyType.Name);
                }
            }
        }

        static void ValidateAttribute<T>(IndexDefinition<T> definition, Type targetType, PropertyInfo property, Type expectedType)
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
                            targetType.Name, property.Name, targetProperty.PropertyType.Name, expectedType.Name);
                    }
                    ValidateGetterAndSetter(definition, property);
                }
                else
                {
                    NoTargetProperty(targetType.Name, property.Name);
                }
            }
        }

        static void ValidateGetterAndSetter<T>(IndexDefinition<T> definition, PropertyInfo property)
        {
            if (property.SetMethod == null)
            {
                NoSetMethodForDefaultIndexedValue(property.Name);
            }

            var buildMethod = property.PropertyType.GetRuntimeMethods().First(m => m.Name == "Build");
            var indexedValue = buildMethod.Invoke(null, new object[0]);

            property.SetValue(definition, indexedValue);

            if (property.GetValue(definition) != indexedValue)
            {
                BrokenSetMethodForDefaultIndexedValue(property.Name);
            }
        }

        static bool IsCustomIndexedValue<T>(IndexDefinition<T> definition, PropertyInfo property)
        {
            //Default properties should return null, custom properties return an IndexedValue
            return property.GetMethod.Invoke(definition, new object[0]) != null;
        }

        static void UnexpectedPropertyTypeError(string propertyName, string typeName)
        {
            throw new InvalidOperationException(
                string.Format("Unexpected property {0} of type {1}. " +
                    "Properties of an IndexDefinition should be of type IndexedValue<,>.",
                    propertyName, typeName)
            );
        }

        static void PropertyTypeMismatchError(
            string typeName,
            string propertyName,
            string indexedValueTypeName,
            string propertyTypeName)
        {
            throw new InvalidOperationException(
                string.Format("Property {0} cannot be indexed as a {1} because it is of type {2}." +
                    "\nDid you mean:" +
                    "\npublic IndexedValue<{3}, {2}> {0} {{get; set;}}",
                    propertyName, indexedValueTypeName, propertyTypeName, typeName)
            );
        }

        static void NoTargetProperty(string targetName, string propertyName)
        {
            throw new InvalidOperationException(
                string.Format("{0} cannot be indexed because {0} does not exist on {1}.", propertyName, targetName)
            );
        }

        static void NoSetMethodForDefaultIndexedValue(string propertyName)
        {
            throw new InvalidOperationException(
                string.Format("{0} does not have a setter. It looks like a default IndexedValue (because it returns null by default). Either add a setter or return base.IndexedValue()"
                    , propertyName)
            );
        }

        static void BrokenSetMethodForDefaultIndexedValue(string propertyName)
        {
            throw new InvalidOperationException(
                string.Format("{0} has a broken set method. It looks like a default IndexedValue (because it returns null by default), however, get is not returning what was set."
                    , propertyName)
            );
        }
    }
}

