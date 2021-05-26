using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class AuthorizeCaptureResponse
    {
        [JsonProperty("clientSecret")]
        public string clientSecret { get; set; }
    }
}
