using System;
using System.Collections.Generic;
using System.Collections;

namespace MarcelloDB.Collections
{
    class ObjectByIDEnumerator<T, TID> : IEnumerable<T>{

        internal IEnumerable<TID> IDs { get; set; }

        internal Collection<T, TID> Collection { get; set; }

        internal ObjectByIDEnumerator(Collection<T, TID> collection, IEnumerable<TID> ids){
            this.IDs = ids;
            this.Collection = collection;
        }

        #region IEnumerable implementation
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var id in this.IDs)
            {
                yield return this.Collection.Find(id);
            }
        }
        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
        #endregion
    }
}

