using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.Domain.Backend
{
    public class Backend
    {
        public Guid Id { get; }

        public JObject OrginalJson { get; }

        public DateTime StartTime { get;}

        public Backend(string json)
        {
            this.Id = Guid.NewGuid();
            this.StartTime = DateTime.UtcNow;
            try
            {
                this.OrginalJson = JObject.Parse(json);
            }
            catch (JsonReaderException ex)
            {
                throw new JsonParseException(ex);
            }
        }
    }
}
