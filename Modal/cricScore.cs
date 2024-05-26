using System;
using System.Collections.Generic;
using System.Text;

namespace Modal
{
    public class cricScore
    {
        public List<Score> _Score { get; set; }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Innings1
        {
            public string runs { get; set; }
            public string wicket { get; set; }
            public string Over { get; set; }
        }

        public class Innings2
        {
            public string runs { get; set; }
            public string wicket { get; set; }
            public string Over { get; set; }
        }

        public class Score
        {
            public Innings1 innings1 { get; set; }
            public Innings2 innings2 { get; set; }
        }


    }
}
