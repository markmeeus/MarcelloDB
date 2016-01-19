using System;
using System.Reflection;
using System.Linq;

namespace MarcelloDB
{
    internal class IndexedFieldDescriptor
    {
        internal string Name {get; set;}

        internal Func<object, object> ValueFunc {get; set;}

        internal bool IsID { get; set; }

        internal IndexedFieldDescriptor()
        {
        }
    }
}

