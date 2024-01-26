using System.Net;
using System.Net.Mime;
using Insurance.Api.Application.Exceptions;
using Insurance.Api.Application.Models;
using Insurance.Api.Presentation.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Presentation.Filters
{
    public class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilterAttribute> _logger;
        public ExceptionFilterAttribute(ILogger<ExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case NotFoundException notFoundException:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    context.Result = new ObjectResult(ErrorMessageResponse.Create(notFoundException.Message));
                    _logger.LogError(notFoundException.Message);
                    return;
                case BadRequestException badRequestException:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    context.Result = new ObjectResult(ErrorMessageResponse.Create(badRequestException.Message));
                    _logger.LogError(badRequestException.Message);
                    return;
                default:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    context.Result = new ObjectResult(ErrorMessageResponse.Create("An error has occurred!"));
                    _logger.LogError(context.Exception.Message);
                    return;
            }
        }
    }
}
