using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.Domain.Randomizers
{
    public class JsonRandomizer : IJsonRandomizer
    {
        private readonly IValueRandomizer valueRandomizer;

        public JsonRandomizer(IValueRandomizer valueRandomizer)
        {
            this.valueRandomizer = valueRandomizer;
        }

        public JObject RandomizeJson(JObject json)
        {
            return (JObject)Traverse(json.DeepClone());
        }


        private JToken Traverse(JToken json)
        {
            foreach (var item in json.Children())
            {
                this.Traverse(item);
            }
            if (json is JValue value)
            {
                value.Value = this.valueRandomizer.Randomize(value);
            }
            return json;
        }
    }
}
