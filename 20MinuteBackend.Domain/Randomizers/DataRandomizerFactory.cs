namespace _20MinuteBackend.Domain.Randomizers
{

    public class DataRandomizerFactory : IDataRandomizerFactory
    {
        public IDataTypeRandomizer Create(string value)
        {
            if (double.TryParse(value, out double number))
            {
                return new NumberRandomizer(number);
            }

            return new StringRandomizer(value);
        }
    }
}
