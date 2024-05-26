using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.CommanrtyScore
{
    public class ShortScore
    {
        public int eti { get; set; }

        public string eid { get; set; }

        public string en { get; set; }

        public string te1n { get; set; }

        public string te2n { get; set; }

        public string t1s { get; set; }

        public string t2s { get; set; }
        public int pt { get; set; }
        public string t1set { get; set; }
        public string t2set { get; set; }
        public string t1p { get; set; }
        public string t2p { get; set; }
    }

    public class WebSocketMessage
    {
        public string eventname { get; set; }
        public string connectionID { get; set; }
        public string data { get; set; }
    }

}
