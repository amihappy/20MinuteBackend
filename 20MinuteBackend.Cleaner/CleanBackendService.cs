using System;
using System.Linq;
using System.Threading.Tasks;
using _20MinuteBackend.Domain.Time;
using _20MinuteBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _20MinuteBackend.Cleaner
{
    public class CleanBackendService : ICleanBackendService
    {
        private readonly IConfiguration configuration;
        private readonly BackendDbContext backendDbContext;
        private readonly IDateTimeProvider dateTimeProvider;

        private const string inactivityMinutesKey = "InactivityMinutes";

        public CleanBackendService(IConfiguration configuration, BackendDbContext backendDbContext, IDateTimeProvider dateTimeProvider)
        {
            this.configuration = configuration;
            this.backendDbContext = backendDbContext;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task CleanInactiveBackendInstances()
        {
            if (int.TryParse(this.configuration[inactivityMinutesKey], out int minutes))
            {
                var unusedInstances = await this.backendDbContext.Backends
                    .Where(s => s.StartTime.AddMinutes(minutes) < this.dateTimeProvider.UtcNow)
                    .ToListAsync();

                if (unusedInstances.Count > 0)
                {
                    backendDbContext.Backends.RemoveRange(unusedInstances);

                    await backendDbContext.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception($"Missing value for {inactivityMinutesKey} or it can't be parsed as an integer");
            }
        }
    }
}