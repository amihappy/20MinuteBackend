using System;
using System.Threading.Tasks;
using _20MinuteBackend.API.Exceptions;
using _20MinuteBackend.Domain.Backend;
using _20MinuteBackend.Domain.Randomizers;
using _20MinuteBackend.Domain.Time;
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
        private readonly IDateTimeProvider dateTimeProvider;

        public BackendService(IConfiguration configuration, BackendDbContext dbContext, IJsonRandomizer jsonRandomizer, IDateTimeProvider dateTimeProvider)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.jsonRandomizer = jsonRandomizer;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Uri> CreateNewBackendAsync(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new InvalidJsonInputApiException("Body request should contain sample JSON Object.");
            }

            Backend backend;
            try
            {
                backend = new Backend(input, this.dateTimeProvider);
            }
            catch (JsonParseException ex)
            {
                throw new InvalidJsonInputApiException(ex.Message);
            }

            await SaveBackend(backend);
            return BackendFullUri(backend);
        }
       
        public async Task<Uri> CreateNewBackendAsync(JObject input)
        {
            Backend backend = new Backend(input, this.dateTimeProvider);
            await SaveBackend(backend);
            return BackendFullUri(backend);
        }

        private Uri BackendFullUri(Backend backend)
        {
            int lastslash = this.configuration[baseUrlKey].LastIndexOf('/');
            var baseUrl = lastslash == this.configuration[baseUrlKey][0..^1].Length
                ? this.configuration[baseUrlKey].Substring(0, lastslash)
                : this.configuration[baseUrlKey];
            return new Uri($"{baseUrl}/api/backend/{backend.Id}");
        }

        private async Task SaveBackend(Backend backend)
        {
            this.dbContext.Backends.Add(backend);

            await this.dbContext.SaveChangesAsync();
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
