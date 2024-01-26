using System;

namespace Insurance.Api.Application.Exceptions
{
    public class BadRequestException : BaseBusinessException
    {
        public BadRequestException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
