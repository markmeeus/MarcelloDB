using System;

namespace Marcello.Records
{
    internal class RecordBuilder
    {
        internal Record Build(byte[] data){
            return Build(data, data.Length);
        }

        internal Record Build(byte[] data, Int32 alloctionSize){
            return new Record()
            {
                Data = data, 
                Header = new RecordHeader(){
                    DataSize = data.Length,
                    AllocatedDataSize = alloctionSize
                }
            };
        }
    }
}

