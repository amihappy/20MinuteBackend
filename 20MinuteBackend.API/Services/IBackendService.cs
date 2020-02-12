using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.API.Services
{
    public interface IBackendService
    {
        Task<Uri> CreateNewBackendAsync(string input);

        Task<Uri> CreateNewBackendAsync(JObject input);

        Task<JObject> GenerateRandomJsonForBackend(string id);
    }
}
