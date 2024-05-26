using System;
using System.Collections.Generic;
using System.Text;

namespace Modal
{
    public class bfSoccerModal
    {

        public int eventTypeId { get; set; }
        public int eventId { get; set; }
        public Score score { get; set; }
        public int timeElapsed { get; set; }
        public int elapsedRegularTime { get; set; }
        public int elapsedAddedTime { get; set; }
        public int timeElapsedSeconds { get; set; }
        public FullTimeElapsed fullTimeElapsed { get; set; }
        public string status { get; set; }
        public string matchStatus { get; set; }
    }
}
