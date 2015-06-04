using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MarcelloDB.Attributes;

namespace MarcelloDB.Serialization
{
    internal class ObjectProxy<T>
    {
        T Obj { get; set; }

        static Dictionary<Type, PropertyInfo> _idPropertyInfoCache = new Dictionary<Type, PropertyInfo>();

        IEnumerable<PropertyInfo> _properties;
        IEnumerable<PropertyInfo> Properties
        {
            get
            { 
                if (_properties == null)
                {
                    _properties = Obj.GetType().GetRuntimeProperties();
                }
                return _properties;
            }
        }

        public ObjectProxy(T obj)
        {
            Obj = obj;
        }

        public object ID
        {
            get
            {
                object id = null;
                var type = typeof(T);
                if(!_idPropertyInfoCache.ContainsKey(type))
                {
                    foreach (var propertyName in IDProperties) 
                    {
                        if(HasProperty(propertyName))
                        {                            
                            _idPropertyInfoCache[type] = GetPropertyInfo(propertyName);
                        }
                    }    
                }
                if (_idPropertyInfoCache.ContainsKey(type))
                {                    
                    id = ReadProperty(_idPropertyInfoCache[type]);
                }
                else
                {
                    GetAttributedId (ref id);                   
                }

                return id;
            }
        }
          
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
            id = ReadProperty(propertyName);
            return true;        
        }

        #region reflection

        string ClassName()
        {
            return Obj.GetType ().Name;
        }            

        PropertyInfo GetPropertyInfo(string propertyName)
        {
            return this.Properties
                .Where(p => p.Name == propertyName).FirstOrDefault();
        }

        PropertyInfo GetPropertyWithAttribute(Type attributeType)
        {
            return this.Properties
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

        #endregion //Reflection
    }
}

