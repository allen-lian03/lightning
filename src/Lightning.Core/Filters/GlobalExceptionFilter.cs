using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Lightning.Core.Bases;

namespace Lightning.Core.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _host;

        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(
            IHostingEnvironment host,
            ILogger<GlobalExceptionFilter> logger)
        {
            _host = host;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var error = context.Exception;
            _logger.LogError(new EventId(error.HResult, error.GetType().Name),
                error, error.Message);
            if (error is ArgumentException)
            {
                context.HttpContext.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(new {Error = new ApiError(StatusCodes.Status400BadRequest, error)});
            }
            else 
            {
                context.HttpContext.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;
                context.Result = new ObjectResult(new {Error = new ApiError(StatusCodes.Status500InternalServerError, "some errors ocurred on the server.")})
                {
                    StatusCode = (int)StatusCodes.Status500InternalServerError
                };
            }
            context.ExceptionHandled = true;
        }
    }
}