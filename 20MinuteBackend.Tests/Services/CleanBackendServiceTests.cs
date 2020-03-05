using System;
using System.Linq;
using System.Threading.Tasks;
using _20MinuteBackend.Cleaner;
using _20MinuteBackend.Domain.Backend;
using _20MinuteBackend.Domain.Time;
using _20MinuteBackend.Infrastructure;
using _20MinuteBackend.Infrastructure.Time;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace _20MinuteBackend.Tests.Services
{
    public class CleanBackendServiceTests
    {
        [Fact]
        public async Task Given_InactiveBackendsExist_When_CleanInactiveBackendInstances_Then_BackendsRemoved()
        {
            // arrange
            using (var testClass = CleanBackendServiceTest.Create())
            {
                var oldTime = Substitute.For<IDateTimeProvider>();
                oldTime.UtcNow.Returns(DateTime.UtcNow.AddMinutes(-30));
                var backend = new Backend("{}", oldTime);
                testClass.DbContext.Backends.Add(backend);
                testClass.DbContext.SaveChanges();
                testClass.DateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
                testClass.Configuration["InactivityMinutes"] = "20";

                // act
                await testClass.CleanBackendService.CleanInactiveBackendInstances();
            }

            using (var testClass = CleanBackendServiceTest.Create())
            {
                // assert
                testClass.DbContext.Backends.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Given_ActiveBackendsExist_When_CleanInactiveBackendInstances_Then_BackendsWontBeRemoved()
        {
            // arrange
            using (var testClass = CleanBackendServiceTest.Create())
            {
                testClass.DateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
                var backend = new Backend("{}", testClass.DateTimeProvider);
                testClass.DbContext.Backends.Add(backend);
                testClass.DbContext.SaveChanges();
                testClass.Configuration["InactivityMinutes"] = "20";

                // act
                await testClass.CleanBackendService.CleanInactiveBackendInstances();
            }

            using (var testClass = CleanBackendServiceTest.Create())
            {
                // assert
                testClass.DbContext.Backends.Count().Should().Be(1);
            }
        }

        private sealed class CleanBackendServiceTest : IDisposable
        {
            public BackendDbContext DbContext { get; }
            public IConfiguration Configuration { get; }
            public IDateTimeProvider DateTimeProvider { get; }

            public CleanBackendService CleanBackendService { get; }

            private CleanBackendServiceTest(BackendDbContext dbContext, IConfiguration configuration, IDateTimeProvider dateTimeProvider)
            {
                DbContext = dbContext;
                Configuration = configuration;
                DateTimeProvider = dateTimeProvider;
                this.CleanBackendService = new CleanBackendService(configuration, dbContext, dateTimeProvider);
            }

            public static CleanBackendServiceTest Create()
            {
                var options = new DbContextOptionsBuilder<BackendDbContext>().UseInMemoryDatabase("backends").Options;
                var config = Substitute.For<IConfiguration>();
                var context = new BackendDbContext(options);
                var dateTimeProvider = Substitute.For<IDateTimeProvider>();
                return new CleanBackendServiceTest(context, config,dateTimeProvider);
            }

            public void Dispose()
            {
                DbContext?.Dispose();
            }
        }
    }
}