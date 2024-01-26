using System;

namespace Insurance.Api.Application.Exceptions
{
    public class BaseBusinessException : Exception
    {
        protected BaseBusinessException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
