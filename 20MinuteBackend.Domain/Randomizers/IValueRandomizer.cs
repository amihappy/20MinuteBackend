using System;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.Domain.Randomizers
{
    public interface IValueRandomizer
    {
        string Randomize(JValue value);
    }
}
