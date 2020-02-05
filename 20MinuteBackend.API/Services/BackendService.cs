using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20MinuteBackend.API.Exceptions;
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

        public async Task<Uri> CreateNewBackendAsync(string input)
        {
            try
            {
                var json = JObject.Parse(input);
                var guid = Guid.NewGuid().ToString();
                var resultString = $"{configuration[baseUrlKey]}backend/{guid}";
                return new Uri(resultString);
            }
            catch (JsonReaderException ex)
            {
                throw new InvalidJsonInputException(ex.Message);
            }
        }
    }
}
