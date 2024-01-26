using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Insurance.Api.Exceptions;
using Insurance.Api.Constants;
using Insurance.Api.Models;

namespace Insurance.Api.Filters
{
    public class InsuranceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case NotFoundException notFoundException:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.HttpContext.Response.ContentType = FieldConstants.Exception.JsonContentType;
                    context.Result = new ObjectResult(ErrorMessageResponse.Create(notFoundException.Message));
                    return;
                default:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.HttpContext.Response.ContentType = FieldConstants.Exception.JsonContentType;
                    context.Result = new ObjectResult(ErrorMessageResponse.Create(FieldConstants.Exception.GenericErrorMessage));
                    return;
            }
        }
    }
}
