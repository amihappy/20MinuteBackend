using System;

namespace _20MinuteBackend.Domain.Randomizers
{

    public class NumberRandomizer : IDataTypeRandomizer
    {
        private static Random random = new Random();

        public string RandomizeValue(string value)
        {
            double number = double.Parse(value);
            var fraction = number % Math.Round(number);
            if (fraction > 0)
            {
                fraction = random.NextDouble();
            }

            return (random.Next(0, Int32.MaxValue) + fraction).ToString();
        }
    }
}
