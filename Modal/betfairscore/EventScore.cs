using Modal.CommanrtyScore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.betfairscore
{
    public class EventScore
    {


        public int eventid { get; set; }
        public int eventtypeid { get; set; }
        public object score { get; set; }
        public ShortScore shortscore { get; set; }
        public bool isfinished { get; set; }
        public DateTime  showtime { get; set; }
    }
}
