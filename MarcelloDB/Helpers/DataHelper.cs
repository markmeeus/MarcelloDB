using System;

namespace MarcelloDB.Helpers
{
    internal class DataHelper
    {
        internal static void CopyData(
            Int64 sourceAddress, 
            byte[] sourceData,
            Int64 targetAddress,
            byte[] targetData)
        {
            var lengthToCopy = sourceData.Length;
            var sourceIndex = 0;
            var targetIndex = 0;

            if (sourceAddress < targetAddress) 
            {
                sourceIndex = (int)(targetAddress - sourceAddress);
                lengthToCopy = sourceData.Length - sourceIndex;
            }

            if (sourceAddress > targetAddress) 
            {
                targetIndex = (int)(sourceAddress - targetAddress);
                lengthToCopy = targetData.Length - targetIndex;
            }

            //max length to copy to not overrun the target array
            lengthToCopy = Math.Min(lengthToCopy, targetData.Length - targetIndex);
            //max length to copy to not overrrude the source array
            lengthToCopy = Math.Min(lengthToCopy, sourceData.Length - sourceIndex);

            if (lengthToCopy > 0) 
            {
                Array.Copy(sourceData, sourceIndex, targetData, targetIndex, lengthToCopy);
            }

        }
    }
}

