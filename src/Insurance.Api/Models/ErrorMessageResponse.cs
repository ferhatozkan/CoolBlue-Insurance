using System.Collections.Generic;
using System.Linq;

namespace Insurance.Api.Models
{
    public class ErrorMessageResponse
    {
        public List<ErrorMessage> ErrorMessages { get; }

        private ErrorMessageResponse(List<ErrorMessage> errorMessages)
        {
            ErrorMessages = errorMessages;
        }

        public static ErrorMessageResponse Create(string message)
        {
            return new ErrorMessageResponse(new List<ErrorMessage> { new(message) });
        }
    }
}
