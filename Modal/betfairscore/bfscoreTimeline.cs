using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modal.betfairscore
{
    public class bfscoreTimeline
    {
        [JsonProperty("eventId")]
        public int EventId { get; set; }

        [JsonProperty("eventTypeId")]
        public int EventTypeId { get; set; }

        //[JsonProperty("score")]
        //public ScoreTimeline Score { get; set; }

        //[JsonProperty("timeElapsed")]
        //public int TimeElapsed { get; set; }

        //[JsonProperty("elapsedRegularTime")]
        //public int ElapsedRegularTime { get; set; }

        //[JsonProperty("updateDetails")]
        //public List<UpdateDetail> UpdateDetails { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("inPlayMatchStatus")]
        public string InPlayMatchStatus { get; set; }
    }
    public class HomeTimeline
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

    public class AwayTimeline
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

    public class ScoreTimeline
    {
        [JsonProperty("home")]
        public HomeTimeline Home { get; set; }

        [JsonProperty("away")]
        public AwayTimeline Away { get; set; }

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

    public class UpdateDetail
    {
        [JsonProperty("updateTime")]
        public DateTime UpdateTime { get; set; }

        [JsonProperty("updateId")]
        public int UpdateId { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("teamName")]
        public string TeamName { get; set; }

        [JsonProperty("matchTime")]
        public int MatchTime { get; set; }

        [JsonProperty("elapsedRegularTime")]
        public int ElapsedRegularTime { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("updateType")]
        public string UpdateType { get; set; }
    }

}
