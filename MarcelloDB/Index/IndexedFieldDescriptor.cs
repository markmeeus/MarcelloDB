using System;
using System.Reflection;
using System.Linq;

namespace MarcelloDB
{
    internal class IndexedFieldDescriptor
    {
        internal string Name {get; set;}

        internal Func<object, object> ValueFunc {get; set;}

        internal IndexedFieldDescriptor()
        {
        }

        internal object GetValueFor(object o)
        {
            var oType = o.GetType();
            if (ValueFunc != null)
            {
                return this.ValueFunc(o);
            }
            else
            {
                //no value function, fallback to property by name
                var prop = oType.GetRuntimeProperties()
                    .FirstOrDefault(p => p.Name == this.Name);

                if (prop != null)
                {
                    return prop.GetMethod.Invoke(o, new object[0]);
                }
            }

            return null;
        }
    }
}

