using Newtonsoft.Json;

namespace BigMission.RaceHeroSdk.Models;

public class Racer
{

    [JsonProperty("racer_session_id")]
    public int RacerSessionId { get; set; }

    [JsonProperty("racer_name")]
    public string RacerName { get; set; }

    [JsonProperty("racer_number")]
    public string RacerNumber { get; set; }

    [JsonProperty("racer_class_name")]
    public string RacerClassName { get; set; }

    [JsonProperty("position_in_run")]
    public int PositionInRun { get; set; }

    [JsonProperty("current_lap")]
    public int CurrentLap { get; set; }

    [JsonProperty("timestamp")]
    public double Timestamp { get; set; }

    [JsonProperty("total_seconds")]
    public double TotalSeconds { get; set; }

    [JsonProperty("total_time")]
    public string TotalTime { get; set; }

    [JsonProperty("best_lap_number")]
    public int BestLapNumber { get; set; }

    [JsonProperty("best_lap_time")]
    public string BestLapTime { get; set; }

    [JsonProperty("best_lap_time_seconds")]
    public double BestLapTimeSeconds { get; set; }

    [JsonProperty("best_overall")]
    public bool BestOverall { get; set; }

    [JsonProperty("gap_to_ahead_in_run")]
    public string GapToAheadInRun { get; set; }

    [JsonProperty("gap_to_p1_in_class")]
    public string GapToP1InClass { get; set; }

    [JsonProperty("gap_to_p1_in_run")]
    public string GapToP1InRun { get; set; }

    [JsonProperty("last_lap_time")]
    public string LastLapTime { get; set; }

    [JsonProperty("last_lap_time_seconds")]
    public double LastLapTimeSeconds { get; set; }

    [JsonProperty("position_in_class")]
    public int PositionInClass { get; set; }

    [JsonProperty("start_position_in_class")]
    public string StartPositionInClass { get; set; }

    [JsonProperty("start_position_in_run")]
    public int StartPositionInRun { get; set; }

    [JsonProperty("last_pit_lap")]
    public int LastPitLap { get; set; }

    [JsonProperty("pit_stops")]
    public int PitStops { get; set; }

}
