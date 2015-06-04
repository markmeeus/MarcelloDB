using System;
using System.Collections.Generic;
using MarcelloDB;
using MarcelloDB.Serialization;
using MarcelloDB.AllocationStrategies;
using MarcelloDB.Storage;
using MarcelloDB.Records;

namespace MarcelloDB.Collections
{
    public class CollectionFile
    {
        Session Session { get; set; }

        RecordManager RecordManager { get; set; }

        string Name { get; set; }

        Dictionary<string, Collection> Collections { get; set; }

        internal CollectionFile(Session session, string name)
        {
            this.Session = session;
            this.Name = name;
            Collections = new Dictionary<string, Collection>();
            this.RecordManager = new RecordManager(
                new DoubleSizeAllocationStrategy(),
                new StorageEngine(this.Session, this.Name)
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
                        collectionName,
                        new BsonSerializer<T>(),
                        new DoubleSizeAllocationStrategy(),
                        this.RecordManager)                                       
                    );
            }
            return (Collection<T>)Collections[collectionName];
        }

    }
}

