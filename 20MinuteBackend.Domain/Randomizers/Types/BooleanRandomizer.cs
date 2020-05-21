using System;

namespace _20MinuteBackend.Domain.Randomizers.Types
{
    public class BooleanRandomizer : IDataTypeRandomizer
    {
        public string RandomizeValue(string value) => new Random().NextDouble() > 0.5 ? "true" : "false";
    }
}
