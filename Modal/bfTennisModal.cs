using System;
using System.Collections.Generic;
using System.Text;

namespace Modal
{
    public class bfTennisModal
    {
        public int eventTypeId { get; set; }
        public int eventId { get; set; }
        public Score score { get; set; }
        public int currentSet { get; set; }
        public int currentGame { get; set; }
        public FullTimeElapsed fullTimeElapsed { get; set; }
    }
}
