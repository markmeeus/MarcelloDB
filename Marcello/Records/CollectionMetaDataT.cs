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

        }

        internal void Update(CollectionMetaDataRecord record)
        {

        }
    }
}

