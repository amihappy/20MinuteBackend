using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using _20MinuteBackend.API.Exceptions;
using _20MinuteBackend.API.Services;
using _20MinuteBackend.Domain.Randomizers;
using _20MinuteBackend.Infrastructure;
using _20MinuteBackend.Infrastructure.Time;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace _20MinuteBackend.Tests.Services
{
    public class BackendServiceTests
    {
        [Theory]
        [InlineData("http://localhost:5000/", "http://localhost:5000/api/backend/")]
        [InlineData("http://localhost:5000", "http://localhost:5000/api/backend/")]
        [InlineData("http://localhost:5000/back", "http://localhost:5000/back/api/backend/")]
        public async Task GetUrl_When_BaseUrlPassed_Then_ProperUriReturned(string baseUrl, string expected)
        {
            // arrange
            using (var testClass = BackendServiceTest.Create())
            {
                testClass.Configuration["BaseUrl"].Returns(baseUrl);
                testClass.JsonRandomizer.RandomizeJson(Arg.Any<JObject>()).Returns(new JObject());

                // act
                var actual = await testClass.BackendService.CreateNewBackendAsync("{}");

                // assert
                actual.ToString().Should().StartWith(expected);
            }
        }

        [Fact]
        public async Task Given_ProperJson_When_CreateBackendAsync_Then_AddBackendToDb()
        {
            // arrange
            using (var testclass = BackendServiceTest.Create())
            {
                testclass.Configuration["BaseUrl"].Returns("https://localhost:5000");
                // act
                var actual = await testclass.BackendService.CreateNewBackendAsync("{}");
            }

            // assert
            using (var testclass = BackendServiceTest.Create())
            {
                testclass.BackendDbContext.Backends.Count().Should().Be(1);
            }
        }

        [Fact]
        public async Task Given_EmptyBody_When_CreateNewBackendAsync_Then_ExceptionThrows()
        {
            // arrange
            using (var testclass = BackendServiceTest.Create())
            {
                testclass.Configuration["BaseUrl"].Returns("https://localhost:5000");
                // act
                Func<Task> act = async () => await testclass.BackendService.CreateNewBackendAsync(string.Empty);

                // assert
                act.Should().Throw<InvalidJsonInputApiException>();
            }


        }

        private sealed class BackendServiceTest : IDisposable
        {
            public BackendService BackendService { get; }

            public IJsonRandomizer JsonRandomizer { get; }

            public BackendDbContext BackendDbContext { get; }

            public IConfiguration Configuration { get; }

            private BackendServiceTest(IConfiguration configuration, BackendDbContext dbContext, IJsonRandomizer randomizer)
            {
                this.JsonRandomizer = randomizer;
                this.BackendDbContext = dbContext;
                this.Configuration = configuration;
                this.BackendService = new BackendService(configuration, dbContext, randomizer, new DateTimeProvider());
            }



            public static BackendServiceTest Create([CallerMemberName]string testName = "")
            {
                var options = new DbContextOptionsBuilder<BackendDbContext>().UseInMemoryDatabase(testName).Options;
                var config = Substitute.For<IConfiguration>();
                var context = new BackendDbContext(options);
                var randomizer = Substitute.For<IJsonRandomizer>();
                return new BackendServiceTest(config, context, randomizer);
            }

            public void Dispose()
            {
                BackendDbContext?.Dispose();
            }
        }
    }
}
