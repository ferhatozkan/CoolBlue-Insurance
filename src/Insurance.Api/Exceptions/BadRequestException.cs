using System;

namespace Insurance.Api.Exceptions
{
    public class BadRequestException : BaseCustomException
    {
        public BadRequestException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
