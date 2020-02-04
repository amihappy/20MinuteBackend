﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20MinuteBackend.API.Controllers;
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
        public BackendControllerTests()
        {

        }

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

            backendService.Setup(s => s.TryCreateNewBackendAsync(It.IsAny<string>())).ReturnsAsync(() => new Uri(url));

            // act
            var actual = await unit.Create();

            // assert
            actual.Should().BeOfType(typeof(CreatedResult));
            ((CreatedResult)actual).Location = url;
            ((CreatedResult)actual).Value = url;
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("{\"name\":asd}")]
        [InlineData("")]
        public async Task Create_When_InvalidJsonPassed_Then_ThrowException(string input)
        {
            // arrange 
            var backendService = new Mock<IBackendService>();
            var unit = new BackendController(backendService.Object);

            var bodyMock = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(s => s.Body).Returns(bodyMock);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(s => s.Request).Returns(requestMock.Object);
            backendService.Setup(s => s.TryCreateNewBackendAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            unit.ControllerContext = new ControllerContext
            {
                HttpContext = contextMock.Object
            };

            // act
            var actual = await unit.Create();

            // assert
            actual.Should().BeOfType(typeof(BadRequestResult));
        }
    }
}
