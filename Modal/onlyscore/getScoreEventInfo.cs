using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.onlyscore
{
    public class getScoreEventInfo
    {
        [JsonProperty("success")]
        public bool success { get; set; }

        [JsonProperty("status")]
        public int status { get; set; }

        [JsonProperty("result")]
        public Result result { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }
    }
}
