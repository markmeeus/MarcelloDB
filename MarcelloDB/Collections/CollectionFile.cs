using System;
using System.Collections.Generic;
using MarcelloDB;
using MarcelloDB.Serialization;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Storage;
using MarcelloDB.Records;
using MarcelloDB.Transactions;

namespace MarcelloDB.Collections
{
    public class CollectionFile
    {
        Session Session { get; set; }

        StorageEngine StorageEngine { get; set; }

        RecordManager RecordManager { get; set; }

        string Name { get; set; }

        Dictionary<string, Collection> Collections { get; set; }

        internal CollectionFile(Session session, string name)
        {
            this.Session = session;
            this.Name = name;
            Collections = new Dictionary<string, Collection>();
            this.StorageEngine = new StorageEngine(this.Session, this.Name);
            this.RecordManager = new RecordManager(
                new DoubleSizeAllocationStrategy(),
                this.StorageEngine
            );
        }

        public Collection<T> Collection<T>(string collectionName)
        {
            if (collectionName == null)
            {
                collectionName = typeof(T).Name.ToLower();
            }
            if(!Collections.ContainsKey(collectionName)){
                Collections.Add (collectionName,
                    new Collection<T> (this.Session,
                        this,
                        collectionName,
                        new BsonSerializer<T>(),
                        new DoubleSizeAllocationStrategy(),
                        this.RecordManager)
                    );
            }

            var retVal = Collections[collectionName] as Collection<T>;
            if (retVal == null)
            {
                ThrowCollectionDefinedForOtherType<T>(collectionName);
            }
            return (Collection<T>)Collections[collectionName];

        }

        void ThrowCollectionDefinedForOtherType<T>(string collectionName)
        {
            throw new InvalidOperationException(
                string.Format("Collection with name \"{0}\" is allready defined as Collection<{1}>" +
                    " and cannot be used as a Collection<{2}>.",
                    collectionName,
                    Collections[collectionName].GetType().GenericTypeArguments[0].Name,
                    typeof(T).Name
                )
            );
        }
    }
}

