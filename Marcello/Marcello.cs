using System;
using Marcello.AllocationStrategies;
using Marcello.Collections;
using Marcello.Serialization;

namespace Marcello
{
    public class Marcello
    {
        internal IStorageStreamProvider StreamProvider { get; set; }

        public Marcello (IStorageStreamProvider streamProvider)
        {
            StreamProvider = streamProvider;
        }

        public Collection<T> GetCollection<T>()
        {
            return new Collection<T>(this, 
                new BsonSerializer<T>(), 
                new DoubleSizeAllocationStrategy());
        }
    }
}

