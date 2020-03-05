using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20MinuteBackend.Cleaner
{
    public interface ICleanBackendService
    {
        Task CleanInactiveBackendInstances();
    }
}
