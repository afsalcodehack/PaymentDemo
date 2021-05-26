using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Configuration
{
    public class CreateCheckoutSessionResponse
    {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
    }
}
