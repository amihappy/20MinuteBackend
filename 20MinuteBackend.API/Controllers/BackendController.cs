using System.Threading.Tasks;
using _20MinuteBackend.API.Extensions;
using _20MinuteBackend.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace _20MinuteBackend.API.Controllers
{
    [Route("[controller]")]
    public class BackendController : ControllerBase
    {
        private readonly IBackendService service;

        public BackendController(IBackendService backendService)
        {
            this.service = backendService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create()
        {
            var input = await this.HttpContext.BodyAsStringAsync();
            var uri = await this.service.CreateNewBackendAsync(input);
            return Created(uri, uri);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(string guid)
        {
            var json = await this.service.GenerateRandomJsonForBackend(guid);
            return Ok(json);
        }
    }
}
