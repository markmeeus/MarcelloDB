using System;
using System.Collections.Generic;

namespace MarcelloDB.Collections
{
    public class IndexEnumerator<TObj, TIndexKEy>
    {
        internal IndexEnumerator()
        {
        }

        public IEnumerable<TObj> Find(TIndexKEy indexKey)
        {
            return new List<TObj>(){default(TObj)};
        }
    }
}

