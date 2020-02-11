using System;
using System.Threading.Tasks;
using _20MinuteBackend.API.Exceptions;
using _20MinuteBackend.Domain.Backend;
using _20MinuteBackend.Domain.Randomizers;
using _20MinuteBackend.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.API.Services
{

    public class BackendService : IBackendService
    {
        private readonly IConfiguration configuration;
        private const string baseUrlKey = "BaseUrl";
        private readonly BackendDbContext dbContext;
        private readonly IJsonRandomizer jsonRandomizer;

        public BackendService(IConfiguration configuration, BackendDbContext dbContext, IJsonRandomizer jsonRandomizer)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.jsonRandomizer = jsonRandomizer;
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
                throw new InvalidJsonInputApiException(ex.Message);
            }

            this.dbContext.Backends.Add(backend);

            await this.dbContext.SaveChangesAsync();
            return backend.GetUrl(new Uri(configuration[baseUrlKey]));
        }

        public async Task<JObject> GenerateRandomJsonForBackend(string id)
        {
            var backend = await this.dbContext.Backends.FindAsync(new Guid(id));

            if (backend is null)
            {
                throw new ResourceNotFoundApiException(id);
            }

            return this.jsonRandomizer.RandomizeJson(backend.OrginalJson);
        }
    }
}
