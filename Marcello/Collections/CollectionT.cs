using System;

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
			StorageEngine.Append(obj);
		}
	}
}

