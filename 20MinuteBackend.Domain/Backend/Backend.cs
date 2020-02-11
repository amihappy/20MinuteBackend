﻿using System;
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

        public Uri GetUrl(Uri baseUrl)
        {
            if (baseUrl == null)
                throw new ArgumentNullException(nameof(baseUrl));

            return new Uri(baseUrl, $"/backend/{this.Id}");
        }
    }
}
