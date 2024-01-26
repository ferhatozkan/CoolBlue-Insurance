using System;

namespace Insurance.Api.Exceptions
{
    public class BaseCustomException : Exception
    {
        protected BaseCustomException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
