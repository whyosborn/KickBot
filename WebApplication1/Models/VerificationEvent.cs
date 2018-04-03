using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class VerificationEvent
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "challenge")]
        public string Challenge { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

    }
}