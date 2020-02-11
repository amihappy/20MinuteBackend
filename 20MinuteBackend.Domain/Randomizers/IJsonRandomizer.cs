using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.Domain.Randomizers
{
    public interface IJsonRandomizer
    {
        JObject RandomizeJson(JObject json);
    }
}
