using _20MinuteBackend.API.Middlewares;
using _20MinuteBackend.API.Services;
using _20MinuteBackend.Domain.Randomizers;
using _20MinuteBackend.Domain.Time;
using _20MinuteBackend.Infrastructure;
using _20MinuteBackend.Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            services.AddDbContext<BackendDbContext>(options => options.UseSqlServer(this.configuration.GetConnectionString("BackendContext")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title =  "20 Minute Backend API", Version = "v1"});
            });

            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(o =>
            {
                o.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("swagger/v1/swagger.json", "20 Minute Backend API v1");
                o.RoutePrefix = "api";
            });

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<BackendDbContext>().Database.Migrate();
        }
    }
}
