using System;
using System.Collections;
using System.Collections.Generic;

namespace Marcello
{
	internal class CollectionEnumerator<T> : IEnumerable<T>
	{
        RecordManager RecordManager  { get; set; }
        IObjectSerializer<T> ObjectSerializer { get; set; }
    
        public CollectionEnumerator(RecordManager recordManager)
        {   
            RecordManager = recordManager;
        }

        #region IEnumerable implementation

        public IEnumerator<T> GetEnumerator ()
        {
            var record = RecordManager.GetFirstRecord ();

            while (record != null) {
                var obj = ObjectSerializer.Deserialize(record.data);
                yield return obj;
                record = RecordManager.GetNextRecord (record);
            }
        }
        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }

        #endregion
    }
}

