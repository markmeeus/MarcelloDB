using System;

namespace Marcello
{
    internal class CollectionMetaData<T>
    {
        Marcello Session { get; set; }

        internal CollectionMetaData (Marcello session)
        {
            Session = session;
        }
       
        internal CollectionMetaDataRecord GetRecord()
        {
            return new CollectionMetaDataRecord();
        }

        internal void Update(CollectionMetaDataRecord record)
        {

        }
    }
}

