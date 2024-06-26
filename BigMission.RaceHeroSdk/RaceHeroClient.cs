﻿using BigMission.RaceHeroSdk.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace BigMission.RaceHeroSdk;

/// <summary>
/// This client wraps access to Race Hero API resources.
/// </summary>
public class RaceHeroClient(string rootUrl, string apiKey) : IRaceHeroClient
{
    private string RootUrl { get; } = rootUrl;
    private string ApiKey { get; } = apiKey;

    public async Task<Events> GetEvents(int limit = 25, int offset = 0, bool live = false)
    {
        var client = new RestClient(options:new RestClientOptions(RootUrl) { Authenticator = new HttpBasicAuthenticator(ApiKey, "") });
        //client.Authenticator = new HttpBasicAuthenticator(ApiKey, "");
        var request = new RestRequest($"events?limit={limit}&offset={offset}&live={live.ToString().ToLower()}&expand=")
        {
            RequestFormat = DataFormat.Json
        };
        var resp = await client.GetAsync(request);
        return JsonConvert.DeserializeObject<Events>(resp.Content);
    }

    public async Task<Event> GetEvent(string eventId)
    {
        var client = new RestClient(options: new RestClientOptions(RootUrl) { Authenticator = new HttpBasicAuthenticator(ApiKey, "") });
        var request = new RestRequest($"events/{eventId}?expand=")
        {
            RequestFormat = DataFormat.Json
        };
        var resp = await client.GetAsync(request);
        return JsonConvert.DeserializeObject<Event>(resp.Content);
    }

    public async Task<Leaderboard> GetLeaderboard(string eventId)
    {
        var client = new RestClient(options: new RestClientOptions(RootUrl) { Authenticator = new HttpBasicAuthenticator(ApiKey, "") });
        var request = new RestRequest($"events/{eventId}/live/leaderboard")
        {
            RequestFormat = DataFormat.Json
        };
        var resp = await client.GetAsync(request);
        return JsonConvert.DeserializeObject<Leaderboard>(resp.Content);
    }

    public async Task<LiveTransponder> GetLiveTransponder(string transponderId)
    {
        var client = new RestClient(options: new RestClientOptions(RootUrl) { Authenticator = new HttpBasicAuthenticator(ApiKey, "") });
        var request = new RestRequest($"live/transponders/{transponderId}")
        {
            RequestFormat = DataFormat.Json
        };
        var resp = await client.GetAsync(request);
        return JsonConvert.DeserializeObject<LiveTransponder>(resp.Content);
    }

    public enum Flag { Unknown, Green, Yellow, Red, Warmup, Finish, Stop }
    public static Flag ParseFlag(string flag)
    {
        if (string.IsNullOrWhiteSpace(flag))
        {
            return Flag.Unknown;
        }

        var s = flag.Trim().ToUpper();
        if (s == "GREEN")
        {
            return Flag.Green;
        }
        if (s == "YELLOW")
        {
            return Flag.Yellow;
        }
        if (s == "RED")
        {
            return Flag.Red;
        }
        if (s == "FINISH")
        {
            return Flag.Finish;
        }
        if (s == "STOP")
        {
            return Flag.Stop;
        }
        if (s == "WARMUP")
        {
            return Flag.Warmup;
        }
        return Flag.Unknown;
    }
}
