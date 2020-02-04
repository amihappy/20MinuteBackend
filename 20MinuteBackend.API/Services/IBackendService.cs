using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20MinuteBackend.API.Services
{
    public interface IBackendService
    {
        Task<Uri> TryCreateNewBackendAsync(string input);
    }
}
