using Newtonsoft.Json;
using System.Collections.Generic;

namespace BigMission.RaceHeroSdk.Models;

public class Leaderboard
{

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("run_id")]
    public int RunId { get; set; }

    [JsonProperty("run_type")]
    public string RunType { get; set; }

    [JsonProperty("current_lap")]
    public int CurrentLap { get; set; }

    [JsonProperty("started_at")]
    public string StartedAt { get; set; }

    [JsonProperty("current_flag")]
    public string CurrentFlag { get; set; }

    [JsonProperty("laps_remaining")]
    public int LapsRemaining { get; set; }

    [JsonProperty("time_remaining")]
    public string TimeRemaining { get; set; }

    [JsonProperty("current_time")]
    public double CurrentTime { get; set; }

    [JsonProperty("racers")]
    public List<Racer> Racers { get; set; }

}
