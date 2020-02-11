using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _20MinuteBackend.Domain.Backend;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace _20MinuteBackend.Tests.Domain
{
    public class BackendTests
    {
        [Theory]
        [InlineData("http://localhost:5000/")]
        [InlineData("http://localhost:5000")]
        public void GetUrl_When_BaseUrlPassed_Then_ProperUriReturned(string baseUrl)
        {
            // arrange
            dynamic t = new { id = 1 };
            var unit = new Backend(JsonConvert.SerializeObject(t));

            // act
            var actual = unit.GetUrl(new Uri(baseUrl));

            // assert
            actual.Should().Be(new Uri($"http://localhost:5000/backend/{unit.Id}"));
        }
    }
}
