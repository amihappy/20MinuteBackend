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
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace _20MinuteBackend.Tests.Controllers
{
    public class BackendControllerTests
    {
        [Fact]
        public async Task Create_When_ProperJsonPassed_Then_GenerateUrl()
        {
            // arrange 
            var backendService = new Mock<IBackendService>();
            var unit = new BackendController(backendService.Object);
            var bodyMock = new MemoryStream(Encoding.UTF8.GetBytes("{\"Name\":\"Sir\"}"));
            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(s => s.Body).Returns(bodyMock);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(s => s.Request).Returns(requestMock.Object);

            unit.ControllerContext = new ControllerContext
            {
                HttpContext = contextMock.Object
            };

            var baseurl = "https://localhost/";
            var guid = Guid.NewGuid().ToString();
            var url = $"{baseurl}/backend/{guid}";
            Uri actualUri = new Uri(url);
            backendService.Setup(s => s.CreateNewBackendAsync(It.IsAny<string>())).ReturnsAsync(() => actualUri);

            // act
            var actual = await unit.Create();

            // assert
            actual.Should().BeOfType(typeof(CreatedResult));
            ((CreatedResult)actual).Location = url;
            ((CreatedResult)actual).Value = url;
        }
    }
}
