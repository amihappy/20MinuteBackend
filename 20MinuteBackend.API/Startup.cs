using _20MinuteBackend.API.Middlewares;
using _20MinuteBackend.API.Services;
using _20MinuteBackend.Domain.Randomizers;
using _20MinuteBackend.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _20MinuteBackend.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBackendService, BackendService>();
            services.AddTransient<IJsonRandomizer, JsonRandomizer>();
            services.AddTransient<IValueRandomizer, JValueRandomizer>();
            services.AddTransient<IDataRandomizerFactory, DataRandomizerFactory>();

            services.AddDbContext<BackendDbContext>(options => options.UseSqlServer(this.configuration.GetConnectionString("BackendContext")));

            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
