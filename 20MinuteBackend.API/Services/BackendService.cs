using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.API.Services
{

    public class BackendService : IBackendService
    {
        private readonly IConfiguration configuration;
        private const string baseUrlKey = "BaseUrl";

        public BackendService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<Uri> TryCreateNewBackendAsync(string input)
        {
            try
            {
                var json = JObject.Parse(input);
                var guid = Guid.NewGuid().ToString();
                var resultString = $"{configuration[baseUrlKey]}backend/{guid}";
                return null;
            }
            catch (JsonReaderException ex)
            {
                //throws apiexception
                return null;
            }
        }
    }
}
