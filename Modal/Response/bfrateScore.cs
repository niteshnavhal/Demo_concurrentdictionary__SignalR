using Modal.ConcurrentDic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.Response
{
    public class bfrateScore
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public ScoreResponse Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
