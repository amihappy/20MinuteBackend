using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace _20MinuteBackend.Cleaner
{
    public class Worker : IHostedService
    {
        private readonly ILogger<Worker> logger;
        private readonly IServiceProvider serviceScopeFactory;
        private Timer timer;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            using (var scope = this.serviceScopeFactory.CreateScope())
            {
                var cleanBackendService = scope.ServiceProvider.GetService<ICleanBackendService>();
                try
                {
                    await cleanBackendService.CleanInactiveBackendInstances();
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex.Message, ex);
                }
            }
        }
    }
}
