using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.Response
{
    public class getToken
    {
        [JsonProperty("token")]
        public string token { get; set; }
    }
}
