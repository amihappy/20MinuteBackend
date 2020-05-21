using System;
using System.Linq;

namespace _20MinuteBackend.Domain.Randomizers
{
    public class StringRandomizer : IDataTypeRandomizer
    {
        private static Random random = new Random();

        public string RandomizeValue(string value)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, value.Length).Select(s => this.RandomizeCapitalLetter(s[random.Next(s.Length)])).ToArray());
        }

        char RandomizeCapitalLetter(char c) => random.NextDouble() > 0.5 ? char.ToLower(c) : c;
    }
}