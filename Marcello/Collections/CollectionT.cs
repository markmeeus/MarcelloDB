using System;
using System.Collections.Generic;

namespace Marcello
{
	public class Collection<T>
	{
		IStorageEngine StorageEngine{ get; set;}

		IObjectSerializer Serializer {get;set;}

		internal Collection (IStorageEngine storageEngine, IObjectSerializer serializer)
		{
			StorageEngine = storageEngine;
			Serializer = serializer;
		}


		public IEnumerable<T> All{
			get{
				return null;
			}
		}

		public void Persist(T obj){

			//
			//var identifier = ObjectIdentification.GetObjectId(obj);
			// find existing object and update if necessary
			//currently only append implemented
			AppendObject (obj);

		}

		public void Destroy(T obj){

		}


		void AppendObject(object obj){
			var bytes = Serializer.Serialize(obj);
			var record = new Record ();
			StorageEngine.Append(record.AsBytes());
		}
	}
}

