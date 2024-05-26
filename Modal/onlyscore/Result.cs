using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.onlyscore
{
    public class Result
    {
        [JsonProperty("es")]
        public Es es { get; set; }
    }
}
