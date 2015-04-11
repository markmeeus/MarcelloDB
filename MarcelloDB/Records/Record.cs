using System;
using System.Diagnostics;
using MarcelloDB.Buffers;

namespace MarcelloDB.Records
{
    internal class Record
    {
        internal RecordHeader Header { get; set;}

        private ByteBuffer _data;
        internal ByteBuffer Data
        {
            get{return _data; }
            set
            {
                if (value.Length > this.Header.AllocatedDataSize)
                {
                    throw new Exception("PANIC: Data cannot exceed AllocatedDataSize");
                }

                this.Header.DataSize = value.Length;
                _data = value;
            }
        }


        internal Record()
        {
            Header = RecordHeader.New();
        }

        internal Int32 ByteSize
        {
            get
            {
                return RecordHeader.ByteSize + Header.AllocatedDataSize;
            }
        }

        internal ByteBuffer AsBuffer(Marcello session)
        {
            var buffer = session.ByteBufferManager.Create(this.ByteSize);
            var headerBuffer = Header.AsBuffer(session);

            Array.Copy(headerBuffer.Bytes, buffer.Bytes, headerBuffer.Length);             
            Array.Copy(Data.Bytes, 0, buffer.Bytes, headerBuffer.Length, Data.Length);

            return buffer;
        }

        internal static Record FromBuffer(Marcello session, Int64 address, ByteBuffer buffer)
        {
            var header = RecordHeader.FromBuffer(session, address, buffer);

            var data = session.ByteBufferManager.Create(header.DataSize);
                     
            Array.Copy(buffer.Bytes, RecordHeader.ByteSize, data.Bytes, 0, header.DataSize);                
                
            return new Record(){
                Header = header,
                Data = data
            };
        }
    }
}

