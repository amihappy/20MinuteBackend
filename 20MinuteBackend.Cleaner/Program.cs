using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _20MinuteBackend.Domain.Time;
using _20MinuteBackend.Infrastructure;
using _20MinuteBackend.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _20MinuteBackend.Cleaner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddTransient<IDateTimeProvider, DateTimeProvider>();
                    services.AddTransient<ICleanBackendService, CleanBackendService>();

                    services.AddDbContext<BackendDbContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("BackendContext")));
                }).UseConsoleLifetime();
    }
}
