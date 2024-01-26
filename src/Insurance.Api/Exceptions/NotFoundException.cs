using System;

namespace Insurance.Api.Exceptions
{
    public class NotFoundException : BaseCustomException
    {
        public NotFoundException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
