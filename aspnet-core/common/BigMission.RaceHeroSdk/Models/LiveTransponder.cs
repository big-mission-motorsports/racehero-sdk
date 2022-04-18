using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BigMission.RaceHeroSdk.Models
{
    public class LiveTransponder
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }

        [JsonProperty("current_lap")]
        public int CurrentLap { get; set; }

        [JsonProperty("position_in_run")]
        public int PositionInRun { get; set; }

        [JsonProperty("position_in_class")]
        public int PositionInClass { get; set; }

        [JsonProperty("timestamp")]
        public double Timestamp { get; set; }

        [JsonProperty("total_time")]
        public double TotalTime { get; set; }

        [JsonProperty("best_lap_number")]
        public int BestLapNumber { get; set; }

        [JsonProperty("best_lap_time")]
        public double BestLapTime { get; set; }

        [JsonProperty("best_overall")]
        public bool BestOverall { get; set; }

        [JsonProperty("gap_to_ahead_in_run")]
        public double GapToAheadInRun { get; set; }

        [JsonProperty("gap_to_ahead_in_run_laps")]
        public int GapToAheadInRunLaps { get; set; }

        [JsonProperty("gap_to_ahead_in_class")]
        public double GapToAheadInClass { get; set; }

        [JsonProperty("gap_to_ahead_in_class_laps")]
        public int GapToAheadInClassLaps { get; set; }

        [JsonProperty("gap_to_p1_in_run")]
        public double GapToP1InRun { get; set; }

        [JsonProperty("gap_to_p1_in_run_laps")]
        public int GapToP1InRunLaps { get; set; }

        [JsonProperty("gap_to_p1_in_class")]
        public double GapToP1InClass { get; set; }

        [JsonProperty("gap_to_p1_in_class_laps")]
        public int GapToP1InClassLaps { get; set; }

        [JsonProperty("last_lap_time")]
        public double LastLapTime { get; set; }

        [JsonProperty("start_position_in_run")]
        public object StartPositionInRun { get; set; }

        [JsonProperty("start_position_in_class")]
        public object StartPositionInClass { get; set; }
    }

    public class Attributes
    {
        [JsonProperty("attribute0")]
        public object Attribute0 { get; set; }

        [JsonProperty("attribute1")]
        public object Attribute1 { get; set; }

        [JsonProperty("attribute2")]
        public object Attribute2 { get; set; }

        [JsonProperty("attribute3")]
        public object Attribute3 { get; set; }

        [JsonProperty("attribute4")]
        public object Attribute4 { get; set; }

        [JsonProperty("attribute5")]
        public object Attribute5 { get; set; }

        [JsonProperty("attribute6")]
        public object Attribute6 { get; set; }

        [JsonProperty("attribute7")]
        public object Attribute7 { get; set; }

        [JsonProperty("attribute8")]
        public object Attribute8 { get; set; }
    }

}
