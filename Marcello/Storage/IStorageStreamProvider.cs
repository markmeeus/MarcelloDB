using System;

namespace Marcello
{
	public interface IStorageStreamProvider
	{
		IStorageStream GetStream(string streamName);
	}
}

