using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Lightning.Core.Bases;
using Lightning.Application.Commands;

namespace Lightning.WebApi.Controllers
{
    [Route("api/v1/api-keys")]
    [Produces("application/json")]
    public class ApiKeyController : NControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public ApiKeyController(ILogger<ApiKeyController> logger) : base(logger)
        {
        }

        /// <summary>
        /// get one new api key
        /// </summary>
        /// <param name="type">type is 1 will throw 400 error; type is 2 will throw 500 error.</param>
        /// <returns></returns>
        [HttpGet, Route("")]
        public async Task<ActionResult<string>> GenerateApiKey([FromQuery] int type = 0)
        {
            _logger.LogInformation("Get Generate Api Key Query");
            switch (type)
            {
                case 1:
                    throw new ArgumentNullException(nameof(type));
                case 2:
                    throw new NullReferenceException(nameof(type));
                default:
                    return Ok(await Task.FromResult("xayloyqerwq"));
            }
        }

        /// <summary>
        /// create api key for one client.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>
        ///  POST /api/v1/api-keys
        ///  {
        ///     "client": "1234567890"
        ///  }
        /// </remarks>
        [HttpPost, Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> GenerateApiKey([FromBody]CreateApiKeyCommand command)
        {
            _logger.LogWarning("Post Generate Api Key");
            return Created(Guid.NewGuid().ToString(), "save successfully");
        }
    }
}