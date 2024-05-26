using System;
using System.Collections.Generic;
using System.Text;

namespace Modal
{
    public class Away
    {
        public string name { get; set; }
        public string score { get; set; }
        public string halfTimeScore { get; set; }
        public string fullTimeScore { get; set; }
        public string penaltiesScore { get; set; }
        public List<object> penaltiesSequence { get; set; }
        public string games { get; set; }
        public string sets { get; set; }
        public List<string> gameSequence { get; set; }
        public bool isServing { get; set; }
        public bool highlight { get; set; }
        public int serviceBreaks { get; set; }
        public int numberOfYellowCards { get; set; }
        public int numberOfRedCards { get; set; }
        public int numberOfCards { get; set; }
        public int numberOfCorners { get; set; }
        public int numberOfCornersFirstHalf { get; set; }
        public int numberOfCornersSecondHalf { get; set; }
        public int bookingPoints { get; set; }

    }
}
