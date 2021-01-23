using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Lightning.Core.Bases;

namespace Lightning.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ModelValidationAttribute> _logger;

        public ModelValidationAttribute() {}

        public ModelValidationAttribute(ILogger<ModelValidationAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var logMessage = new System.Text.StringBuilder("{", 128);
                logMessage.AppendFormat("\"api\":\"{0}\"", context.HttpContext.Request.Path);

                var apiError = new ApiError(400);
                foreach (var pair in context.ModelState)
                {
                    var key = pair.Key;
                    logMessage.AppendFormat(",\"invalidField\":\"{0}\"", key);

                    foreach (var error in pair.Value.Errors)
                    {
                        apiError.AddErrorDetail(error.ErrorMessage, key);
                    }
                }
                logMessage.Append("}");

                context.Result = new BadRequestObjectResult(new { Error = apiError });

                if (_logger != null)
                {
                    _logger.LogWarning(logMessage.ToString());
                }
            }
        }
    }
}