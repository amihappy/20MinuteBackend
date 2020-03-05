using System;
using _20MinuteBackend.Domain.Time;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _20MinuteBackend.Domain.Backend
{
    public class Backend
    {
        public Guid Id { get; }

        public JObject OrginalJson { get; }

        public DateTime StartTime { get; }

        private Backend()
        {

        }

        private Backend(IDateTimeProvider dateTimeProvider)
        {
            this.Id = Guid.NewGuid();
            this.StartTime = dateTimeProvider.UtcNow;
        }

        public Backend(JObject json, IDateTimeProvider dateTimeProvider) : this(dateTimeProvider)
        {
            this.OrginalJson = json;
        }

        public Backend(string json, IDateTimeProvider dateTimeProvider) : this(dateTimeProvider)
        {
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
