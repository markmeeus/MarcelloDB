using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Marcello.Serialization
{
    public class ObjectProxy
    {
        object Obj { get; set; }

        public ObjectProxy(object obj)
        {
            Obj = obj;
        }

        public object ID
        {
            get
            {
                object id = null;

                foreach (var propertyName in IDProperties) 
                {
                    if(GetPropertyValue(propertyName, ref id))
                    {
                        return id;
                    }
                }
                    
                GetAttributedId (ref id);                   
                return id;
            }
        }
          
        #region private properties
        string[] IDProperties
        {
            get
            {
                return new String[] {
                    "ID", 
                    "Id", 
                    "id",
                    ClassName () + "ID",
                    ClassName () + "Id",
                    ClassName () + "id",
                };
            }
        }
        #endregion

        #region private methods
        bool GetAttributedId(ref object id)
        {
            var attributedProperty = GetPropertyWithAttribute(typeof(IDAttribute));
            if (attributedProperty != null) 
            {
                id = ReadProperty(attributedProperty);
                return true;
            }
            return false;
        }

        bool GetPropertyValue(string propertyName, ref object id)
        {
            if(HasProperty(propertyName))
            {
                id = ReadProperty(propertyName);
                return true;
            }                       
            return false;
        }
        #endregion

        #region reflection
        string ClassName()
        {
            return Obj.GetType ().Name;
        }

        PropertyInfo GetPropertyInfo(string propertyName)
        {
            return Obj.GetType()
                .GetTypeInfo()
                .DeclaredProperties
                .Where(p => p.Name == propertyName).FirstOrDefault();
        }

        PropertyInfo GetPropertyWithAttribute(Type attributeType)
        {
            return Obj.GetType()
                .GetTypeInfo()
                .DeclaredProperties
                .Where(p => p.GetCustomAttribute(attributeType) != null)
                .FirstOrDefault();
        }

        bool HasProperty(string propertyName)
        {
            return GetPropertyInfo(propertyName) != null;
        }

        object ReadProperty(string propertyName)
        {
            var prop = GetPropertyInfo(propertyName);
            if (prop != null) {
                return ReadProperty(prop);
            }
            return null;
        }

        object ReadProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod.Invoke(this.Obj, new object[0]);
        }
        #endregion
    }
}

