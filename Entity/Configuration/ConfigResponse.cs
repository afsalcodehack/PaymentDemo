using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Configuration
{
    public class ConfigResponse
    {
        [JsonProperty("publishableKey")]
        public string PublishableKey { get; set; }

        [JsonProperty("unitAmount")]
        public long? UnitAmount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
