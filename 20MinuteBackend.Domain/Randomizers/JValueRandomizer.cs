using System;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.Domain.Randomizers
{

    public class JValueRandomizer : IValueRandomizer
    {
        private readonly IDataRandomizerFactory dataRandomizerFactory;

        public JValueRandomizer(IDataRandomizerFactory dataRandomizerFactory)
        {
            this.dataRandomizerFactory = dataRandomizerFactory;
        }

        public string Randomize(JValue value)
        {
            var randomizer = this.dataRandomizerFactory.Create(value.ToString());
            return randomizer.RandomizeValue(value.ToString());
        }
    }
}
