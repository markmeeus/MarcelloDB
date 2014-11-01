using System;
using System.Collections;
using System.Collections.Generic;

namespace Marcello
{
	internal class CollectionEnumerator<T> : IEnumerable<T>
	{
        RecordManager<T> RecordManager  { get; set; }

        IObjectSerializer<T> Serializer { get; set; }
        
        public CollectionEnumerator(RecordManager<T> recordManager, 
            IObjectSerializer<T> serializer)
        {   
            RecordManager = recordManager;
            Serializer = serializer;
        }

        #region IEnumerable implementation

        public IEnumerator<T> GetEnumerator()
        {
            var record = RecordManager.GetFirstRecord();

            while (record != null) {
                var obj = Serializer.Deserialize(record.Data);
                yield return obj;
                record = RecordManager.GetNextRecord(record);
            }
        }
        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}