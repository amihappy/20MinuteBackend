using System;
using _20MinuteBackend.API.Middlewares;
using _20MinuteBackend.API.Services;
using _20MinuteBackend.Domain.Randomizers;
using _20MinuteBackend.Domain.Time;
using _20MinuteBackend.Infrastructure;
using _20MinuteBackend.Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;

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

            services.AddHealthChecks()
                .AddCheck("liveness", () => HealthCheckResult.Unhealthy())
                .AddDbContextCheck<BackendDbContext>("readiness", HealthStatus.Degraded, tags: new[] { "readiness" });

            services.AddDbContext<BackendDbContext>(options => options.UseSqlServer(this.configuration.GetConnectionString("BackendContext"),
                options => options.EnableRetryOnFailure()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "20 Minute Backend API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://localhost:4200").AllowAnyHeader();
                    builder.WithOrigins("http://localhost:4200").AllowAnyHeader();
                    builder.AllowAnyOrigin().AllowAnyHeader();
                });
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

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("api/health/live", new HealthCheckOptions()
                {
                    Predicate = _ => false
                });
                endpoints.MapHealthChecks("api/health/ready", new HealthCheckOptions()
                {
                    Predicate = check => check.Tags.Contains("readiness"),
                });
            });

            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
            var sqlSetupPolicy = Policy.Handle<SqlException>().WaitAndRetry(new[] {
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(30),
                TimeSpan.FromSeconds(60)
            },
            (e, t) => logger.LogWarning($"{e.Message}, retry in {t.TotalSeconds} seconds"));
            sqlSetupPolicy.Execute(() => scope.ServiceProvider.GetService<BackendDbContext>().Database.Migrate());
        }
    }
}
