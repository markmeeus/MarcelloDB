using System;
using System.Linq;
using System.Collections.Generic;
using MarcelloDB.Records;
using MarcelloDB.Serialization;
using MarcelloDB.Index;
using System.Reflection;
using System.Collections;
using MarcelloDB.Index.BTree;
using MarcelloDB.Collections.Scopes;

namespace MarcelloDB.Collections
{
    public abstract class IndexedValue : SessionBoundObject
    {
        public IndexedValue(Session session) : base(session){}

        internal abstract object GetKey(object o, Int64 address);

        protected internal abstract void Register(object o, Int64 address);

        protected internal abstract void UnRegister(object o, Int64 address);

        protected internal string PropertyName { get; set; }
    }
}