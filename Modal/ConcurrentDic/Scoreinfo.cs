using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.ConcurrentDic
{
   public class Scoreinfo
    {
        public int EventID { get; set; }
        public List<ScoreProviderLinkSnap> links { get; set; }

        //public int providerType { get; set; }

        //public Dictionary<int, string> links { get; set; }
    }
}
