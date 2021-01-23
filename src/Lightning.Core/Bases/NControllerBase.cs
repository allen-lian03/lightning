
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

using Lightning.Core.Attributes;

namespace Lightning.Core.Bases
{
    [ModelValidation]
    public class NControllerBase : ControllerBase
    {
        protected readonly ILogger _logger;

        protected readonly string _version;

        protected NControllerBase(ILogger logger)
        {
            _logger = logger;
            _version = "v1";
        }

        #region 400

        public override BadRequestObjectResult BadRequest(object error)
        {
            return base.BadRequest(new { Error = new ApiError(400, error) });
        }

        public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
        {
            var apiError = new ApiError(400);
            foreach (var pair in modelState)
            {
                foreach (var error in pair.Value.Errors)
                {
                    apiError.AddErrorDetail(error.ErrorMessage, pair.Key);
                }
            }
            return base.BadRequest(new { Error = apiError });
        }

        #endregion

        public override NotFoundObjectResult NotFound(object value)
        {
            return base.NotFound(new { Error = new ApiError(404, value) });
        }

        #region 200

        public override OkObjectResult Ok(object value)
        {
            return base.Ok(new ApiResult(_version, value));
        }

        #endregion

        #region 201

        public override CreatedResult Created(Uri uri, object value)
        {
            return base.Created(uri, new ApiResult(_version, value));
        }

        public override CreatedResult Created(string uri, object value)
        {
            return base.Created(uri, new ApiResult(_version, value));
        }

        public override CreatedAtActionResult CreatedAtAction(string actionName, object routeValues, object value)
        {
            return base.CreatedAtAction(actionName, routeValues,
                new ApiResult(_version, value));
        }

        public override CreatedAtActionResult CreatedAtAction(string actionName, string controllerName, object routeValues, object value)
        {
            return base.CreatedAtAction(actionName, controllerName,
                new ApiResult(_version, value));
        }

        public override CreatedAtActionResult CreatedAtAction(string actionName, object value)
        {
            return base.CreatedAtAction(actionName,
                new ApiResult(_version, value));
        }

        public override CreatedAtRouteResult CreatedAtRoute(object routeValues, object value)
        {
            return base.CreatedAtRoute(routeValues,
                new ApiResult(_version, value));
        }

        public override CreatedAtRouteResult CreatedAtRoute(string routeName, object routeValues, object value)
        {
            return base.CreatedAtRoute(routeName, routeValues,
                new ApiResult(_version, value));
        }

        public override CreatedAtRouteResult CreatedAtRoute(string routeName, object value)
        {
            return base.CreatedAtRoute(routeName,
                new ApiResult(_version, value));
        }

        #endregion

        public override ObjectResult StatusCode(int statusCode, object value)
        {
            if (statusCode >= 400)
            {
                return base.StatusCode(statusCode, new { Error = new ApiError(statusCode, value) });
            }
            else
            {
                return base.StatusCode(statusCode, new ApiResult(_version, value));
            }
        }
    }

    /// <summary>
    /// An uniform result object
    /// </summary>
    internal class ApiResult
    {
        public ApiResult(string version, object item)
        {
            ApiVersion = version;
            if (item is ICollection items)
            {
                Data = new { Items = items };
            }
            else
            {
                Data = item;
            }
        }

        public string ApiVersion { get; private set; }

        public object Data { get; private set; }
    }

    /// <summary>
    /// An uniform error object.
    /// reference https://google.github.io/styleguide/jsoncstyleguide.xml#Reserved_Property_Names_in_the_error_object
    /// </summary>
    internal class ApiError
    {
        public ApiError(int statusCode)
        {
            Code = statusCode;
            Errors = new List<object>(5);
        }

        public ApiError(int statusCode, object item)
        {
            Code = statusCode;
            if (item is ICollection items)
            {
                Errors = new List<object>(items.Count);
                foreach (var error in items)
                {
                    AddErrorDetail(error);
                }
            }
            else
            {
                Errors = new List<object>(1);
                AddErrorDetail(item);
            }
        }

        public int Code { get; private set; }

        public object Message { get; private set; }

        public ICollection<object> Errors { get; private set; }

        public void AddErrorDetail(object message, string domain = "", string reason = null,
                string extendedHelp = null, string sendReport = null)
        {
            string errorText = null;            
            if (message == null)
            {
                errorText = string.Empty;
                domain = string.Empty;
            }

            if (message is Exception error)
            {
                errorText = error.Message;
                domain = error.Source;
            }
            else
            {
                errorText = message.ToString();
            }

            if (Errors.Count == 0)
            {
                // the first error message
                Message = errorText;
            }
            Errors.Add(new InnerError(errorText, domain, reason, extendedHelp, sendReport));
        }

        internal class InnerError
        {
            public InnerError(string message, string domain = "", string reason = "",
                string extendedHelp = "", string sendReport = "")
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new ArgumentNullException(message);
                }

                Message = message;
                Domain = domain;
                Reason = reason;
                ExtendedHelp = extendedHelp;
                SendReport = sendReport;
            }

            /// <summary>
            /// Unique identifier for the service raising this error.
            /// This helps distinguish service-specific errors (i.e. error inserting an event in a calendar) from general protocol errors (i.e. file not found).
            /// </summary>
            /// <value></value>
            public string Domain { get; set; }

            /// <summary>
            /// Unique identifier for this error.
            /// Different from the error.code property in that this is not an http response code.
            /// </summary>
            /// <value></value>
            public string Reason { get; set; }

            /// <summary>
            /// A human readable message providing more details about the error.
            /// If there is only one error, this field will match error.message.
            /// </summary>
            /// <value></value>
            public string Message { get; set; }

            /// <summary>
            /// A URI for a help text that might shed some more light on the error.
            /// </summary>
            /// <value></value>
            public string ExtendedHelp { get; set; }

            /// <summary>
            /// A URI for a report form used by the service to collect data about the error condition.
            /// This URI should be preloaded with parameters describing the request.
            /// </summary>
            /// <value></value>
            public string SendReport { get; set; }
        }
    }
}
