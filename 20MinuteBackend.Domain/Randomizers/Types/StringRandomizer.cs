using System;
using System.Linq;

namespace _20MinuteBackend.Domain.Randomizers
{
    public class StringRandomizer : IDataTypeRandomizer
    {
        private string value;
        private Random random;

        public StringRandomizer(string value)
        {
            this.value = value;
            this.random = new Random();
        }

        public string RandomizeValue()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, this.value.Length).Select(s => this.RandomizeCapitalLetter(s[this.random.Next(s.Length)])).ToArray());
        }

        Char RandomizeCapitalLetter(char c) => this.random.Next(0, 1) > 0 ? Char.ToLower(c) : c;
    }
}