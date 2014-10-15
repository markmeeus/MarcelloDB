using System;

namespace Marcello
{
	internal class RecordHeader
	{
		internal long Next	 { get; set;}
		internal long Previous { get; set;}

		static internal int ByteSize {
			get {
				return sizeof(long) + sizeof(long);
			}
		}

		internal byte[] AsBytes(){
			var bytes = new byte[ByteSize];
			BitConverter.GetBytes (this.Next).CopyTo (bytes, 0);
			BitConverter.GetBytes (this.Previous).CopyTo (bytes, sizeof(long));
			return bytes;
		}
	}
}

