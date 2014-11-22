﻿using System;
using System.Collections;
using System.Collections.Generic;
using Marcello.Records;
using Marcello.Serialization;

namespace Marcello.Collections
{
	internal class CollectionEnumerator<T> : IEnumerable<T>
	{
        RecordManager<T> RecordManager  { get; set; }

        IObjectSerializer<T> Serializer { get; set; }

        Marcello Session { get; set; }

        public CollectionEnumerator(
            Marcello session,
            RecordManager<T> recordManager, 
            IObjectSerializer<T> serializer)
        {   
            Session = session;
            RecordManager = recordManager;
            Serializer = serializer;
        }

        #region IEnumerable implementation

        public IEnumerator<T> GetEnumerator()
        {
            lock(this.Session.SyncLock){
                var record = RecordManager.GetFirstRecord();
                while (record != null) {
                    var obj = Serializer.Deserialize(record.Data);
                    yield return obj;
                    record = RecordManager.GetNextRecord(record);
                }
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