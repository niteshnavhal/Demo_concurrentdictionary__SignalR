using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.Request
{
    public class manualScoreRequest
    {
        public string eventIds { get; set; }
        public int link { get; set; } = 0;
        public string color { get; set; } = "";
        public string font { get; set; } = "";
        public int provider { get; set; } = 0;

    }
}
