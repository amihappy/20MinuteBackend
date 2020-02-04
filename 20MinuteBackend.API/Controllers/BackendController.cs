using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.API.Controllers
{
    [Route("[controller]")]
    public class BackendController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            string jsonPayload = string.Empty;
            using (var reader = new StreamReader(this.HttpContext.Request.Body))
            {
                jsonPayload = await reader.ReadToEndAsync();
            }
            try
            {
                var json = JObject.Parse(jsonPayload);
                return Ok(json.ToString());
            }
            catch (JsonReaderException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{guid}")]
        public async Task<IActionResult> Get(string guid)
        {
            return Ok(guid);
        }
    }
}
