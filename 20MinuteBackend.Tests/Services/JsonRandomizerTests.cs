using System.Collections.Generic;
using _20MinuteBackend.Domain.Randomizers;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace _20MinuteBackend.Tests.Services
{
    public class JsonRandomizerTests
    {
        [Fact]
        public void When_SimpleJsonPassed_ValueChanged()
        {
            // arrange
            var randomizer = Substitute.For<IValueRandomizer>();
            randomizer.Randomize(Arg.Any<JValue>()).Returns("mocked");
            var unit = new JsonRandomizer(randomizer);
            var startValue = "test";
            var json = (JObject)JToken.FromObject(new { Name = startValue, Surname = startValue + "s"});

            // act
            var actual = unit.RandomizeJson(json);

            // assert
            actual["Name"].ToString().Should().NotBeEmpty();
            actual["Name"].ToString().Should().Be("mocked");
        }

        [Fact]
        public void When_JsonWithArrayPassed_ValueChanged()
        {
            // arrange
            var randomizer = Substitute.For<IValueRandomizer>();
            randomizer.Randomize(Arg.Any<JValue>()).Returns("mocked");
            var unit = new JsonRandomizer(randomizer);
            var startValue = "test";
            var json = (JObject)JToken.FromObject(
                new { Name = startValue,
                    Family = new List<dynamic> {
                        new {Name = "in1", Surname = "in11" },
                        new { Name = "in2", Surname = "in22" } 
                    },
                    Surname = startValue + "s" });

            // act
            var actual = unit.RandomizeJson(json);

            // assert
            actual["Name"].ToString().Should().NotBeEmpty();
            actual["Name"].ToString().Should().NotBe(startValue);
        }
    }
}
