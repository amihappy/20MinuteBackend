using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace _20MinuteBackend.Cleaner
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IServiceProvider serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            using (var scope = this.serviceScopeFactory.CreateScope())
            {
                var cleanBackendService = scope.ServiceProvider.GetService<ICleanBackendService>();
                await cleanBackendService.CleanInactiveBackendInstances();
            }
        }
    }
}
