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
            Int64 lengthToCopy = sourceData.Length;
            Int64 sourceIndex = 0;
            Int64 targetIndex = 0;

            if (sourceAddress < targetAddress) 
            {
                sourceIndex = (targetAddress - sourceAddress);
                lengthToCopy = sourceData.Length - sourceIndex;
            }

            if (sourceAddress > targetAddress) 
            {
                targetIndex = (sourceAddress - targetAddress);
                lengthToCopy = targetData.Length - targetIndex;
            }

            //max length to copy to not overrun the target array
            lengthToCopy = Math.Min(lengthToCopy, targetData.Length - targetIndex);
            //max length to copy to not overrrun the source array
            lengthToCopy = Math.Min(lengthToCopy, sourceData.Length - sourceIndex);

            if (lengthToCopy > 0) 
            {
                Array.Copy(sourceData, (int)sourceIndex, targetData, (int)targetIndex, (int)lengthToCopy);
            }

        }
    }
}

