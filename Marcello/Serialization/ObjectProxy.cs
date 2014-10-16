using System;
using System.Collections.Generic;

namespace Marcello.Serialization
{
    public class ObjectProxy
    {
        object Obj { get; set; }

        public ObjectProxy (object obj)
        {
            Obj = obj;
        }

        public string ID{
            get
            {
                return string.Empty;
            }
        }
            
        public IEnumerable<string> PropertyNames
        {
            get 
            {
                return new List<string> ();
            }
        }
    }
}

