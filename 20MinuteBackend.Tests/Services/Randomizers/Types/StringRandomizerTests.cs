using _20MinuteBackend.Domain.Randomizers;
using Xunit;
using AutoFixture.Xunit2;
using FluentAssertions;

namespace _20MinuteBackend.Tests.Services.Randomizers.Types
{
    public class StringRandomizerTests
    {
        [Theory]
        [AutoData]
        public void RandomizeValue_When_StringPassed_Then_NewStringReturnedWithSameLength(string randomString)
        {
            // arrange
            var unit = new StringRandomizer(randomString);

            // act
            var actual = unit.RandomizeValue();

            // assert
            actual.Should().NotBe(randomString);
            actual.Length.Should().Be(randomString.Length);
        }
    }
}
