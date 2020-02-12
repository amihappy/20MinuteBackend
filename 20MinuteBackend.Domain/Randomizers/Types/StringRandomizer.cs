using System;
using System.Linq;

namespace _20MinuteBackend.Domain.Randomizers
{
    public class StringRandomizer : IDataTypeRandomizer
    {
        private readonly string value;
        private static Random random = new Random();

        public StringRandomizer(string value)
        {
            this.value = value;
        }

        public string RandomizeValue()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, this.value.Length).Select(s => this.RandomizeCapitalLetter(s[random.Next(s.Length)])).ToArray());
        }

        char RandomizeCapitalLetter(char c) => random.NextDouble() > 0.5 ? char.ToLower(c) : c;
    }
}