using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20MinuteBackend.API.Controllers;
using _20MinuteBackend.API.Exceptions;
using _20MinuteBackend.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace _20MinuteBackend.Tests.Controllers
{
    public class BackendControllerTests
    {
        [Fact]
        public async Task Create_When_ProperJsonPassed_Then_GenerateUrl()
        {
            // arrange 
            var backendService = Substitute.For<IBackendService>();
            var unit = new BackendController(backendService);
            var bodyStub = new MemoryStream(Encoding.UTF8.GetBytes("{\"Name\":\"Sir\"}"));
            var requestStub = Substitute.For<HttpRequest>();
            requestStub.Body.Returns(bodyStub);

            var contextMock = Substitute.For<HttpContext>();
            contextMock.Request.Returns(requestStub);

            unit.ControllerContext = new ControllerContext
            {
                HttpContext = contextMock
            };

            var baseurl = "https://localhost/";
            var guid = Guid.NewGuid().ToString();
            var url = $"{baseurl}/backend/{guid}";
            Uri actualUri = new Uri(url);
            backendService.CreateNewBackendAsync(Arg.Any<string>()).Returns(actualUri);

            // act
            var actual = await unit.Create();

            // assert
            actual.Should().BeOfType(typeof(CreatedResult));
            ((CreatedResult)actual).Location = url;
            ((CreatedResult)actual).Value = url;
        }
    }
}
