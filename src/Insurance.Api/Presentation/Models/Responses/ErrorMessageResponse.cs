using System.Collections.Generic;

namespace Insurance.Api.Presentation.Models.Responses
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
