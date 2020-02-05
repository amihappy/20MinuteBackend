using System;
using System.Threading.Tasks;

namespace _20MinuteBackend.API.Services
{
    public interface IBackendService
    {
        Task<Uri> CreateNewBackendAsync(string input);
    }
}
