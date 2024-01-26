using System;

namespace Insurance.Api.Application.Exceptions
{
    public class NotFoundException : BaseBusinessException
    {
        public NotFoundException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
