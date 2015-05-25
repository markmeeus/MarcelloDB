using System;

namespace MarcelloDB.Exceptions
{
    public class IDMissingException : Exception
    {
        public IDMissingException(string Message) : base(Message)
        {
        }
    }
}

