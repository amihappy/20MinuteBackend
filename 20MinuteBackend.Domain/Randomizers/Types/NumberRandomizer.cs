using System;

namespace _20MinuteBackend.Domain.Randomizers
{

    public class NumberRandomizer : IDataTypeRandomizer
    {
        private readonly double number;
        private static Random random = new Random();

        public NumberRandomizer(double number)
        {
            this.number = number;
        }

        public string RandomizeValue()
        {
            var fraction = this.number % Math.Round(this.number);
            if (fraction > 0)
            {
                fraction = random.NextDouble();
            }

            return (random.Next(0, Int32.MaxValue) + fraction).ToString();
        }
    }
}
