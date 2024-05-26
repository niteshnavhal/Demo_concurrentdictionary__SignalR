using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.betfairscore
{
    public class bfscore
    {
        [JsonProperty("eventTypeId")]
        public int EventTypeId { get; set; }

        [JsonProperty("eventId")]
        public int EventId { get; set; }

        //[JsonProperty("score")]
        //public Score Score { get; set; }

        //[JsonProperty("timeElapsed")]
        //public int TimeElapsed { get; set; }

        //[JsonProperty("elapsedRegularTime")]
        //public int ElapsedRegularTime { get; set; }

        //[JsonProperty("hasSets")]
        //public bool HasSets { get; set; }

        //[JsonProperty("timeElapsedSeconds")]
        //public int TimeElapsedSeconds { get; set; }

        //[JsonProperty("fullTimeElapsed")]
        //public FullTimeElapsed FullTimeElapsed { get; set; }

        [JsonProperty("matchStatus")]
        public string MatchStatus { get; set; }
    }
    public class Home
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("halfTimeScore")]
        public string HalfTimeScore { get; set; }

        [JsonProperty("fullTimeScore")]
        public string FullTimeScore { get; set; }

        [JsonProperty("penaltiesScore")]
        public string PenaltiesScore { get; set; }

        [JsonProperty("penaltiesSequence")]
        public List<object> PenaltiesSequence { get; set; }

        [JsonProperty("games")]
        public string Games { get; set; }

        [JsonProperty("sets")]
        public string Sets { get; set; }

        [JsonProperty("highlight")]
        public bool Highlight { get; set; }

        [JsonProperty("numberOfYellowCards")]
        public int NumberOfYellowCards { get; set; }

        [JsonProperty("numberOfRedCards")]
        public int NumberOfRedCards { get; set; }

        [JsonProperty("numberOfCards")]
        public int NumberOfCards { get; set; }

        [JsonProperty("numberOfCorners")]
        public int NumberOfCorners { get; set; }

        [JsonProperty("numberOfCornersFirstHalf")]
        public int NumberOfCornersFirstHalf { get; set; }

        [JsonProperty("numberOfCornersSecondHalf")]
        public int NumberOfCornersSecondHalf { get; set; }

        [JsonProperty("bookingPoints")]
        public int BookingPoints { get; set; }
    }

    public class Away
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("halfTimeScore")]
        public string HalfTimeScore { get; set; }

        [JsonProperty("fullTimeScore")]
        public string FullTimeScore { get; set; }

        [JsonProperty("penaltiesScore")]
        public string PenaltiesScore { get; set; }

        [JsonProperty("penaltiesSequence")]
        public List<object> PenaltiesSequence { get; set; }

        [JsonProperty("games")]
        public string Games { get; set; }

        [JsonProperty("sets")]
        public string Sets { get; set; }

        [JsonProperty("highlight")]
        public bool Highlight { get; set; }

        [JsonProperty("numberOfYellowCards")]
        public int NumberOfYellowCards { get; set; }

        [JsonProperty("numberOfRedCards")]
        public int NumberOfRedCards { get; set; }

        [JsonProperty("numberOfCards")]
        public int NumberOfCards { get; set; }

        [JsonProperty("numberOfCorners")]
        public int NumberOfCorners { get; set; }

        [JsonProperty("numberOfCornersFirstHalf")]
        public int NumberOfCornersFirstHalf { get; set; }

        [JsonProperty("numberOfCornersSecondHalf")]
        public int NumberOfCornersSecondHalf { get; set; }

        [JsonProperty("bookingPoints")]
        public int BookingPoints { get; set; }
    }

    public class Score
    {
        [JsonProperty("home")]
        public Home Home { get; set; }

        [JsonProperty("away")]
        public Away Away { get; set; }

        [JsonProperty("numberOfYellowCards")]
        public int NumberOfYellowCards { get; set; }

        [JsonProperty("numberOfRedCards")]
        public int NumberOfRedCards { get; set; }

        [JsonProperty("numberOfCards")]
        public int NumberOfCards { get; set; }

        [JsonProperty("numberOfCorners")]
        public int NumberOfCorners { get; set; }

        [JsonProperty("numberOfCornersFirstHalf")]
        public int NumberOfCornersFirstHalf { get; set; }

        [JsonProperty("numberOfCornersSecondHalf")]
        public int NumberOfCornersSecondHalf { get; set; }

        [JsonProperty("bookingPoints")]
        public int BookingPoints { get; set; }
    }

    public class FullTimeElapsed
    {
        [JsonProperty("hour")]
        public int Hour { get; set; }

        [JsonProperty("min")]
        public int Min { get; set; }

        [JsonProperty("sec")]
        public int Sec { get; set; }
    }

}
