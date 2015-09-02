using System;
using MarcelloDB.Records;
using System.Collections.Generic;
using MarcelloDB.AllocationStrategies;

namespace MarcelloDB.Test
{
    class InMemoryRecordManager : IRecordManager
    {
        public Dictionary<Int64, Record> _records = new Dictionary<Int64, Record>();
        public Dictionary<string, Int64> _namedRecordAdresses = new Dictionary<string, long>();

        #region IRecordManager implementation
        public Record GetRecord(long address)
        {
            if (_records.ContainsKey(address))
            {
                return _records[address];
            }
            else
            {
                throw new ArgumentException("No record here: " + address.ToString());
            }
        }
        public Record AppendRecord(
            byte[] data,
            IAllocationStrategy allocationStrategy = null)
        {
            var record = new Record();
            record.Header.Address = _records.Values.Count + 1;
            record.Header.AllocatedDataSize = data.Length;
            record.Header.DataSize = data.Length;
            record.Data = data;
            _records[record.Header.Address] = record;
            return record;
        }
        public Record UpdateRecord(
            Record record,
            byte[] data,
            IAllocationStrategy allocationStrategy = null)
        {
            if (_records.ContainsKey(record.Header.Address))
            {
                if (data.Length > record.Header.AllocatedDataSize)
                {
                    return AppendRecord(data, allocationStrategy);
                }
                else
                {
                    _records[record.Header.Address].Data = data;
                    return _records[record.Header.Address];
                }
            }
            else
            {
                throw new ArgumentException("No record here: " + record.Header.Address.ToString());
            }
        }
        public void Recycle(long address)
        {
            if (_records.ContainsKey(address))
            {
                _records.Remove(address);
            }
            else
            {
                throw new ArgumentException("No record here: " + address.ToString());
            }
        }
        public void RegisterNamedRecordAddress(string name, long recordAddress)
        {
            if (!_namedRecordAdresses.ContainsKey(name))
            {
                _namedRecordAdresses[name] = recordAddress;
            }

        }
        public long GetNamedRecordAddress(string name)
        {
            if (_namedRecordAdresses.ContainsKey(name))
            {
                return _namedRecordAdresses[name];
            }
            else
            {
                throw new ArgumentException("No record named: " + name.ToString());
            }
        }
        #endregion
    }
}

