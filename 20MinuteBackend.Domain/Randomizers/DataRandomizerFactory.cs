using _20MinuteBackend.Domain.Randomizers.Types;

namespace _20MinuteBackend.Domain.Randomizers
{

    public class DataRandomizerFactory : IDataRandomizerFactory
    {
        public IDataTypeRandomizer Create(string value)
        {
            if (double.TryParse(value, out _))
            {
                return new NumberRandomizer();
            }

            if (bool.TryParse(value, out _))
            {
                return new BooleanRandomizer();
            }

            return new StringRandomizer();
        }
    }
}
