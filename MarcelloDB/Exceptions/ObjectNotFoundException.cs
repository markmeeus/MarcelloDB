using System;

namespace MarcelloDB.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string message) : base(message)
        {
        }
    }
}

