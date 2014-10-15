using System;

namespace Marcello
{
	internal interface IStorageEngine
	{

		Record GetFirstRecord();

		Record GetNextRecord(Record record);

		Record GetLastRecord();

		/// <summary>
		/// Writes the specified record on the specified location
		/// </summary>
		/// <param name="address">Address.</param>
		/// <param name="record">Record.</param>
		long Append(Record record);

		/// <summary>
		/// Writes the specified record on the specified location
		/// </summary>
		/// <param name="address">Address.</param>
		/// <param name="record">Record.</param>
		void Write(long address, Record record);


		/// <summary>
		/// Erase the data on the specified address.
		/// </summary>
		/// <param name="address">Address.</param>
		void Erase(long address);
	}
}

