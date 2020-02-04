using System;
using System.IO;
using System.Threading.Tasks;
using _20MinuteBackend.API.Extensions;
using _20MinuteBackend.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.API.Controllers
{
    [Route("[controller]")]
    public class BackendController : ControllerBase
    {
        private readonly IBackendService backendService;

        public BackendController(IBackendService backendService)
        {
            this.backendService = backendService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            var input = await this.HttpContext.BodyAsStringAsync();
            var uri = await this.backendService.TryCreateNewBackendAsync(input);
            if (uri is null)
            {
                return BadRequest(); //service can return result;
            }

            return Created(uri, uri);
        }

        [HttpGet]
        [Route("{guid}")]
        public async Task<IActionResult> Get(string guid)
        {
            return Ok(guid);
        }
    }
}
