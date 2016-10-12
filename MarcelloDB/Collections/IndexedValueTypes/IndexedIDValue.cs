using System;
using MarcelloDB.Collections;
using MarcelloDB.Records;
using MarcelloDB.Index;
using System.Collections.Generic;

namespace MarcelloDB.Collections
{
    internal class IndexedIDValue<TObj, TID> : IndexedValue<TObj, TID>
    {
        internal IndexedIDValue():base(null, null)
        {
            this.PropertyName = "ID";
        }

        internal Func<TObj, TID> IDValueFunction { get; set; }

        internal override IEnumerable<object> GetKeys(object o, long address)
        {
            return new object[]{IDValueFunction((TObj)o)};
        }

        internal  override void RegisterKey(object key,
            Int64 address,
            Session session,
            RecordManager recordManager,
            string indexName)
        {
            var index = new RecordIndex<TID>(
                this.Session,
                recordManager,
                indexName,
                this.Session.SerializerResolver.SerializerFor<Node<TID>>()
            );

            index.Register((TID)key, address);
        }

        internal  override void UnRegisterKey(object key,
            Int64 address,
            Session session,
            RecordManager recordManager,
            string indexName)
        {
            var index = new RecordIndex<TID>(
                this.Session,
                recordManager,
                indexName,
                this.Session.SerializerResolver.SerializerFor<Node<TID>>()
            );

            index.UnRegister((TID)key);
        }
    }
}

