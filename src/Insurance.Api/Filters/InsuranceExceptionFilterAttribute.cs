using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Insurance.Api.Exceptions;
using Insurance.Api.Constants;
using Insurance.Api.Models;
using Insurance.Api.Services.Chain;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Filters
{
    public class InsuranceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<InsuranceExceptionFilterAttribute> _logger;
        public InsuranceExceptionFilterAttribute(ILogger<InsuranceExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case NotFoundException notFoundException:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.HttpContext.Response.ContentType = FieldConstants.Exception.JsonContentType;
                    context.Result = new ObjectResult(ErrorMessageResponse.Create(notFoundException.Message));
                    _logger.LogError(notFoundException.Message);
                    return;
                case BadRequestException badRequestException:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.HttpContext.Response.ContentType = FieldConstants.Exception.JsonContentType;
                    context.Result = new ObjectResult(ErrorMessageResponse.Create(badRequestException.Message));
                    _logger.LogError(badRequestException.Message);
                    return;
                default:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.HttpContext.Response.ContentType = FieldConstants.Exception.JsonContentType;
                    context.Result = new ObjectResult(ErrorMessageResponse.Create(FieldConstants.Exception.GenericErrorMessage));
                    _logger.LogError(context.Exception.Message);
                    return;
            }
        }
    }
}
