
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal
{
    public class Score
    {
        public Home home { get; set; }
        public Away away { get; set; }
        public int numberOfYellowCards { get; set; }
        public int numberOfRedCards { get; set; }
        public int numberOfCards { get; set; }
        public int numberOfCorners { get; set; }
        public int numberOfCornersFirstHalf { get; set; }
        public int numberOfCornersSecondHalf { get; set; }
        public int bookingPoints { get; set; }
    }
}
