using System;
using _20MinuteBackend.Domain.Randomizers;
using Xunit;
using FluentAssertions;

namespace _20MinuteBackend.Tests.Services.Randomizers.Types
{
    public class NumberRandomizerTests
    {
        [Fact]
        public void RandomizeValue_When_NoFraction_Then_IntReturned()
        {
            // arrange
            var unit = new NumberRandomizer();

            // act
            var actual = unit.RandomizeValue("123");

            // assert
            int.TryParse(actual, out _).Should().BeTrue();
        }

        [Fact]
        public void RandomizeValue_When_Fraction_Then_NumberWithFractionReturned()
        {
            // arrange
            var unit = new NumberRandomizer();

            // act
            var actual = unit.RandomizeValue("123.4");

            // assert
            int.TryParse(actual, out _).Should().BeFalse();
            double.TryParse(actual, out _).Should().BeTrue();
        }
    }
}
