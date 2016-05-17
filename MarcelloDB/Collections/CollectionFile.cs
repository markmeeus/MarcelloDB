using System;
using System.Collections.Generic;
using MarcelloDB;
using MarcelloDB.Serialization;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Storage;
using MarcelloDB.Records;
using MarcelloDB.Transactions;
using MarcelloDB.Index;

namespace MarcelloDB.Collections
{
    public class CollectionFile : SessionBoundObject
    {
        RecordManager RecordManager { get; set; }

        string Name { get; set; }

        Dictionary<string, Collection> Collections { get; set; }

        internal CollectionFile(Session session, string name) : base(session)
        {
            this.Name = name;
            Collections = new Dictionary<string, Collection>();
            this.RecordManager = new RecordManager(
                this.Session,
                new StorageEngine(this.Session, this.Name));
        }

        public Collection<T, TID> Collection<T, TID>(string collectionName, Func<T, TID> idFunc)
        {
            if (collectionName == null)
            {
                collectionName = typeof(T).Name.ToLower();
            }
            if(!Collections.ContainsKey(collectionName)){
                Collections.Add (collectionName,
                    new Collection<T, TID> (this.Session,
                        this,
                        collectionName,
                        this.Session.SerializerResolver.SerializerFor<T>(),
                        this.RecordManager,
                        idFunc)
                    );
            }

            var retVal = Collections[collectionName] as Collection<T, TID>;
            if (retVal == null)
            {
                ThrowCollectionDefinedForOtherType<T>(collectionName);
            }
            return (Collection<T, TID>)Collections[collectionName];
        }

        public Collection<T, TID, TIndexDef> Collection<T, TID, TIndexDef>(
            string collectionName,
            Func<T, TID> idFunc) where TIndexDef: IndexDefinition<T>, new()
        {
            if (collectionName == null)
            {
                collectionName = typeof(T).Name.ToLower();
            }
            if(!Collections.ContainsKey(collectionName)){
                Collections.Add (collectionName,
                    new Collection<T, TID, TIndexDef> (this.Session,
                        this,
                        collectionName,
                        this.Session.SerializerResolver.SerializerFor<T>(),
                        this.RecordManager,
                        idFunc)
                );
            }

            var retVal = Collections[collectionName] as Collection<T, TID, TIndexDef>;
            if (retVal == null)
            {
                ThrowCollectionDefinedForOtherType<T>(collectionName);
            }
            return (Collection<T, TID, TIndexDef>)Collections[collectionName];
        }

        void ThrowCollectionDefinedForOtherType<T>(string collectionName)
        {
            throw new InvalidOperationException(
                string.Format("Collection with name \"{0}\" is allready defined as Collection<{1}, TID>" +
                    " and cannot be used as a Collection<{2}, TID>.",
                    collectionName,
                    Collections[collectionName].GetType().GenericTypeArguments[0].Name,
                    typeof(T).Name
                )
            );
        }
    }
}

