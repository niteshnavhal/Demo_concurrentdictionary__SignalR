using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.onlyscore
{
    public class Es
    {
        [JsonProperty("eid")]
        public string eid { get; set; }

        [JsonProperty("ety")]
        public string ety { get; set; }

        [JsonProperty("mtyp")]
        public string mtyp { get; set; }

        [JsonProperty("com")]
        public string com { get; set; }

        [JsonProperty("en")]
        public string en { get; set; }

        [JsonProperty("ed")]
        public string ed { get; set; }

        [JsonProperty("et")]
        public string et { get; set; }

        [JsonProperty("te1n")]
        public string te1n { get; set; }

        [JsonProperty("te2n")]
        public string te2n { get; set; }

        [JsonProperty("s1n")]
        public string s1n { get; set; }

        [JsonProperty("s2n")]
        public string s2n { get; set; }
    }
}
