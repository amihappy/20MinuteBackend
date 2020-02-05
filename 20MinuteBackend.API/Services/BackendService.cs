using System;
using System.Threading.Tasks;
using _20MinuteBackend.API.Exceptions;
using _20MinuteBackend.Domain.Backend;
using Microsoft.Extensions.Configuration;

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
            Backend backend;
            try
            {
                backend = new Backend(input);
            }
            catch (JsonParseException ex)
            {
                throw new InvalidJsonInputException(ex.Message);
            }

            var resultString = $"{configuration[baseUrlKey]}backend/{backend.Id}";
            return new Uri(resultString);
        }
    }
}
