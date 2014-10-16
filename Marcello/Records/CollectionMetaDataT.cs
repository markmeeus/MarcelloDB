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

		internal Record GetRecord()
		{
		}

		internal void Update(Record record)
		{

		}
	}
}

